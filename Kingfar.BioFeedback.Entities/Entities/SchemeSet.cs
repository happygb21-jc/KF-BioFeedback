using System;

namespace Kingfar.BioFeedback.Entities.Entities;

[SugarTable("bf_SchemeSet")]
[Serializable]
[SysTable]
public class SchemeSet : FullEntity<Guid>
{
    [SugarColumn(ColumnDescription = "方案名称")]
    public string Name { get; set; } = string.Empty;

    [SugarColumn(ColumnDescription = "方案类型")]
    public SchemeSetType Type { get; set; }

    [SugarColumn(ColumnDescription = "发布时间")]
    public DateTime? PublishedDate { get; set; }

    [SugarColumn(ColumnDescription = "结束时间")]
    public DateTime? EndDate { get; set; }

    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(Train.SchemeSetId))]
    public List<Train> Trains { get; set; }

    public bool IsPublished { get; set; } = false;

    [SugarColumn(ColumnDescription = "文件夹路径")]
    public string FolderPath { get; set; }
    public string CurrentPath => FolderPath.CombinePath(Id.ToString());
    public SchemeSetTemplate CopyToTemplate(Guid? id, string folderPath)
    {
        return new SchemeSetTemplate()
        {
            Id = id is not null ? id.Value : Guid.NewGuid(),
            Name = Name,
            Type = Type,
            Trains = new(),
            FolderPath = folderPath
        };
    }
}