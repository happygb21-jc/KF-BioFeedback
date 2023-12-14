using System;
using Kingfar.BioFeedback.Shared.Abstractions;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Mvvm;

public partial class MainViewModel : ViewModelBase
{
    #region Service

    private bool _isInitialized = false;
    private readonly IAccountService _accountService;
    private readonly ISnackbarService _snackbarService;
    private readonly IContentDialogService _contentDialogService;

    #endregion Service

    #region Property

    [ObservableProperty]
    private string _applicationTitle = String.Empty;

    [ObservableProperty]
    private ICurrentUser _loginUser;

    [ObservableProperty]
    private ObservableCollection<object> _navigationItems;

    [ObservableProperty]
    private ObservableCollection<object> _navigationFooter;

    [ObservableProperty]
    private ObservableCollection<object> _trayMenuItems;

    #endregion Property

    private readonly IApplicationContext _appliationContext;

    public MainViewModel(IApplicationContext appliationContext, IAccountService accountService, ISnackbarService snackbarService, IContentDialogService contentDialogService)
    {
        _appliationContext = appliationContext;
        if (!_isInitialized) InitializeViewModel();
        _accountService = accountService;
        _snackbarService = snackbarService;
        _contentDialogService = contentDialogService;
        LoginUser = _appliationContext.CurrentUser!;
    }

    private void InitializeViewModel()
    {
        ApplicationTitle = _appliationContext.SystemInfo.SoftwareName;

        NavigationItems = new ObservableCollection<object>
        {
            new NavigationViewItemSeparator(),
            new NavigationViewItem()
            {
                Content = "数据管理",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home20 },
                TargetPageTag = AppViews.Dashboard,
                TargetPageType=GetRegisteredType(AppViews.Dashboard),
            },
            new NavigationViewItemSeparator(),
            new NavigationViewItem()
            {
                Content = "方案管理",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Group20 },
                TargetPageTag = AppViews.SchemeSet,
                TargetPageType=GetRegisteredType(AppViews.SchemeSet)
            },
            new NavigationViewItemSeparator(),
            new NavigationViewItem()
            {
                Content = "模板管理",
                Icon = new SymbolIcon { Symbol = SymbolRegular.BookTemplate20 },
                TargetPageTag = AppViews.SchemeSetTemplate,
                TargetPageType=GetRegisteredType(AppViews.SchemeSetTemplate)
            },
            // new NavigationViewItem()
            //{
            //    Content = "新建方案",
            //    TargetPageTag = AppViews.AddSchemeSet,
            //    TargetPageType=GetViewType(AppViews.AddSchemeSet),
            //    Visibility=System.Windows.Visibility.Collapsed,//隐藏，用于面包屑，不在导航不显示Title
            //},
            //new NavigationViewItem()
            //{
            //    Content = "训练",
            //    TargetPageTag = AppViews.Scheme,
            //    TargetPageType=GetViewType(AppViews.Scheme),
            //    Visibility=System.Windows.Visibility.Collapsed,//隐藏，用于面包屑，不在导航不显示Title
            //},
            //new NavigationViewItem()
            //{
            //    Content = "方案详情",
            //    TargetPageTag = AppViews.Scheme,
            //    TargetPageType=GetViewType(AppViews.SchemeSetDetail),
            //    Visibility=System.Windows.Visibility.Collapsed,//隐藏，用于面包屑，不在导航不显示Title
            //},
            new NavigationViewItemSeparator(),
            new NavigationViewItem()
            {
                Content = "患者管理",
                Icon = new SymbolIcon { Symbol = SymbolRegular.BuildingPeople20 },
                TargetPageTag = AppViews.UserManagement,
                TargetPageType=GetRegisteredType(AppViews.UserManagement)
            },
        };
        //#if DEBUG
        //        NavigationItems.Add(
        //               new NavigationViewItem()
        //               {
        //                   Content = "组织管理",
        //                   Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
        //                   TargetPageTag = AppViews.UserManagement,
        //                   TargetPageType = GetRegisteredType(AppViews.Organization)
        //               }
        //           );
        //#endif
        if (_appliationContext.CurrentUser is not null && _appliationContext.CurrentUser.UserType == Shared.AppUserType.Admin)
        {
            NavigationItems.Add(
               new NavigationViewItemSeparator()
            );
            NavigationItems.Add(
                new NavigationViewItem()
                {
                    Content = "组织管理",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Organization20 },
                    TargetPageTag = AppViews.Organization,
                    TargetPageType = GetRegisteredType(AppViews.Organization)
                }
            );
        }
        NavigationItems.Add(
          new NavigationViewItemSeparator()
        );
        NavigationItems.Add(
            new NavigationViewItem()
            {
                Content = "回收站",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Recycle20 },
                TargetPageTag = AppViews.RecycleBin,
                TargetPageType = GetRegisteredType(AppViews.RecycleBin)
            }
        );
        NavigationItems.Add(
           new NavigationViewItemSeparator()
        );
        NavigationFooter = new ObservableCollection<object>
        {
            new NavigationViewItemSeparator(),
            new NavigationViewItem()
            {
                Content = "系统管理",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageTag = AppViews.Setting,
                TargetPageType=GetRegisteredType(AppViews.Setting)
            }
        };

        _isInitialized = true;
    }

    public override void OnNavigatedTo()
    {
        _isInitialized = false;
    }
}

/// <summary>
/// 修改密码
/// </summary>
public partial class MainViewModel : ViewModelBase
{
    #region Property

    [ObservableProperty]
    private object _changePwdDialog;

    [ObservableProperty]
    private string _oldPassword = string.Empty;

    [ObservableProperty]
    private string _newPassword = string.Empty;

    [ObservableProperty]
    private string _checkPassword = string.Empty;

    #endregion Property

    [RelayCommand]
    private async Task ChangePassword()
    {
        var result = await ShowDialogAsync("修改密码", ChangePwdDialog, CheckChangPasswordData);
        if (result == ContentDialogResult.Primary)
        {
            if (_appliationContext.CurrentUser is not null)
            {
                var editorUser = await _accountService.GetByIdAsync(_appliationContext.CurrentUser!.Id);
                editorUser.Password = NewPassword.GetMd5Hash();
                var userDto = await _accountService.AddOrUpdateUserAsync(editorUser);

                if (userDto is not null)
                {
                    _snackbarService.Show("提示", "密码修改成功", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
                }
            }
        }
    }

    private async Task<bool> CheckChangPasswordData()
    {
        var result = true;
        string errorMessage = string.Empty;
        if (string.IsNullOrWhiteSpace(OldPassword))
        {
            errorMessage = "新密码不能为空";
            result = false;
        }
        if (!string.IsNullOrWhiteSpace(OldPassword))
        {
            var user = await _accountService.GetByIdAsync(_appliationContext.CurrentUser!.Id);
            if (user.Password != OldPassword.GetMd5Hash())
            {
                errorMessage = "原始密码不正确";
                result = false;
            }
        }
        if (string.IsNullOrWhiteSpace(NewPassword))
        {
            errorMessage = "新密码不能为空";
            result = false;
        }
        if (string.IsNullOrWhiteSpace(CheckPassword))
        {
            errorMessage = "校验密码不能为空";
            result = false;
        }
        if (NewPassword != CheckPassword)
        {
            errorMessage = "新密码与校验不一致";
            result = false;
        }

        if (!result)
        {
            _snackbarService.Show("错误", errorMessage, ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
        }
        return result;
    }
}