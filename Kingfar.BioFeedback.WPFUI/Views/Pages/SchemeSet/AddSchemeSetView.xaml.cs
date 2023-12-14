using Kingfar.BioFeedback.Mvvm.Pages;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kingfar.BioFeedback.WPFUI.Views.Pages
{
    /// <summary>
    /// AddSchemeView.xaml 的交互逻辑
    /// </summary>
    public partial class AddSchemeSetView : Page
    {
        private AddSchemeSetViewModel _viewModel;

        public AddSchemeSetView()
        {
            InitializeComponent();

            #region Dialog

            if (this.DataContext != null && this.DataContext is AddSchemeSetViewModel vm)
            {
                _viewModel = vm;
                //var dialog = FindResource("AddOrganization");
                //if (dialog is StackPanel panel)
                //{
                //    panel.DataContext = this.DataContext;
                //    vm.DialogContent = panel;
                //}
            }

            #endregion Dialog

            #region GridEvent

            this.Loaded += (s, e) =>
               {
                   double rowHeight = Application.Current.MainWindow.ActualHeight - 120;
                   RootGrid.Height = rowHeight;
                   ApplicationTypeList.Width = ParentGrid.ActualWidth - 80;
               };
            RootGrid.SizeChanged += (s, e) =>
            {
                double rowHeight = Application.Current.MainWindow.ActualHeight - 120;
                RootGrid.Height = rowHeight;
                ApplicationTypeList.Width = ParentGrid.ActualWidth - 80;
            };

            #endregion GridEvent
        }

        private void TrainListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TrainListView.SelectedItem is not null && TrainListView.SelectedItem is Services.SchemeDto trainDto)
            {
                _viewModel.SelectedItems.Add(trainDto);
            }
        }
    }
}