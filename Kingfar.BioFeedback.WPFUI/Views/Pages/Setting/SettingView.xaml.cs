using System.Windows.Controls;

namespace Kingfar.BioFeedback.WPFUI.Views.Pages
{
    /// <summary>
    /// SettingView.xaml 的交互逻辑
    /// </summary>
    public partial class SettingView : Page
    {
        private readonly IApplicationContext _applicationContext;

        public SettingView(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            InitializeComponent();

            SysName.Text = _applicationContext.SystemInfo.SoftwareName;
            SysEnglishName.Text = _applicationContext.SystemInfo.EnglishName;
            Company.Text = _applicationContext.SystemInfo.Company;
            Telephone.Text = $"联系方式：{_applicationContext.SystemInfo.Telephone}";
            Version.Text = $"版本：{_applicationContext.SystemInfo.Version}";
        }
    }
}