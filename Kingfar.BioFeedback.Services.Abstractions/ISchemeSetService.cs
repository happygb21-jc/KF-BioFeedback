namespace Kingfar.BioFeedback.Services;

public interface ISchemeSetService
{
    PageModel<SchemeSetDto> GetListByPage(int skipCount, int pageSize, string? name, SchemeSetType? schemeType, bool? isPublished, bool isNeedTotalCount = false, bool isDeleted = false);

    Task<bool> TombstonedByIdAsync(Guid id);

    Task<SchemeSetDto?> AddOrUpdateTrainAsync(AddOrUpdateSchemeSetDto train);

    Task<bool> CopyToTemplateAsync(Guid id, Guid? targetId = null);

    Task<Guid?> CheckAssociatedAsync(Guid id);

    Task<bool> RestoreAsync(Guid id);

    Task<bool> DeleteByIdAsync(Guid id);

    Task<SchemeSetDetailDto> GetByIdAsync(Guid id);

    Task<bool> ChangePublishStateAsync(Guid id, bool isPublished);
}