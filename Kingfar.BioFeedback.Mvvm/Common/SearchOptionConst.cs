namespace Kingfar.BioFeedback.Mvvm.Common;

public static class SearchOptionConst
{
    public static List<TrainTypeOption> SchemeTypeOptions = new()
    {
        new TrainTypeOption("评估", TrainType.Access),
        new TrainTypeOption("个人生物反馈", TrainType.Individual),
        new TrainTypeOption("团体生物反馈", TrainType.Group),
    };

    public static List<TrainApplicationTypeOption> SchemeApplicationTypeOptions = new()
    {
        new TrainApplicationTypeOption("放松训练", TrainApplicationType.Relax),
        new TrainApplicationTypeOption("脑波训练", TrainApplicationType.Brainwave),
        new TrainApplicationTypeOption("生理监测", TrainApplicationType.Physiology),
        new TrainApplicationTypeOption("情绪调节", TrainApplicationType.Emotion),
        new TrainApplicationTypeOption("脑认知增强", TrainApplicationType.BrainCognition),
        new TrainApplicationTypeOption("团队生物反馈", TrainApplicationType.GroupBioFeedback),
        new TrainApplicationTypeOption("VR场景训练", TrainApplicationType.VR),
        new TrainApplicationTypeOption("心理应激", TrainApplicationType.Psychology),
        new TrainApplicationTypeOption("HRV心理一致性", TrainApplicationType.HRV),
    };
}