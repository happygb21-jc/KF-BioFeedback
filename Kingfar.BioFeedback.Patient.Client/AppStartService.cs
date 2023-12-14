using Kingfar.BioFeedback.Patient.Client.Common;
using Prism.Mvvm;
using Prism.Regions;
using INavigationAware = Wpf.Ui.Controls.INavigationAware;

namespace Kingfar.BioFeedback.Patient.Client
{
    public class AppStartService
    {
        public void CreateShell()
        {
            var container = ContainerLocator.Container;
            var validationResult = Validation(container);
            static ButtonResult Validation(IContainerProvider container)
            {
                var dialogService = container.Resolve<IHostDialogService>();
                return dialogService.ShowWindow(AppViews.Login).Result;
            }

            try
            {
                var shell = container.Resolve<MainWindow>();
                if (shell is Window view)
                {
                    ViewModelLocator.SetAutoWireViewModel(view, true);
                    var regionManager = container.Resolve<IRegionManager>();
                    RegionManager.SetRegionManager(view, regionManager);
                    RegionManager.UpdateRegions();

                    if (view.DataContext is INavigationAware)
                    {
                        Application.Current.MainWindow = view;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Exit()
        {
            Environment.Exit(0);
        }

        public void Logout()
        {
            Application.Current.MainWindow.Hide();
            CreateShell();
            Application.Current.MainWindow.Show();

            if (Application.Current.MainWindow.DataContext is INavigationAware navigationAware)
                navigationAware.OnNavigatedTo();
        }
    }
}