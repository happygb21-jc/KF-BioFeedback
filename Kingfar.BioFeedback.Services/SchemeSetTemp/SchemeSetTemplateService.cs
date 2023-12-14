namespace Kingfar.BioFeedback.Services
{
    public class SchemeSetTemplateService : ServiceBase, ISchemeSetTemplateService
    {
        private readonly ISqlSugarRepository<SchemeSetTemplate> _schemeSetTempRepository;
        private readonly ISqlSugarRepository<SchemeSet> _schemeSetRepository;

        public SchemeSetTemplateService(ISqlSugarRepository<SchemeSetTemplate> schemeSetTempRepository, ISqlSugarRepository<SchemeSet> schemeSetRepository)
        {
            _schemeSetTempRepository = schemeSetTempRepository;
            _schemeSetRepository = schemeSetRepository;
        }

        public async Task<bool> CreateToSchemeSetAsync(Guid id, string schemeSetName)
        {
            var schemeSetTemp = await _schemeSetTempRepository.AsQueryable().Includes(e => e.Trains).FirstAsync(e => e.Id == id);
            var schemeSet = schemeSetTemp.CreateToSchemeSet(schemeSetName);
            for (int i = 0; i < schemeSetTemp.Trains.Count; i++)
            {
                var trainId = Guid.NewGuid();
                var trainTemp = schemeSetTemp.Trains[i];
                var newTrain = new Train()
                {
                    Id = trainId,
                    PublishSettings = trainTemp.PublishSettings,
                    Content = new TrainContent() { Experiments = trainTemp.Experiments },
                    FolderPath = FilePathConst.GetTrainConfigPath(schemeSet.CurrentPath)
                };
                schemeSet.Trains.Add(newTrain);
                FolderCopyHelper.CopyDirectory(trainTemp.CurrentPath, newTrain.CurrentPath, false);
            }
            return await _schemeSetRepository.AsSugarClient().InsertNav(schemeSet).Include(e => e.Trains).ThenInclude(t => t.Content).ExecuteCommandAsync();
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            //删除所有当前方案的文件，包含报告等数据
            Directory.Delete(FilePathConst.GetSchemeSetTempConfigPath(id), true);
            return await _schemeSetTempRepository.DeleteByIdAsync(id);
        }

        public PageModel<SchemeSetTemplateDto> GetListByPage(int skipCount, int pageSize, string? name, SchemeSetType? schemeType, bool isNeedTotalCount = false)
        {
            var query = _schemeSetTempRepository.AsQueryable()
              .WhereIF(
                ApplicationContext.CurrentUser != null && ApplicationContext.CurrentUser.UserType != AppUserType.Admin,
                e => e.CreatorId == ApplicationContext.CurrentUser!.Id
              )
              .WhereIF(schemeType.HasValue, e => e.Type == schemeType)
              .WhereIF(!string.IsNullOrEmpty(name), e => e.Name.Contains(name!));
            int totalCount = 0;
            if (isNeedTotalCount) totalCount = query.Count();
            var reslut = query.OrderByDescending(e => e.CreationTime).Skip(skipCount).Take(pageSize).ToList();
            return new PageModel<SchemeSetTemplateDto>(totalCount, AppMapper.Map<List<SchemeSetTemplateDto>>(reslut));
        }
    }
}