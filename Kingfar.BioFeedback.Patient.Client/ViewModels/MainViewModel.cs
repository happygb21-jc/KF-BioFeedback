using Kingfar.BioFeedback.Patient.Client.Common;
using Kingfar.BioFeedback.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Patient.Client.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        #region Service

        private bool _isInitialized = false;
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

        public MainViewModel(IApplicationContext appliationContext, ISnackbarService snackbarService, IContentDialogService contentDialogService)
        {
            _appliationContext = appliationContext;
            if (!_isInitialized) InitializeViewModel();
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
                    Content = "方案列表",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.BuildingPeople20 },
                    TargetPageTag = AppViews.SchemeSet,
                    TargetPageType=GetRegisteredType(AppViews.SchemeSet)
                },
                new NavigationViewItemSeparator(),
            };

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
}