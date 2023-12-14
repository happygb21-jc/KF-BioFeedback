namespace Kingfar.BioFeedback.Mvvm
{
    public partial class DialogViewModel : ObservableObject, IDialogAware
    {
        public string Title { get; set; } = string.Empty;

        [ObservableProperty]
        private string _applicationTitle;

        public event Action<IDialogResult> RequestClose;

        [RelayCommand]
        public virtual void Cancel() => OnDialogClosed(ButtonResult.Cancel);

        [RelayCommand]
        public virtual void Save() => OnDialogClosed(ButtonResult.OK);

        public bool CanCloseDialog() => true;

        public virtual void OnDialogClosed() => OnDialogClosed(ButtonResult.OK);

        public virtual void OnDialogOpened(IDialogParameters parameters)
        { }

        public virtual void OnDialogClosed(ButtonResult result)
        {
            RequestClose?.Invoke(new DialogResult(result));
        }

        public virtual void OnDialogClosed(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }
    }
}