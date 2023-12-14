using Newtonsoft.Json;
using SqlSugar;

namespace Kingfar.BioFeedback.Services
{
    public class SchemeService : ServiceBase, ISchemeService
    {
        private readonly ISqlSugarRepository<Scheme> _schemeRepository;

        private readonly ISqlSugarRepository<SchemeAppType> _schemeAppTypeRepository;

        public SchemeService(ISqlSugarRepository<Scheme> schemeRepository, ISqlSugarRepository<SchemeAppType> schemeAppTypeRepository)
        {
            _schemeRepository = schemeRepository;
            _schemeAppTypeRepository = schemeAppTypeRepository;
        }

        public async Task<SchemeDto?> AddOrUpdateTrainAsync(AddOrUpdateSchemeDto train)
        {
            bool isCreate = !train.Id.HasValue;
            Guid id = train.Id ?? Guid.NewGuid();

            Scheme entity = isCreate ? new Scheme() :
                _schemeRepository.AsQueryable().Includes(e => e.ApplicationTypes).First(e => e.Id == id);
            entity.SetValue(train.Name, train.Type, train.ApplicationTypes.Select(e => new SchemeAppType() { Type = e }).ToList());
            entity.FolderPath = FilePathConst.SchemePath;
            if (!isCreate)
            {
                await _schemeAppTypeRepository.DeleteAsync(e => e.SchemeId == id);
                await _schemeRepository.AsSugarClient().UpdateNav(entity).Include(e => e.ApplicationTypes).ExecuteCommandAsync();
            }
            else
            {
                //数据来源于后续编辑参数后通过tar中进行文件读取
                //发布信息可以默认，但是后续考虑单人多次实验如何分组
                entity.PublishSettings = new Entities.PublishSettings();
                entity.Content = new SchemeContent() { Experiments = JsonConvert.DeserializeObject<List<Experiment>>("[{\"id\":\"1cumdd16jm\",\"name\":\"线条判断实验\",\"type\":4,\"icon\":14,\"isEmpty\":false,\"rank\":0,\"stats\":[{\"blockName\":\"线条判断\",\"type\":19,\"name\":\"平均绝对误差长度(px)\"},{\"blockName\":\"线条判断\",\"type\":12,\"name\":\"平均相对误差(%)\"}],\"requirements\":[5],\"cover\":[{\"name\":\"线条判断\",\"value\":14}],\"usedBlockFormTypes\":[14],\"fileNames\":[]},{\"id\":\"1cumg43md6\",\"name\":\"反应时范式\",\"type\":3,\"icon\":3,\"isEmpty\":false,\"rank\":1,\"stats\":[{\"blockName\":\"试次组\",\"type\":0,\"name\":\"正确率(%)\"},{\"blockName\":\"试次组\",\"type\":1,\"name\":\"错误率(%)\"},{\"blockName\":\"试次组\",\"type\":2,\"name\":\"漏报率(%)\"},{\"blockName\":\"试次组\",\"type\":3,\"name\":\"平均反应时(s)\"},{\"blockName\":\"试次组\",\"type\":4,\"name\":\"平均正确反应时(s)\"}],\"requirements\":[5],\"cover\":[{\"name\":\"指导语\",\"value\":3},{\"name\":\"试次组\",\"value\":4}],\"usedBlockFormTypes\":[],\"fileNames\":[\"1cumgysjqw.png\",\"1cumh2qgbr.png\"]}]") };
                await _schemeRepository.AsSugarClient().InsertNav(entity).IncludesAllFirstLayer().ExecuteCommandAsync();//.Include(e => e.ApplicationTypes).Include(e => e.Content)
            }
            string filePath = entity.FolderPath.CombinePath(entity.Id.ToString());
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            return AppMapper.Map<SchemeDto>(entity);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> TombstonedByIdAsync(Guid id)
        {
            var entity = await _schemeRepository.GetByIdAsync(id);
            entity.IsDeleted = true;
            return await _schemeRepository.UpdateAsync(entity);
        }

        public async Task<SchemeDto> RnameAsync(Guid id, string name)
        {
            var scheme = await _schemeRepository.GetByIdAsync(id);
            scheme.Name = name;
            return AppMapper.Map<SchemeDto>(scheme);
        }

        public async Task<T> GetByIdAsync<T>(Guid id) where T : class
        {
            var scheme = await _schemeRepository.AsQueryable()
                .Includes(e => e.ApplicationTypes).FirstAsync(e => e.Id == id);
            return AppMapper.Map<T>(scheme);
        }

        public PageModel<SchemeDto> GetListByPage(int skipCount, int pageSize, string searchText, List<TrainType> trainTypes, List<TrainApplicationType> trainApplicationTypes, bool isNeedTotalCount = false)
        {
            var query = _schemeRepository.AsQueryable()
                .Includes(e => e.ApplicationTypes)
                .WhereIF(!string.IsNullOrWhiteSpace(searchText), e => e.Name.Contains(searchText))
                .WhereIF(trainTypes.Count > 0, e => trainTypes!.Contains(e.Type))
                .WhereIF(trainApplicationTypes.Count > 0, e => e.ApplicationTypes.Any(t => trainApplicationTypes!.Contains(t.Type)));
            int totalCount = 0;
            if (isNeedTotalCount) totalCount = query.Count();
            var reslut = query.OrderByDescending(e => e.CreationTime).Skip(skipCount).Take(pageSize).ToList();
            return new PageModel<SchemeDto>(totalCount, AppMapper.Map<List<SchemeDto>>(reslut));
        }

        public async Task<bool> ExistsByNameAsync(string newName, Guid? id)
        {
            return await _schemeRepository.AsQueryable()
                .WhereIF(id != null && id != Guid.Empty, e => e.Id == id!.Value)
                .CountAsync(e => e.Name == newName) > 1;
        }
    }
}