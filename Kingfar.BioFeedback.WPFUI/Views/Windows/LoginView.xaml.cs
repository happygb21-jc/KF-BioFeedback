using Prism.Services.Dialogs;

namespace Kingfar.BioFeedback.WPFUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginView : FluentWindow, IDialogWindow
    {
        private readonly ILoginSnackbarService _snackbarService;

        public LoginView(ILoginSnackbarService snackbarService)
        {
            // 设置对话框窗口的样式和属性
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            ShowInTaskbar = false;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();
            _snackbarService = snackbarService;
            _snackbarService.SetSnackbarPresenter(LoginSnackbarPresenter);
        }

        public IDialogResult Result { get; set; } = default!;

        private void TitleBar_CloseClicked(TitleBar sender, RoutedEventArgs args)
        {
            Environment.Exit(0);
        }
    }
}