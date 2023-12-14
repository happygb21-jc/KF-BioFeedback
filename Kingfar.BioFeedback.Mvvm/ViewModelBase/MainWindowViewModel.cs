using Kingfar.BioFeedback.Mvvm.Common;
using Microsoft.Extensions.Configuration;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using MenuItem=Wpf.Ui.Controls.MenuItem;

namespace Kingfar.BioFeedback.WPFUI.ViewModels;

public partial class MainWindowViewModel : BindableBase
{
    private bool _isInitialized = false;

    private string _applicationTitle = String.Empty;
    public string ApplicationTitle { get => _applicationTitle; set => _applicationTitle = value; }

    private ObservableCollection<object> _navigationItems = new();
    public ObservableCollection<object> NavigationItems { get => _navigationItems; set => _navigationItems = value; }

    private ObservableCollection<object> _navigationFooter = new();
    public ObservableCollection<object> NavigationFooter { get => _navigationFooter; set => _navigationFooter = value; }

    private ObservableCollection<MenuItem> _trayMenuItems = new();
    public ObservableCollection<MenuItem> TrayMenuItems { get => _trayMenuItems; set => _trayMenuItems = value; }


    private readonly IConfiguration _configuration;
    public MainWindowViewModel(INavigationService navigationService, IConfiguration configuration)
    {
        _configuration= configuration;
        if (!_isInitialized)
            InitializeViewModel();
    }

    private void InitializeViewModel()
    {
        var systemInfo = _configuration.GetSection(AppConsts.AppKey).Get<SystemInfo>();
        ApplicationTitle = systemInfo!.Title;

        NavigationItems = new ObservableCollection<object>
        {
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
            new NavigationViewItem()
            {
                Content = "Data",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(Views.Pages.DataPage)
            }
        };

        NavigationFooter = new ObservableCollection<object>
        {
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };

        TrayMenuItems = new ObservableCollection<MenuItem>
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };

        _isInitialized = true;
    }


}
