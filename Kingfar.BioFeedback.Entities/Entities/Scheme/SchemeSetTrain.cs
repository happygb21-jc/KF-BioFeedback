using SqlSugar;

namespace Kingfar.BioFeedback.Entities.Entities;

[SugarTable("SchemeSetTrains")]
[Serializable]
[SysTable]
public class SchemeSetTrain
{
    [SugarColumn(IsNullable = false, IsPrimaryKey = true, ColumnDescription = "方案主键")]
    public Guid SchemeSetId { get; set; }

    /// <summary>
    /// 可以用于判断引用，但是如果内容是复制到方案集下的，其实好像就没啥用了，相当于创建了新的方案
    /// 如果相同方案可以做两次，那这里就SchemeId就不能为Key
    /// </summary>
    [SugarColumn(IsNullable = false, IsPrimaryKey = true, ColumnDescription = "训练主键")]
    public Guid SchemeId { get; set; }

    //如果上述主试内容成立，这里也就不需要进行引用关联了
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToOne, nameof(Scheme.Id))]
    public Scheme Train { get; set; }

    [SugarColumn(ColumnDescription = "训练排序")]
    public int OrderNum { get; set; }
}