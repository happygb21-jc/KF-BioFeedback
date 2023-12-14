﻿using Kingfar.BioFeedback.Services;
using Microsoft.Extensions.Configuration;
using Prism.Commands;
using System;

namespace Kingfar.BioFeedback.Mvvm
{
    public partial class LoginViewModel : DialogViewModel
    {
        private readonly ILoginSnackbarService _snackbarService;
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

        private IAccountService _accountService;

        public LoginViewModel(IConfiguration configuration,
            ILoginSnackbarService snackbarService,
            IApplicationContext applicationContext,
            IAccountService accountService)
        {
            _snackbarService = snackbarService;
            _accountService = accountService;
            ExecuteCommand = new DelegateCommand<string>(Execute);
            //var systemInfo = configuration.GetSection(AppConsts.AppKey).Get<SystemInfo>();
            ApplicationTitle = applicationContext.SystemInfo.SoftwareName;
            Description = applicationContext.SystemInfo.EnglishName;
#if DEBUG
            UserName = "admin"; Password = "123123";
#endif
        }

        private void Execute(string arg)
        {
            switch (arg)
            {
                case "LoginUser": LoginUser(); break;
                case "ForgotPassword": ForgotPassword(); break;
            }
        }

        private void LoginUser()
        {
            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
            {
                if (_accountService.Login(UserName, Password))
                {
                    _snackbarService.Show("提示", "登录成功", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Info20), TimeSpan.FromSeconds(2));
                    Save();
                }
                else
                {
                    _snackbarService.Show("错误", "用户名或密码正确", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.ErrorCircle20), TimeSpan.FromSeconds(2));
                }
            }
            else
            {
                _snackbarService.Show("错误", "用户名或密码不能为空", ControlAppearance.Danger, new SymbolIcon(SymbolRegular.ErrorCircle20), TimeSpan.FromSeconds(2));
            }
        }

        private void ForgotPassword()
        {
            throw new NotImplementedException();
        }

        public void SetLoginButtonEnabled()
        {
            IsLoginEnabled = !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
        }
    }
}