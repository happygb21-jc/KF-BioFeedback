namespace Kingfar.BioFeedback.Entities.Entities;

//需要考虑后续会不会有医生参与报告分析，并且针对报告给予结果反馈
[SugarTable("bf_TrainResult")]
[Serializable]
[SysTable]
public class TrainResult : ModificationEntity<Guid>
{
    [SugarColumn(ColumnDescription = "方案Id")]
    public Guid SchemeSetId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(SchemeSetId))]
    public SchemeSet SchemeSet { get; set; } = default!;

    [SugarColumn(ColumnDescription = "训练Id")]
    public Guid TrainId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(TrainId))]
    public Train Train { get; set; }

    [SugarColumn(ColumnDescription = "患者Id")]
    public Guid UserId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public AppUser User { get; set; } = default!;

    [SugarColumn(ColumnDescription = "训练名称")]
    public SchemeResultState State { get; set; }

    [SugarColumn(ColumnDescription = "训练开始时间")]
    public DateTime BeginExpTime { get; set; }

    [SugarColumn(ColumnDescription = "训练结束时间")]
    public DateTime EndExpTime { get; set; }

    [SugarColumn(ColumnDescription = "文件夹路径")]
    public string FolderPath { get; set; }

    public string CurrentPath => FolderPath.CombinePath(Id.ToString());

    /// <summary>
    /// 包含实验的采集时间范围及对应统计数据 问题：不知道是从json里获取还是回传数据
    /// </summary>
    [SugarColumn(ColumnDescription = "实验报告相关")]
    public ExperimentReport[]? Reports { get; set; }
}

public enum SchemeResultState
{
    Running = 0,
    Offline = 1,
    Staged = 2,
    Aborted = 3,
    Submitted = 4
}