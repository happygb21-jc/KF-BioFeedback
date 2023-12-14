using Kingfar.BioFeedback.Patient.Client.Common;
using Wpf.Ui;
using Appearance = Wpf.Ui.Appearance;

namespace Kingfar.BioFeedback.Patient.Client
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : FluentWindow, INavigationWindow

    {
        private bool _isUserClosedPane;

        private bool _isPaneOpenedOrClosedFromCode;
        private readonly AppStartService _appStartService;
        public bool IsOpenPane { get; set; } = true;

        public MainView(
            IPageService pageService,
            INavigationService navigationService,
            ISnackbarService snackbarService,
            AppStartService appStartService,
            IContentDialogService contentDialogService)
        {
            Appearance.SystemThemeWatcher.Watch(this);
            _appStartService = appStartService;
            InitializeComponent();

            SetPageService(pageService);
            snackbarService.SetSnackbarPresenter(RootSnackbarPresenter);
            navigationService.SetNavigationControl(RootNavigation);
            contentDialogService.SetContentPresenter(RootContentDialog);
        }

        public void CloseWindow() => Close();

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow() => Show();

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
            RootNotifyIcon.Dispose();
            Environment.Exit(0);
        }

        private void RootNavigation_Loaded(object sender, RoutedEventArgs e)
        {
            //RootNavigation.Navigate(AppViews.Main);
        }

        private void Logout_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Hide();
            Application.Current.MainWindow = ContainerLocator.Container.Resolve<NullView>();
            _appStartService.CreateShell();
            Application.Current.MainWindow.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            RootNotifyIcon.Dispose();
            Environment.Exit(0);
        }

        private void TitleBar_CloseClicked(TitleBar sender, RoutedEventArgs args)
        {
            RootNotifyIcon.Dispose();
            Environment.Exit(0);
        }

        #endregion ViewEvent
    }
}