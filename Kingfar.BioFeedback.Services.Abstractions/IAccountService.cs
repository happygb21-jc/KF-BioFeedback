namespace Kingfar.BioFeedback.Services;

public interface IAccountService
{
    bool Login(string userName, string password);

    Task<bool> LoginAsync(string userName, string password);

    Task<AppUserDto?> AddOrUpdateUserAsync(AppUserDto appUser);

    Task<AppUserDto> GetByIdAsync(Guid id);

    List<AppUserDto> GetList(string? name, bool? isMale);

    PageModel<AppUserDto> GetListByPage(int skipCount, int pageSize, AppUserType appUserType, string? name, bool? isMale, bool isNeedTotalCount = false, bool isDeleted = false);

    Task<bool> TombstonedByIdAsync(Guid id);

    Task<bool> ExistsByUserNameAsync(string userName, Guid? excludeId = null, AppUserType appUserType = AppUserType.Subject);

    Task<bool> ExistsByPhoneAsync(string phone, Guid? excludeId = null, AppUserType appUserType = AppUserType.Subject);

    Task<bool> RestoreAsync(Guid id);

    Task<bool> DeleteByIdAsync(Guid id, AppUserType appUserTyp);
}