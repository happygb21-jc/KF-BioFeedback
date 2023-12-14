namespace Kingfar.BioFeedback.Patient.Client
{
    public partial class LoginViewModel : DialogViewModel
    {
        private readonly ISnackbarService _snackbarService;
        public DelegateCommand<string> ExecuteCommand { get; }

        private string _userName = string.Empty;

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                SetLoginButtonEnabled();
                OnPropertyChanged();
            }
        }

        private string _password = string.Empty;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                SetLoginButtonEnabled();
                OnPropertyChanged();
            }
        }

        private string _passwordChar;

        public string PasswordChar
        {
            get => _passwordChar;
            set
            {
                _passwordChar = value;
            }
        }

        private bool isLoginEnabled;

        public bool IsLoginEnabled
        {
            get { return isLoginEnabled; }
            set { isLoginEnabled = value; OnPropertyChanged(); }
        }

        [ObservableProperty]
        private string _description;

        public LoginViewModel(
            ISnackbarService snackbarService,
            IApplicationContext applicationContext)
        {
            _snackbarService = snackbarService;
            ExecuteCommand = new DelegateCommand<string>(Execute);
            //var systemInfo = configuration.GetSection(AppConsts.AppKey).Get<SystemInfo>();
            ApplicationTitle = applicationContext.SystemInfo.SoftwareName;
            Description = applicationContext.SystemInfo.EnglishName;
#if DEBUG
            UserName = "admin";
#endif
        }

        private void Execute(string arg)
        {
            switch (arg)
            {
                case "LoginUser": LoginUser(); break;
            }
        }

        private void LoginUser()
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                //if (_accountService.Login(UserName, Password))
                //{
                //    _snackbarService.Show("提示", "登录成功", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
                //    Save();
                //}
                //else
                //{
                //    _snackbarService.Show("错误", "用户名或密码正确", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.ErrorCircle20), TimeSpan.FromSeconds(2));
                //}
                Save();
            }
            else
            {
                _snackbarService.Show("错误", "用户名或密码不能为空", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.ErrorCircle20), TimeSpan.FromSeconds(2));
            }
        }

        public void SetLoginButtonEnabled()
        {
            IsLoginEnabled = !string.IsNullOrWhiteSpace(UserName);
        }
    }
}