using Prism.Services.Dialogs;

namespace Kingfar.BioFeedback.Prism.Common
{
    public interface IHostDialogService : IDialogService
    {
        IDialogResult ShowWindow(string name);
    }
}