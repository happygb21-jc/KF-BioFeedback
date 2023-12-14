using System.Windows.Controls;

namespace Kingfar.BioFeedback.WPFUI.Views.Pages
{
    /// <summary>
    /// SchemeView.xaml 的交互逻辑
    /// </summary>
    public partial class SchemeSetView : Page
    {
        public SchemeSetView()
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