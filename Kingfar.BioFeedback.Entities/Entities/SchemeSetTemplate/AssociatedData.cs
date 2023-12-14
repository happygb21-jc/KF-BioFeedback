namespace Kingfar.BioFeedback.Entities.Entities;

//这张表存在的意义:用于确认方案集关联了哪些内容，包括后续资源也会出现关联
[SugarTable("bf_AssociatedData")]
[Serializable]
[SysTable]
public class AssociatedData : Entity<Guid>
{
    public AssociatedData()
    { }

    public AssociatedData(string sourceName, Guid sourceId, string targetName, Guid targetId)
    {
        SourceName = sourceName;
        SourceId = sourceId;
        TargetName = targetName;
        TargetId = targetId;
    }

    public string SourceName { get; set; } = string.Empty;
    public Guid SourceId { get; set; }
    public string TargetName { get; set; } = string.Empty;
    public Guid TargetId { get; set; }
}