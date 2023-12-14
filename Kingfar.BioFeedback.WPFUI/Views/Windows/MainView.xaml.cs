using Kingfar.BioFeedback.WebHost;
using NewLife;
using Prism.Services.Dialogs;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows.Controls;
using System.Windows.Interop;
using Appearance = Wpf.Ui.Appearance;

namespace Kingfar.BioFeedback.WPFUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : FluentWindow, INavigationWindow
    {
        private bool _isUserClosedPane;

        private bool _isPaneOpenedOrClosedFromCode;
        private readonly AppStartService _appStartService;
        public bool IsOpenPane { get; set; } = true;
        private readonly IWebApiServer _webApiServer;

        public MainView(IPageService pageService,
            INavigationService navigationService,
            ISnackbarService snackbarService,
            AppStartService appStartService,
            IContentDialogService contentDialogService,
            IWebApiServer webApiServer)
        {
            Appearance.SystemThemeWatcher.Watch(this);
            _appStartService = appStartService;
            _webApiServer = webApiServer;
            InitializeComponent();

            SetPageService(pageService);
            snackbarService.SetSnackbarPresenter(RootSnackbarPresenter);
            navigationService.SetNavigationControl(RootNavigation);
            contentDialogService.SetContentPresenter(RootContentDialog);
            var ipAddress = GetLocalIPAddress();
            var test = GetLocalIPAddresses();
            string baseAddress = $"http://{ipAddress}:9527/";

            _webApiServer.Start(baseAddress, ContainerLocator.Container);

            #region ChangePassword

            if (this.DataContext != null && this.DataContext is MainViewModel vm)
            {
                var changePwdDialog = FindResource("ChangePasswordDialog");
                if (changePwdDialog is StackPanel changePwdPanel)
                {
                    changePwdPanel.DataContext = this.DataContext;
                    vm.ChangePwdDialog = changePwdPanel;
                }
            }

            #endregion ChangePassword
        }

        public IPAddress? GetLocalIPAddresses()
        {
            var addresses = NetHelper.MyIP();
            return addresses;
        }

        private string GetLocalIPAddress()
        {
            var networkInterfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

            foreach (var networkInterface in networkInterfaces)
            {
                if (networkInterface.OperationalStatus != System.Net.NetworkInformation.OperationalStatus.Up || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;

                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    IPInterfaceProperties ip = networkInterface.GetIPProperties();
                }
                var ipProperties = networkInterface.GetIPProperties();
                var unicastAddresses = ipProperties.UnicastAddresses;

                foreach (var unicastAddress in unicastAddresses)
                {
                    if (unicastAddress.Address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork || System.Net.IPAddress.IsLoopback(unicastAddress.Address))
                        continue;

                    return unicastAddress.Address.ToString();
                }
            }

            return string.Empty;
        }

        #region INavigationWindow

        public void CloseWindow() => Close();

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow() => Show();

        #endregion INavigationWindow

        #region ViewEvent

        private void MainView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_isUserClosedPane)
            {
                return;
            }

            _isPaneOpenedOrClosedFromCode = true;
            RootNavigation.IsPaneOpen = !(e.NewSize.Width <= 1200);
            _isPaneOpenedOrClosedFromCode = false;
        }

        private void NavigationView_OnPaneClosed(NavigationView sender, RoutedEventArgs args)
        {
            if (_isPaneOpenedOrClosedFromCode)
            {
                return;
            }
            NavPaneHeader.Visibility = Visibility.Collapsed;
            _isUserClosedPane = true;
        }

        private void NavigationView_OnPaneOpened(NavigationView sender, RoutedEventArgs args)
        {
            if (_isPaneOpenedOrClosedFromCode)
            {
                return;
            }
            NavPaneHeader.Visibility = Visibility.Visible;
            _isUserClosedPane = false;
        }

        private void OnNavigationSelectionChanged(NavigationView sender, RoutedEventArgs args)
        {
            if (sender is not Wpf.Ui.Controls.NavigationView navigationView)
            {
                return;
            }
            //是否需要关闭标题
            //RootNavigation.HeaderVisibility =
            //    navigationView.SelectedItem?.TargetPageType != typeof(DashboardPage) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _webApiServer.Stop();
            RootNotifyIcon.Dispose();
            Environment.Exit(0);
        }

        private void RootNavigation_Loaded(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate(AppViews.Dashboard);
        }

        private void Logout_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _webApiServer.Stop();
            Application.Current.MainWindow.Hide();
            Application.Current.MainWindow = ContainerLocator.Container.Resolve<NullView>();
            _appStartService.CreateShell();
            Application.Current.MainWindow.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            _webApiServer.Stop();
            RootNotifyIcon.Dispose();
            Environment.Exit(0);
        }

        private void TitleBar_CloseClicked(TitleBar sender, RoutedEventArgs args)
        {
            _webApiServer.Stop();
            RootNotifyIcon.Dispose();
            Environment.Exit(0);
        }

        #endregion ViewEvent
    }
}