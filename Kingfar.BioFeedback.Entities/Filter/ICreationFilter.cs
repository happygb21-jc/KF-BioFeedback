namespace Kingfar.BioFeedback.Entites
{
    public interface ICreationFilter
    {
        DateTime? CreationTime { get; set; }
        Guid? CreatorId { get; set; }
    }
}