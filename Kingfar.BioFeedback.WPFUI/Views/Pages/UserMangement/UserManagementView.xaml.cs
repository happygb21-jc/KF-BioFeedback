using Kingfar.BioFeedback.Mvvm.User;
using System.Windows.Controls;

namespace Kingfar.BioFeedback.WPFUI.Views.Pages
{
    /// <summary>
    /// UserManagementView.xaml 的交互逻辑
    /// </summary>
    public partial class UserManagementView : Page
    {
        public UserManagementView()
        {
            InitializeComponent();

            #region AddUserDialog

            if (this.DataContext != null && this.DataContext is UserManagementViewModel vm)
            {
                var adddDialog = FindResource("AddDialog");
                if (adddDialog is StackPanel panel)
                {
                    panel.DataContext = this.DataContext;
                    vm.AddDialog = adddDialog;
                }
            }

            #endregion AddUserDialog

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