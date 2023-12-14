using Prism.Services.Dialogs;

namespace Kingfar.BioFeedback.WPFCommon.Services
{
    public interface IHostDialogService : IDialogService
    {
        IDialogResult ShowWindow(string name);
    }
}