using System.Windows.Controls;

namespace Kingfar.BioFeedback.WPFUI.Views.Pages
{
    /// <summary>
    /// RecycleBinView.xaml 的交互逻辑
    /// </summary>
    public partial class RecycleBinView : Page
    {
        public RecycleBinView()
        {
            InitializeComponent();

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
    }
}