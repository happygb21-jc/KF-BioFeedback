using Kingfar.BioFeedback.Mvvm.Pages;
using System.Windows.Controls;

namespace Kingfar.BioFeedback.WPFUI.Views.Pages
{
    /// <summary>
    /// SchemeView.xaml 的交互逻辑
    /// </summary>
    public partial class SchemeSetTemplateView : Page
    {
        public SchemeSetTemplateView()
        {
            InitializeComponent();

            #region Dialog

            if (this.DataContext != null && this.DataContext is SchemeSetTemplateViewModel vm)
            {
                var adddDialog = FindResource("AddSchemeSetDialog");
                if (adddDialog is StackPanel addSchemePanel)
                {
                    addSchemePanel.DataContext = this.DataContext;
                    vm.AddSchemeSetDialog = adddDialog;
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
    }
}