namespace Kingfar.BioFeedback.Patient.Client
{
    public interface IHostDialogService : IDialogService
    {
        IDialogResult ShowWindow(string name);
    }
}