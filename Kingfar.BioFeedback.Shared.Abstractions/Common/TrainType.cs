using System.ComponentModel;

namespace Kingfar.BioFeedback.Shared
{
    public enum SchemeSetType
    {
        [Description("个人训练方案")]
        Individual,

        [Description("团体训练方案")]
        Group
    }

    public enum TrainType
    {
        [Description("评估")]
        Access,

        [Description("个人生物反馈")]
        Individual,

        [Description("团体生物反馈")]
        Group
    }

    public enum TrainResultState
    {
        Running = 0,
        Offline = 1,
        Staged = 2,
        Aborted = 3,
        Submitted = 4
    }

    public enum TrainApplicationType
    {
        [Description("放松训练")]
        Relax,

        [Description("脑波训练")]
        Brainwave,

        [Description("生理监测")]
        Physiology,

        [Description("情绪调节")]
        Emotion,

        [Description("脑认知增强")]
        BrainCognition,

        [Description("团体生物反馈")]
        GroupBioFeedback,

        [Description("VR场景训练")]
        VR,

        [Description("心理应激")]
        Psychology,

        [Description("HRV心理一致性")]
        HRV
    }
}