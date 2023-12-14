using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Kingfar.BioFeedback.WPFCommon.Services
{
    public class LoginSnackbarService : SnackbarService, ILoginSnackbarService
    {
        private SnackbarPresenter? _snackbarPresenter;

        public new void SetSnackbarPresenter(SnackbarPresenter contentPresenter)
        {
            if (_snackbarPresenter is null)
            {
                _snackbarPresenter = contentPresenter;
                base.SetSnackbarPresenter(contentPresenter);
            }
        }
    }
}