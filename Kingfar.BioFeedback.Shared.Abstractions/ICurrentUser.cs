namespace Kingfar.BioFeedback.Shared.Abstractions
{
    public interface ICurrentUser
    {
        Guid Id { get; }
        string Name { get; }
        AppUserType UserType { get; }
    }
}