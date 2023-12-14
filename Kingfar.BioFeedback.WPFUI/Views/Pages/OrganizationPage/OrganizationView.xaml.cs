using System.Windows.Controls;

namespace Kingfar.BioFeedback.WPFUI.Views.Pages
{
    /// <summary>
    /// OrganizationView.xaml 的交互逻辑
    /// </summary>
    public partial class OrganizationView : Page
    {
        public OrganizationView()
        {
            InitializeComponent();

            #region Dialog

            if (DataContext != null && DataContext is OrganizationViewModel vm)
            {
                var adddDialog = FindResource("AddDialog");
                if (adddDialog is StackPanel addPanel)
                {
                    addPanel.DataContext = DataContext;
                    vm.AddDialog = addPanel;
                }
                var changePwdDialog = FindResource("ChangePasswordDialog");
                if (changePwdDialog is StackPanel changePwdPanel)
                {
                    changePwdPanel.DataContext = DataContext;
                    vm.ChangePwdDialog = changePwdPanel;
                }
            }

            #endregion Dialog

            #region GridEvent

            this.Loaded += (s, e) =>
            {
                double rowHeight = Application.Current.MainWindow.ActualHeight - ParentGrid.RowDefinitions[0].ActualHeight - ParentGrid.RowDefinitions[2].ActualHeight - 128;
                ParentGrid.RowDefinitions[1].Height = new GridLength(rowHeight);
            };
            ParentGrid.SizeChanged += (s, e) =>
            {
                double rowHeight = Application.Current.MainWindow.ActualHeight - ParentGrid.RowDefinitions[0].ActualHeight - ParentGrid.RowDefinitions[2].ActualHeight - 128;
                ParentGrid.RowDefinitions[1].Height = new GridLength(rowHeight);
            };

            #endregion GridEvent
        }
    }
}