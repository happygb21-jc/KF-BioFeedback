using System.Collections.Concurrent;

namespace Kingfar.BioFeedback.Mvvm
{
    public class AppViews
    {
        public const string NullView = "NullView";
        public const string Login = "LoginView";
        public const string Main = "MainView";
        public const string UserManagement = "UserManagementView";
        public const string Dashboard = "DashboardView";
        public const string Scheme = "SchemeView";
        public const string SchemeSet = "SchemeSetView";
        public const string SchemeSetTemplate = "SchemeSetTemplateView";
        public const string AddSchemeDialog = "AddSchemeDialogView";
        public const string AddSchemeSet = "AddSchemeSetView";
        public const string Organization = "OrganizationView";
        public const string AddOrganizationDialog = "AddOrganizationDialogView";
        public const string Setting = "SettingView";
        public const string RecycleBin = "RecycleBinView";
        public const string SchemeSetDetail = "SchemeSetDetailView";

        public static ConcurrentDictionary<string, Type> ViewTypeDic = new();
    }
}