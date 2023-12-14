namespace Kingfar.BioFeedback.Entities.Entities;

[SugarTable("bf_SchemeAppTypes")]
[Serializable]
[SysTable]
public class SchemeAppType : Entity<Guid>
{
    public Shared.TrainApplicationType Type { get; set; }

    public Guid SchemeId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(SchemeId))]
    public Scheme Scheme { get; set; }
}