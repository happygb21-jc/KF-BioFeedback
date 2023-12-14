using Kingfar.BioFeedback.Shared;

namespace Kingfar.BioFeedback.Services
{
    public class AccountService : ServiceBase, IAccountService
    {
        private ISqlSugarRepository<AppUser> _userRepository;
        private ISqlSugarRepository<TrainResult> _trainResultRepository;

        public AccountService(ISqlSugarRepository<AppUser> userRepository, ISqlSugarRepository<TrainResult> trainResultRepository)
        {
            _userRepository = userRepository;
            _trainResultRepository = trainResultRepository;
        }

        public async Task<AppUserDto?> AddOrUpdateUserAsync(AppUserDto appUser)
        {
            AppUser user;
            if (appUser.Id.HasValue)
            {
                user = _userRepository.GetById(appUser.Id.Value);
                user.SetData(appUser.Name, appUser.UserName, appUser.Phone, appUser.IsMale);
                if (!string.IsNullOrWhiteSpace(appUser.Password))
                    user.Password = appUser.Password.GetMd5Hash();
            }
            else
            {
                user = new AppUser(appUser.Name, appUser.UserName, appUser.Password, appUser.Phone, appUser.IsMale, appUser.Birthday, appUser.Description, appUser.UserType);
            }
            await _userRepository.InsertOrUpdateAsync(user);

            return AppMapper.Map<AppUserDto>(user);
        }

        public async Task<AppUserDto> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return AppMapper.Map<AppUserDto>(user);
        }

        public List<AppUserDto> GetList(string? name, bool? isMale)
        {
            var reslut = _userRepository.AsQueryable()
                .Where(e => !e.IsDeleted)
                .WhereIF(!string.IsNullOrEmpty(name), e => e.UserName.Contains(name!) || e.Name.Contains(name!))
                .WhereIF(isMale.HasValue, e => e.IsMale == isMale!.Value).ToList();
            return AppMapper.Map<List<AppUserDto>>(reslut);
        }

        public PageModel<AppUserDto> GetListByPage(int skipCount, int pageSize, AppUserType appUserType, string? name, bool? isMale, bool isNeedTotalCount = false, bool isDeleted = false)
        {
            var query = _userRepository.AsQueryable();
            if (isDeleted) query = query.ClearFilter();
            query = query.Mapper(u => u.Creator, u => u.CreatorId)
            .Where(e => e.UserType == appUserType && e.IsDeleted == isDeleted)
            .WhereIF(ApplicationContext.CurrentUser != null && ApplicationContext.CurrentUser.UserType != AppUserType.Admin, e => e.CreatorId == ApplicationContext.CurrentUser!.Id)
            .WhereIF(!string.IsNullOrEmpty(name), e => e.UserName.Contains(name!) || e.Name.Contains(name!))
            .WhereIF(isMale.HasValue, e => e.IsMale == isMale!.Value);

            int totalCount = 0;
            if (isNeedTotalCount) totalCount = query.Count();
            var reslut = query.OrderByDescending(e => e.CreationTime).Skip(skipCount).Take(pageSize).ToList();
            return new PageModel<AppUserDto>(totalCount, AppMapper.Map<List<AppUserDto>>(reslut));
        }

        public bool Login(string userName, string password)
        {
            var user = _userRepository.GetFirst(e => e.UserName == userName && e.Password == password.GetMd5Hash());
            if (user != null)
            {
                ApplicationContext.SetCurrentUser(user);
                return true;
            }
            return false;
        }

        public async Task<bool> LoginAsync(string userName, string password)
        {
            var user = await _userRepository.GetFirstAsync(e => e.UserName == userName && e.Password == password.GetMd5Hash());
            if (user != null)
            {
                ApplicationContext.SetCurrentUser(user);
                return true;
            }
            return false;
        }

        public async Task<bool> TombstonedByIdAsync(Guid id)
        {
            var entity = await _userRepository.GetByIdAsync(id);
            entity.IsDeleted = true;
            return _userRepository.Update(entity);
        }

        public async Task<bool> ExistsByUserNameAsync(string userName, Guid? excludeId = null, AppUserType appUserType = AppUserType.Subject)
        {
            return await _userRepository.AsQueryable()
                 .Where(e => e.UserType == appUserType).Where(e => !e.IsDeleted)
                 .WhereIF(excludeId != null && excludeId != Guid.Empty, e => e.Id != excludeId!.Value).CountAsync(e => e.UserName == userName) > 0;
        }

        public async Task<bool> ExistsByPhoneAsync(string phone, Guid? excludeId = null, AppUserType appUserType = AppUserType.Subject)
        {
            return await _userRepository.AsQueryable()
                .Where(e => e.UserType == appUserType).Where(e => !e.IsDeleted)
                .WhereIF(excludeId != null && excludeId != Guid.Empty, e => e.Id != excludeId!.Value).CountAsync(e => e.Phone == phone) > 0;
        }

        public async Task<bool> DeleteByIdAsync(Guid id, AppUserType appUserType)
        {
            //删除用户的所有报告

            var results = _trainResultRepository.AsQueryable().Includes(e => e.Train).Where(e => e.UserId == id).ToArray();
            foreach (var result in results)
            {
                Directory.Delete(result.CurrentPath, true);
            }
            //删除测试接口表数据
            await _trainResultRepository.DeleteAsync(e => e.UserId == id);
            //删除用户
            return await _userRepository.DeleteAsync(e => e.Id == id && e.UserType == appUserType);
        }

        public Task<bool> RestoreAsync(Guid id)
        {
            var entity = _userRepository.GetById(id);
            entity.IsDeleted = false;
            return _userRepository.UpdateAsync(entity);
        }
    }
}