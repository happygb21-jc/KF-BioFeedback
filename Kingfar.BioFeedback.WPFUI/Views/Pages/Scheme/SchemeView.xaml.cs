using ErgoLAB.Expert.Client;
using Kingfar.BioFeedback.Mvvm.Pages;
using Kingfar.BioFeedback.WPFUI.Views.Pages.Scheme;
using System.Windows.Controls;

namespace Kingfar.BioFeedback.WPFUI.Views.Pages
{
    /// <summary>
    /// SchemeSetView.xaml 的交互逻辑
    /// </summary>
    public partial class SchemeView : Page
    {
        //放到APP里全局用，不用每次关闭，程序退出是整体关闭
        private ExpertClient? _expertClient;

        private Guid _clientKey = Guid.NewGuid();

        public SchemeView()
        {
            InitializeComponent();

            #region Dialog

            if (this.DataContext != null && this.DataContext is SchemeViewModel vm)
            {
                var adddDialog = FindResource("AddSchemeDialog");
                if (adddDialog is StackPanel addSchemePanel)
                {
                    addSchemePanel.DataContext = this.DataContext;
                    vm.AddSchemeDialog = adddDialog;
                }
            }

            #endregion Dialog

            #region GridEvent

            this.Loaded += (s, e) =>
            {
                double rowHeight = Application.Current.MainWindow.ActualHeight - 120;
                RootGrid.Height = rowHeight;
            };
            RootGrid.SizeChanged += (s, e) =>
            {
                double rowHeight = Application.Current.MainWindow.ActualHeight - 120;
                RootGrid.Height = rowHeight;
            };

            #endregion GridEvent
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _expertClient = new ExpertClient(new ExpertClientOptions
            {
                //不要设置为当前ErgoLAB Expert路径下的exe，外壳没有生命周期，需要执行版本内部的
                StartFilePath = @"C:\Users\Administrator\AppData\Local\ErgoLAB.Expert\ErgoLAB Expert.exe",
                Args = new CommandLineArgs
                {
                    Mode = RunMode.Admin,
                    AppTitle = "医疗",
                    Rpc = "rpc://shared-memory/test_channel",
                    SingleInstanceToken = _clientKey.ToString(),
                    ScreenIndex = 0
                },
                HoldLifetimeLock = true
            });
            _expertClient.StateChanged += OnExpertClientStateChanged;

            TestRpcInteractionMaster master = new TestRpcInteractionMaster(_expertClient.Slave);
            master.EmbedSchemeRunPath = "";
            _ = _expertClient.StartAsync(master);
            Application.Current.MainWindow.Hide();
        }

        private void OnExpertClientStateChanged(in ExpertClientStateChangedArgs args)
        {
            if (args.NewState == ExpertClientState.Disposed)
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    Application.Current.MainWindow.Show();
                    _expertClient.DisposeAsync();
                });
            }
        }
    }
}