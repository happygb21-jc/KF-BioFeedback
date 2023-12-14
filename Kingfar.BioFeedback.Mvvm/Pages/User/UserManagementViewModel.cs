namespace Kingfar.BioFeedback.Mvvm.User
{
    public partial class UserManagementViewModel : PaginationViewModelBase<AppUserDto>
    {
        #region Service

        private readonly IAccountService _accountService;

        #endregion Service

        #region Property

        #region Search

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private int? _searchSexIndex = default!;

        #endregion Search

        #endregion Property

        public UserManagementViewModel(IAccountService accountService)
        {
            _accountService = accountService;
            CurrentPageChanged(true);
        }

        [RelayCommand]
        private async Task Delete(AppUserDto dto)
        {
            await DeleteData($"确定要删除 {dto.Name} 吗？", () => _accountService.TombstonedByIdAsync(dto.Id!.Value));
        }

        public override void ClearItem()
        {
            SearchText = string.Empty;
            SearchSexIndex = null;
        }

        public override void CurrentPageChanged(bool isNeedTotalCount = false)
        {
            var result = _accountService.GetListByPage((Current - 1) * CountPerPage, CountPerPage, AppUserType.Subject, SearchText, SearchSexIndex == null ? null : SearchSexIndex == 0, isNeedTotalCount);
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (isNeedTotalCount)
                {
                    TotalCount = result.TotalCount;
                    if (Current != 1)
                        Current = 1;
                }
                Source = result.Data;
            });
        }
    }

    public partial class UserManagementViewModel : PaginationViewModelBase<AppUserDto>
    {
        #region DialogProperty

        [ObservableProperty]
        private object _addDialog;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _userName = string.Empty;

        [ObservableProperty]
        private string _phone = string.Empty;

        [ObservableProperty]
        private bool _isMale = true;

        [ObservableProperty]
        private string _description = string.Empty;

        #endregion DialogProperty

        [RelayCommand]
        private async Task AddOrUpdate(object? userId)
        {
            AppUserDto editorUser = await InitDialogProperty(userId);

            ContentDialogResult result = await ShowDialogAsync(userId is not null ? "编辑患者" : "添加患者", AddDialog, () => CheckAddData(editorUser.Id));
            if (result == ContentDialogResult.Primary)
            {
                editorUser.SetValues(Name, UserName, Phone, IsMale, Description);
                var userDto = await _accountService.AddOrUpdateUserAsync(editorUser);
                if (userDto is not null)
                {
                    SnackbarService.Show("提示", "添加成功", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
                    CurrentPageChanged(true);
                }
            }
            else
            {
                SetDialogProperty();
            }
        }

        private async Task<AppUserDto> InitDialogProperty(object? userId)
        {
            AppUserDto? editorUser = null;
            if (userId is not null && !string.IsNullOrWhiteSpace(userId.ToString()) && Guid.TryParse(userId!.ToString(), out Guid id))
                editorUser = await _accountService.GetByIdAsync(id);
            SetDialogProperty(editorUser);
            return editorUser is null ? new AppUserDto() : editorUser;
        }

        private void SetDialogProperty(AppUserDto? user = null)
        {
            Name = user?.Name ?? string.Empty;
            UserName = user?.UserName ?? string.Empty;
            Phone = user?.Phone ?? string.Empty;
            IsMale = user?.IsMale ?? true;
            Description = user?.Description ?? string.Empty;
        }

        private async Task<bool> CheckAddData(Guid? id)
        {
            var result = true;
            string errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(Name))
            {
                errorMessage = "姓名不能为空";
                result = false;
            }
            else if (string.IsNullOrWhiteSpace(UserName))
            {
                errorMessage = "身份证号不能为空";
                result = false;
            }
            else if (!string.IsNullOrWhiteSpace(UserName) && await _accountService.ExistsByUserNameAsync(UserName, id))
            {
                errorMessage = "身份证号已存在";
                result = false;
            }
            else if (string.IsNullOrWhiteSpace(Phone))
            {
                errorMessage = "联系方式不能为空";
                result = false;
            }
            else if (!string.IsNullOrWhiteSpace(Phone) && await _accountService.ExistsByUserNameAsync(Phone, id))
            {
                errorMessage = "联系方式已存在";
                result = false;
            }

            if (!result)
            {
                SnackbarService.Show("错误", errorMessage, ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
            }
            return result;
        }
    }
}