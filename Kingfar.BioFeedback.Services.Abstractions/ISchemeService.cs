namespace Kingfar.BioFeedback.Services;

public interface ISchemeService
{
    Task<SchemeDto?> AddOrUpdateTrainAsync(AddOrUpdateSchemeDto train);

    PageModel<SchemeDto> GetListByPage(int skipCount, int pageSize, string searchText, List<TrainType> trainTypes, List<TrainApplicationType> trainApplicationTypes, bool isNeedTotalCount = false);

    Task<T> GetByIdAsync<T>(Guid id) where T : class;

    Task<bool> TombstonedByIdAsync(Guid id);

    Task<SchemeDto> RnameAsync(Guid id, string name);

    Task<bool> ExistsByNameAsync(string newName, Guid? id = null);
}