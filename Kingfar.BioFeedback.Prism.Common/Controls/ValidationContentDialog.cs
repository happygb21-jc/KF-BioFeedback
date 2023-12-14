using Wpf.Ui.Controls;

namespace Kingfar.BioFeedback.Patient.Client.Controls
{
    public class ValidationContentDialog : ContentDialog
    {
        private Func<Task<bool>>? _checkTaskFunc;
        private Func<bool>? _checkFunc;

        public ValidationContentDialog(ContentPresenter contentPresenter, Func<Task<bool>>? checkTaskFunc) : base(contentPresenter)
        {
            _checkTaskFunc = checkTaskFunc;
        }

        public ValidationContentDialog(ContentPresenter contentPresenter, Func<bool>? checkFunc) : base(contentPresenter)
        {
            _checkFunc = checkFunc;
        }

        protected override async void OnButtonClick(ContentDialogButton button)
        {
            if (button == ContentDialogButton.Primary && _checkTaskFunc is not null && !(await _checkTaskFunc()))
            {
                return;
            }
            else if (button == ContentDialogButton.Primary && _checkFunc is not null && !_checkFunc())
            {
                return;
            }
            else
            {
                base.OnButtonClick(button);
            }
        }
    }
}