using System.IO;

namespace Kingfar.BioFeedback.Services
{
    public class SchemeSetService : ServiceBase, ISchemeSetService
    {
        private readonly ISqlSugarRepository<SchemeSet> _schemeSetRespository;
        private readonly ISqlSugarRepository<Scheme> _schemeRespository;
        private readonly ISqlSugarRepository<SchemeSetTemplate> _schemeSetTemplateRespository;
        private readonly ISqlSugarRepository<AssociatedData> _associatedDataRespository;

        public SchemeSetService(ISqlSugarRepository<SchemeSet> schemeSetRespository,
            ISqlSugarRepository<SchemeSetTemplate> schemeSetTemplateRespository,
            ISqlSugarRepository<Scheme> schemeRespository,
            ISqlSugarRepository<AssociatedData> associatedDataRespository)
        {
            _schemeSetRespository = schemeSetRespository;
            _schemeRespository = schemeRespository;
            _schemeSetTemplateRespository = schemeSetTemplateRespository;
            _associatedDataRespository = associatedDataRespository;
        }

        public async Task<SchemeSetDto?> AddOrUpdateTrainAsync(AddOrUpdateSchemeSetDto dto)
        {
            var schemeSet = new SchemeSet() { Id = Guid.NewGuid(), Type = dto.Type, Name = dto.Name, Trains = new List<Train>() };
            schemeSet.FolderPath = FilePathConst.SchemeSetPath;
            string filePath = schemeSet.FolderPath.CombinePath(schemeSet.Id.ToString());
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            var schemes = await _schemeRespository.AsQueryable().Includes(e => e.Content)
                .Where(e => dto.SchemeIds.Contains(e.Id))
                .OrderBy(item => Array.IndexOf(dto.SchemeIds, item.Id)).ToArrayAsync();

            for (int i = 0; i < schemes.Length; i++)
            {
                var trainId = Guid.NewGuid();
                var scheme = schemes[i];
                var newTrain = scheme.CopyToTrain(trainId, i, FilePathConst.GetTrainConfigPath(schemeSet.FolderPath));
                schemeSet.Trains.Add(newTrain);
                FolderCopyHelper.CopyDirectory(scheme.CurrentPath, newTrain.CurrentPath, false);
            }
            await _schemeSetRespository.AsSugarClient().InsertNav(schemeSet).Include(e => e.Trains).ThenInclude(t => t.Content).ExecuteCommandAsync();
            return AppMapper.Map<SchemeSetDto>(schemeSet);
        }

        public async Task<Guid?> CheckAssociatedAsync(Guid id)
        {
            var associatedData = await _associatedDataRespository.AsQueryable().FirstAsync(e => e.SourceName == nameof(SchemeSet) && e.SourceId == id && e.TargetName == nameof(SchemeSetTemplate));
            return associatedData?.TargetId;
        }

        public async Task<bool> CopyToTemplateAsync(Guid id, Guid? targetId = null)
        {
            bool result = false;
            if (targetId.HasValue)
            {
                Directory.Delete(FilePathConst.GetSchemeSetTempConfigPath(targetId.Value), true);
            }
            var schemeSet = await _schemeSetRespository.AsQueryable().Includes(e => e.Trains, t => t.Content).FirstAsync(e => e.Id == id);
            var schemeSetTemp = schemeSet.CopyToTemplate(targetId, FilePathConst.SchemeTemplatePath);
            for (int i = 0; i < schemeSet.Trains.Count; i++)
            {
                var trainTempId = Guid.NewGuid();
                var train = schemeSet.Trains[i];
                string trainPath = FilePathConst.GetTrainTemplateConfigPath(schemeSetTemp.CurrentPath);
                schemeSetTemp.Trains.Add(new TrainTemplate()
                {
                    Id = trainTempId,
                    Name = train.Name,
                    PublishSettings = train.PublishSettings,
                    Experiments = train.Content.Experiments,
                    Rank = i,
                    FolderPath = trainPath
                });
                FolderCopyHelper.CopyDirectory(
                    train.FolderPath,
                    trainPath, false);
            }

            if (!targetId.HasValue)
            {
                result = _schemeSetTemplateRespository.AsSugarClient().InsertNav(schemeSetTemp).Include(e => e.Trains).ExecuteCommand();
                _associatedDataRespository.Insert(new AssociatedData(nameof(SchemeSet), id, nameof(SchemeSetTemplate), schemeSetTemp.Id));
            }
            else
                result = true;

            return result;
        }

        public Task<bool> TombstonedByIdAsync(Guid id)
        {
            var entity = _schemeSetRespository.GetById(id);
            entity.IsDeleted = true;
            return _schemeSetRespository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            //删除所有当前方案的文件，包含报告等数据
            Directory.Delete(FilePathConst.GetSchemeSetConfigPath(id), true);
            //删除所有相关数据
            return await _schemeSetRespository.AsSugarClient().DeleteNav<SchemeSet>(e => e.Id == id).IncludesAllFirstLayer().ExecuteCommandAsync();
        }

        public Task<bool> RestoreAsync(Guid id)
        {
            var entity = _schemeSetRespository.GetById(id);
            entity.IsDeleted = false;
            return _schemeSetRespository.UpdateAsync(entity);
        }

        public PageModel<SchemeSetDto> GetListByPage(int skipCount, int pageSize, string? name, SchemeSetType? schemeType, bool? isPublished, bool isNeedTotalCount = false, bool isDeleted = false)
        {
            var query = _schemeSetRespository.AsQueryable();
            if (isDeleted) query = query.ClearFilter();
            query = query.Where(e => e.IsDeleted == isDeleted)
              .WhereIF(ApplicationContext.CurrentUser != null && ApplicationContext.CurrentUser.UserType != AppUserType.Admin, e => e.CreatorId == ApplicationContext.CurrentUser!.Id)
              .WhereIF(schemeType.HasValue, e => e.Type == schemeType)
              .WhereIF(!string.IsNullOrEmpty(name), e => e.Name.Contains(name!))
              .WhereIF(isPublished.HasValue, e => e.IsPublished == isPublished!.Value);
            int totalCount = 0;
            if (isNeedTotalCount) totalCount = query.Count();
            var reslut = query.OrderByDescending(e => e.CreationTime).Skip(skipCount).Take(pageSize).ToList();
            return new PageModel<SchemeSetDto>(totalCount, AppMapper.Map<List<SchemeSetDto>>(reslut));
        }

        public async Task<SchemeSetDetailDto> GetByIdAsync(Guid id)
        {
            var schemeSet = await _schemeSetRespository.AsQueryable().Includes(e => e.Trains, t => t.Content).FirstAsync(e => e.Id == id);

            return AppMapper.Map<SchemeSetDetailDto>(schemeSet);
        }

        public async Task<bool> ChangePublishStateAsync(Guid id, bool isPublished)
        {
            var entity = await _schemeSetRespository.GetByIdAsync(id);
            entity.IsPublished = isPublished;
            return await _schemeSetRespository.UpdateAsync(entity);
        }
    }
}