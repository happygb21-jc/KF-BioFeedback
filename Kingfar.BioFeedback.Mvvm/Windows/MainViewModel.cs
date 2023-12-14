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
                Content = "���ݹ���",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home20 },
                TargetPageTag = AppViews.Dashboard,
                TargetPageType=GetRegisteredType(AppViews.Dashboard),
            },
            new NavigationViewItemSeparator(),
            new NavigationViewItem()
            {
                Content = "��������",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Group20 },
                TargetPageTag = AppViews.SchemeSet,
                TargetPageType=GetRegisteredType(AppViews.SchemeSet)
            },
            new NavigationViewItemSeparator(),
            new NavigationViewItem()
            {
                Content = "ģ�����",
                Icon = new SymbolIcon { Symbol = SymbolRegular.BookTemplate20 },
                TargetPageTag = AppViews.SchemeSetTemplate,
                TargetPageType=GetRegisteredType(AppViews.SchemeSetTemplate)
            },
            // new NavigationViewItem()
            //{
            //    Content = "�½�����",
            //    TargetPageTag = AppViews.AddSchemeSet,
            //    TargetPageType=GetViewType(AppViews.AddSchemeSet),
            //    Visibility=System.Windows.Visibility.Collapsed,//���أ��������м�����ڵ�������ʾTitle
            //},
            //new NavigationViewItem()
            //{
            //    Content = "ѵ��",
            //    TargetPageTag = AppViews.Scheme,
            //    TargetPageType=GetViewType(AppViews.Scheme),
            //    Visibility=System.Windows.Visibility.Collapsed,//���أ��������м�����ڵ�������ʾTitle
            //},
            //new NavigationViewItem()
            //{
            //    Content = "��������",
            //    TargetPageTag = AppViews.Scheme,
            //    TargetPageType=GetViewType(AppViews.SchemeSetDetail),
            //    Visibility=System.Windows.Visibility.Collapsed,//���أ��������м�����ڵ�������ʾTitle
            //},
            new NavigationViewItemSeparator(),
            new NavigationViewItem()
            {
                Content = "���߹���",
                Icon = new SymbolIcon { Symbol = SymbolRegular.BuildingPeople20 },
                TargetPageTag = AppViews.UserManagement,
                TargetPageType=GetRegisteredType(AppViews.UserManagement)
            },
        };
        //#if DEBUG
        //        NavigationItems.Add(
        //               new NavigationViewItem()
        //               {
        //                   Content = "��֯����",
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
                    Content = "��֯����",
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
                Content = "����վ",
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
                Content = "ϵͳ����",
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
/// �޸�����
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
        var result = await ShowDialogAsync("�޸�����", ChangePwdDialog, CheckChangPasswordData);
        if (result == ContentDialogResult.Primary)
        {
            if (_appliationContext.CurrentUser is not null)
            {
                var editorUser = await _accountService.GetByIdAsync(_appliationContext.CurrentUser!.Id);
                editorUser.Password = NewPassword.GetMd5Hash();
                var userDto = await _accountService.AddOrUpdateUserAsync(editorUser);

                if (userDto is not null)
                {
                    _snackbarService.Show("��ʾ", "�����޸ĳɹ�", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
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
            errorMessage = "�����벻��Ϊ��";
            result = false;
        }
        if (!string.IsNullOrWhiteSpace(OldPassword))
        {
            var user = await _accountService.GetByIdAsync(_appliationContext.CurrentUser!.Id);
            if (user.Password != OldPassword.GetMd5Hash())
            {
                errorMessage = "ԭʼ���벻��ȷ";
                result = false;
            }
        }
        if (string.IsNullOrWhiteSpace(NewPassword))
        {
            errorMessage = "�����벻��Ϊ��";
            result = false;
        }
        if (string.IsNullOrWhiteSpace(CheckPassword))
        {
            errorMessage = "У�����벻��Ϊ��";
            result = false;
        }
        if (NewPassword != CheckPassword)
        {
            errorMessage = "��������У�鲻һ��";
            result = false;
        }

        if (!result)
        {
            _snackbarService.Show("����", errorMessage, ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
        }
        return result;
    }
}