namespace Kingfar.BioFeedback.Entities.Entities;

//这里的scheme对应的expert的方案，没有进行改变，此处也相对用作模板使用
//创建方案集时会吧所有内容进行copy插入到新表，可以在方案中编辑而不影响此处模板
[SugarTable("bf_Schemes")]
[Serializable]
[SysTable]
public class Scheme : FullEntity<Guid>
{
    public void SetValue(string name, TrainType type, List<SchemeAppType> applicationTypes)
    {
        Name = name;
        Type = type;
        ApplicationTypes = applicationTypes;
    }

    public Train CopyToTrain(Guid id, int rank, string folderPath)
    {
        return new Train()
        {
            Id = id,
            Name = Name,
            PublishSettings = PublishSettings,
            Content = new TrainContent() { Experiments = Content?.Experiments },
            Rank = rank,
            FolderPath = folderPath
        };
    }

    [SugarColumn(ColumnDescription = "训练名称")]
    public string Name { get; set; }

    [SugarColumn(ColumnDescription = "训练类型")]
    public TrainType Type { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(SchemeAppType.SchemeId))]
    public List<SchemeAppType>? ApplicationTypes { get; set; }

    [SugarColumn(ColumnDescription = "发布配置", IsJson = true)]
    public PublishSettings PublishSettings { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(Id), nameof(SchemeContent.SchemeId))]
    public SchemeContent? Content { get; set; }

    [SugarColumn(ColumnDescription = "文件夹路径")]
    public string FolderPath { get; set; }

    public string CurrentPath => FolderPath.CombinePath(Id.ToString());
}