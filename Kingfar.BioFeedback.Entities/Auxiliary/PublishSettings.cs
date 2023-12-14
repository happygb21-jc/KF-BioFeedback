namespace Kingfar.BioFeedback.Entities
{
    #region Kingfar.Expert 中参数类型

    /// <summary>
    /// 方案发布设置
    /// </summary>
    public record class PublishSettings
    {
        /// <summary>
        /// 是否允许新创建被试
        /// </summary>
        public bool AcceptNewSubject { get; init; } = false;

        /// <summary>
        /// 是否允许暂存
        /// </summary>
        public bool AllowStage { get; init; } = true;

        /// <summary>
        /// 允许的最大运行次数
        /// </summary>
        public int RunLimit { get; init; } = 1;

        /// <summary>
        /// 终止的结果是否计入次数限制
        /// </summary>
        public bool IncludeAborted { get; init; } = true;

        /// <summary>
        /// 提交结果后是否显示报告
        /// </summary>
        public bool ShowResult { get; init; } = true;
    }

    #endregion Kingfar.Expert 中参数类型
}