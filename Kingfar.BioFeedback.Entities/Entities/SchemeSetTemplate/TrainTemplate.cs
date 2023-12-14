namespace Kingfar.BioFeedback.Entities.Entities;

[SugarTable("bf_TrainTemplates")]
[Serializable]
[SysTable]
public class TrainTemplate : Entity<Guid>
{
    [SugarColumn(ColumnDescription = "训练名称")]
    public string Name { get; set; } = string.Empty;

    [SugarColumn(ColumnDescription = "发布配置", IsJson = true)]
    public PublishSettings PublishSettings { get; set; } = default!;

    [SugarColumn(ColumnDescription = "实验内容", IsJson = true)]
    public List<Experiment>? Experiments { get; set; }

    [SugarColumn(ColumnDescription = "训练内容排序")]
    public int Rank { get; set; }

    [SugarColumn(ColumnDescription = "文件夹路径")]
    public string FolderPath { get; set; }

    public string CurrentPath => FolderPath.CombinePath(Id.ToString());

    public Guid SchemeSetTemplateId { get; set; }
}