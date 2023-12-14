using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;
using Prism.DryIoc;
using System.IO;
using System.Runtime.InteropServices;
using Kingfar.BioFeedback.DataAccess;
using NLog;
using Kingfar.BioFeedback.Shared.Abstractions.Common;

namespace Kingfar.BioFeedback.WPFUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        #region user32调用

        /// <summary>
        /// 查找窗体
        /// </summary>
        /// <param name="cls"></param>
        /// <param name="win"></param>
        /// <returns></returns>
        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string cls, string win);

        /// <summary>
        /// 激活窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 判断最小化
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32")]
        private static extern bool OpenIcon(IntPtr hWnd);

        private System.Threading.Mutex mutex;

        #endregion user32调用

        public IConfiguration Configuration { get; set; }

        public App()
        {
            Configuration = new ConfigurationBuilder().SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Config"))
                .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true)
                .Build();
        }

        protected override void OnInitialized()
        {
            MainWindow.Hide();
            var appStart = ContainerLocator.Container.Resolve<AppStartService>();
            appStart.CreateShell();

            base.OnInitialized();
        }

        protected override Window? CreateShell()
        {
            #region Log

            //UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
            //多线程异常
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            #endregion Log

            return ContainerLocator.Container.Resolve<NullView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            #region 全局注册

            containerRegistry.RegisterInstance(Configuration);

            #endregion 全局注册

            #region Register Services

            containerRegistry.AddWPFUIServices();
            containerRegistry.AddServices();
            containerRegistry.AddShared();
            containerRegistry.AddSqlSugar(Configuration);

            #endregion Register Services

            #region Register ViewModel

            containerRegistry.AddViews();

            #endregion Register ViewModel
        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //通常全局异常捕捉的都是致命信息
            AppLogs.Debug($"{e.Exception.StackTrace},{e.Exception.Message}");
        }

        private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            AppLogs.Error(e.Exception);
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception? ex = e.ExceptionObject as Exception;
            if (ex != null)
                AppLogs.Error(ex);
        }

        #region 系统单例，只允许打开一个

        protected override void OnStartup(StartupEventArgs e)
        {
            //判断程序互斥
            mutex = new System.Threading.Mutex(true, "BioFeedbackSystem", out bool ret);

            if (!ret)
            {
                ActivateOtherWindow();
                Environment.Exit(0);
            }
            base.OnStartup(e);
        }

        private void ActivateOtherWindow()
        {
            //里面的文本改成自己程序窗口的标题
            var other = FindWindow(string.Empty, "BioFeedbackSystem");
            if (other != IntPtr.Zero)
            {
                SetForegroundWindow(other);
                if (IsIconic(other))
                    OpenIcon(other);
            }
        }

        #endregion 系统单例，只允许打开一个
    }
}