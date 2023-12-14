namespace Kingfar.BioFeedback.Entities.Entities;

[SugarTable("bf_SchemeSetTemplates")]
[Serializable]
[SysTable]
public class SchemeSetTemplate : CreationEntity<Guid>
{
    [SugarColumn(ColumnDescription = "方案名称")]
    public string Name { get; set; } = string.Empty;

    [SugarColumn(ColumnDescription = "方案类型")]
    public SchemeSetType Type { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(TrainTemplate.SchemeSetTemplateId))]
    public List<TrainTemplate> Trains { get; set; }

    [SugarColumn(ColumnDescription = "文件夹路径")]
    public string FolderPath { get; set; }

    public string CurrentPath => FolderPath.CombinePath(Id.ToString());

    public SchemeSet CreateToSchemeSet(string schemeSetName)
    {
        //这里只复制了数据，并没有复制相应的参数文件
        return new SchemeSet()
        {
            Id = Guid.NewGuid(),
            Type = Type,
            Name = schemeSetName,
            Trains = new(),
        };
    }
}