namespace Kingfar.BioFeedback.Entities.Entities;

//放入到方案集里，训练内容的类型就不可修改了，能改的只有参数，其他的只是用来展示不能变更
//创建这个也是为了让训练内容在方案集中也可以进行重新编辑
[SugarTable("bf_Trains")]
[Serializable]
[SysTable]
public class Train : ModificationEntity<Guid>
{
    [SugarColumn(ColumnDescription = "训练名称")]
    public string Name { get; set; } = string.Empty;

    [SugarColumn(ColumnDescription = "发布配置", IsJson = true)]
    public PublishSettings PublishSettings { get; set; } = default!;

    [SugarColumn(ColumnDescription = "采集数量")]
    public int CollectCount { get; set; } = 0;

    [SugarColumn(ColumnDescription = "训练内容排序")]
    public int Rank { get; set; }

    [SugarColumn(ColumnDescription = "文件夹路径")]
    public string FolderPath { get; set; } = string.Empty;

    public string CurrentPath => FolderPath.CombinePath(Id.ToString());

    public Guid SchemeSetId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(SchemeSetId))]
    public SchemeSet SchemeSet { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(Id), nameof(TrainContent.TrainId))]
    public TrainContent Content { get; set; } = default!;

    [Navigate(NavigateType.OneToMany, nameof(TrainResult.TrainId))]
    public List<TrainResult> Results { get; set; }
}