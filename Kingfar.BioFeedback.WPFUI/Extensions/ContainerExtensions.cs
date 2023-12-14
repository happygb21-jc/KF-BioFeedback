using Kingfar.BioFeedback.Mvvm.User;
using Kingfar.BioFeedback.Mvvm.Pages;
using Kingfar.BioFeedback.WebHost;

namespace Kingfar.BioFeedback.WPFUI
{
    public static class ContainerExtensions
    {
        /// <summary>
        /// 用于 View与ViewModel的绑定注册
        /// </summary>
        /// <param name="services"></param>
        public static void AddViews(this IContainerRegistry services)
        {
            services.RegisterForNavigationEx<NullView>(AppViews.NullView);
            services.RegisterForNavigationEx<LoginView, LoginViewModel>(AppViews.Login);

            services.RegisterForNavigationEx<MainView, MainViewModel>(AppViews.Main);
            services.RegisterForNavigationEx<UserManagementView, UserManagementViewModel>(AppViews.UserManagement);
            services.RegisterForNavigationEx<DashboardView, DashboardViewModel>(AppViews.Dashboard);
            services.RegisterForNavigationEx<SchemeView, SchemeViewModel>(AppViews.Scheme);
            services.RegisterForNavigationEx<SchemeSetView, SchemeSetViewModel>(AppViews.SchemeSet);
            //services.RegisterForNavigation<AddSchemeDialogView, AddSchemeDialogViewModel>(AppViews.AddSchemeDialog);//
            services.RegisterForNavigationEx<AddSchemeSetView, AddSchemeSetViewModel>(AppViews.AddSchemeSet);
            //services.RegisterForNavigation<AddOrganizationDialogView, AddOrganizationDialogViewModel>(AppViews.AddOrganizationDialog);//
            services.RegisterForNavigationEx<OrganizationView, OrganizationViewModel>(AppViews.Organization);
            services.RegisterForNavigationEx<SettingView>(AppViews.Setting);
            services.RegisterForNavigationEx<RecycleBinView, RecycleBinViewModel>(AppViews.RecycleBin);
            services.RegisterForNavigationEx<SchemeSetTemplateView, SchemeSetTemplateViewModel>(AppViews.SchemeSetTemplate);
            services.RegisterForNavigationEx<SchemeSetDetailView>(AppViews.SchemeSetDetail);
        }

        internal static void RegisterForNavigationEx<TView, TViewModel>(this IContainerRegistry services, string viewName)
        {
            AppViews.ViewTypeDic.TryAdd(viewName, typeof(TView));
            services.RegisterForNavigation<TView, TViewModel>(viewName);
        }

        internal static void RegisterForNavigationEx<TView>(this IContainerRegistry services, string viewName)
        {
            AppViews.ViewTypeDic.TryAdd(viewName, typeof(TView));
            services.RegisterForNavigation<TView>(viewName);
        }

        public static void AddWPFUIServices(this IContainerRegistry services)
        {
            services.RegisterSingleton<IHostDialogService, DialogHostService>();
            services.RegisterSingleton<AppStartService>();

            services.RegisterSingleton<ILoginSnackbarService, LoginSnackbarService>();

            services.RegisterSingleton<ISnackbarService, SnackbarService>();
            services.RegisterSingleton<IPageService, PageService>();
            services.RegisterScoped<INavigationWindow, MainView>();
            services.RegisterSingleton<INavigationService, NavigationService>();
            // Theme manipulation
            services.RegisterSingleton<IThemeService, ThemeService>();
            // Taskbar manipulation
            services.RegisterSingleton<ITaskBarService, TaskBarService>();
            services.RegisterSingleton<Wpf.Ui.Tray.INotifyIconService, Wpf.Ui.Tray.NotifyIconService>();
            services.RegisterSingleton<IContentDialogService, ContentDialogService>();

            services.RegisterSingleton<IWebApiServer, WebApiServer>();
        }
    }
}