namespace Kingfar.BioFeedback.Mvvm;

/// <summary>
/// 放主要属性、Service及方法 由于用到了CommunityToolkit.Mvvm所以无法拆分多个文件
/// </summary>
public partial class OrganizationViewModel : PaginationViewModelBase<AppUserDto>
{
    #region Service

    private readonly IAccountService _accountService;

    #endregion Service

    #region Property

    private int? _searchSexIndex;
    private string _searchText = string.Empty;

    public int? SearchSexIndex
    {
        get { return _searchSexIndex; }
        set { _searchSexIndex = value; this.OnPropertyChanged("_searchSexIndex"); }
    }

    public string SearchText
    {
        get { return _searchText; }
        set { _searchText = value; this.OnPropertyChanged("SearchText"); }
    }

    #endregion Property

    public OrganizationViewModel(IAccountService accountService)
    {
        _accountService = accountService;
        CurrentPageChanged(true);
    }

    public override void ClearItem()
    {
        SearchText = string.Empty;
        SearchSexIndex = null;
    }

    public override void CurrentPageChanged(bool isNeedTotalCount = false)
    {
        var result = _accountService.GetListByPage((Current - 1) * CountPerPage, CountPerPage, AppUserType.Main, SearchText, SearchSexIndex == null ? null : SearchSexIndex == 0, isNeedTotalCount);
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

    [RelayCommand]
    private async Task Delete(AppUserDto dto)
    {
        await DeleteData($"确定要删除 {dto.Name} 吗？", () => _accountService.TombstonedByIdAsync(dto.Id!.Value));
    }
}

/// <summary>
/// 添加组织成员相关属性及方法
/// </summary>
public partial class OrganizationViewModel : PaginationViewModelBase<AppUserDto>
{
    #region DialogProperty

    [ObservableProperty]
    private object? _addDialog;

    [ObservableProperty]
    private DateTime _birthday = DateTime.Now;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private bool _isMale = true;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private Visibility _passwordVisibility = Visibility.Visible;

    [ObservableProperty]
    private string _phone = string.Empty;

    [ObservableProperty]
    private string _userName = string.Empty;

    #endregion DialogProperty

    [RelayCommand]
    private async Task AddOrUpdate(object? userId)
    {
        AppUserDto editorUser = await InitDialogProperty(userId);

        var result = await ShowDialogAsync(userId is not null ? "编辑成员" : "添加成员", AddDialog, () => CheckAddData(editorUser.Id));
        if (result == ContentDialogResult.Primary)
        {
            editorUser.SetValues(Name, UserName, Password, Phone, IsMale, Birthday, Description);
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
            errorMessage = "用户名不能为空";
            result = false;
        }
        else if (!string.IsNullOrWhiteSpace(UserName) && await _accountService.ExistsByUserNameAsync(UserName, id, AppUserType.Admin))
        {
            errorMessage = "用户名已存在";
            result = false;
        }
        else if (string.IsNullOrWhiteSpace(Password))
        {
            errorMessage = "密码不能为空";
            result = false;
        }
        else if (string.IsNullOrWhiteSpace(Phone))
        {
            errorMessage = "联系方式不能为空";
            result = false;
        }
        else if (!string.IsNullOrWhiteSpace(Phone) && await _accountService.ExistsByPhoneAsync(Phone, id, AppUserType.Admin))
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
        Password = user?.Password ?? string.Empty;
        Phone = user?.Phone ?? string.Empty;
        IsMale = user?.IsMale ?? true;
        Birthday = user?.Birthday ?? DateTime.Now;
        Description = user?.Description ?? string.Empty;
        PasswordVisibility = user is null ? Visibility.Visible : Visibility.Collapsed;
    }
}

/// <summary>
/// 修改相关属性及方法
/// </summary>
public partial class OrganizationViewModel : PaginationViewModelBase<AppUserDto>
{
    #region Property

    [ObservableProperty]
    private object? _changePwdDialog;

    [ObservableProperty]
    private string _checkPassword = string.Empty;

    [ObservableProperty]
    private string _newPassword = string.Empty;

    #endregion Property

    [RelayCommand]
    private async Task ChangePassword(object? userId)
    {
        var result = await ShowDialogAsync("修改密码", ChangePwdDialog, CheckChangPasswordData);
        if (result == ContentDialogResult.Primary)
        {
            if (userId is not null && !string.IsNullOrWhiteSpace(userId.ToString()) && Guid.TryParse(userId!.ToString(), out Guid id))
            {
                var editorUser = await _accountService.GetByIdAsync(id);
                editorUser.Password = NewPassword.GetMd5Hash();
                var userDto = await _accountService.AddOrUpdateUserAsync(editorUser);

                if (userDto is not null)
                {
                    SnackbarService.Show("提示", "密码修改成功", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
                    CurrentPageChanged(true);
                }
            }
        }
    }

    private bool CheckChangPasswordData()
    {
        var result = true;
        string errorMessage = string.Empty;
        if (string.IsNullOrWhiteSpace(NewPassword))
        {
            errorMessage = "新密码不能为空";
            result = false;
        }
        else if (string.IsNullOrWhiteSpace(CheckPassword))
        {
            errorMessage = "校验密码不能为空";
            result = false;
        }
        else if (NewPassword != CheckPassword)
        {
            errorMessage = "新密码与校验不一致";
            result = false;
        }

        if (!result)
        {
            SnackbarService.Show("错误", errorMessage, ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
        }
        return result;
    }
}