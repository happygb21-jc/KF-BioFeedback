namespace Kingfar.BioFeedback.Entities.Entities;

//tar文件为后上传，上传时必带方案Id，所以上传完读取scheme.json文件，进行这里的追加配置，用于记录方案有多少个实验及实验的统计指标
//修改的时候也可以直接添加新的内容，并移除旧数据而不影响方案的基础信息
//每个方案的配置对应一个.scheme后缀的tar压缩包
[SugarTable("bf_TrainContent")]
[Serializable]
[SysTable]
public class TrainContent : ModificationEntity<Guid>
{
    public Guid TrainId { get; set; }

    //对于此系统，是用于展示训练内容
    [SugarColumn(ColumnDescription = "实验内容", IsJson = true)]
    public List<Experiment>? Experiments { get; set; }
}