using Kingfar.BioFeedback.Shared;

namespace Kingfar.BioFeedback.Services;

public interface ISchemeSetTemplateService
{
    PageModel<SchemeSetTemplateDto> GetListByPage(int skipCount, int pageSize, string? name, SchemeSetType? schemeType, bool isNeedTotalCount = false);

    Task<bool> CreateToSchemeSetAsync(Guid id, string schemeSetName);

    Task<bool> DeleteByIdAsync(Guid id);
}