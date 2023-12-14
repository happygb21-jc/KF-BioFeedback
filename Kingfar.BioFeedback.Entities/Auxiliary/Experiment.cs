namespace Kingfar.BioFeedback.Entities.Entities
{
    #region Kingfar.Expert 中参数类型

    public class Experiment
    {
        public string Id { get; set; }

        public int ModelType { get; set; }

        public string Name { get; set; }

        public string[]? FileNames { get; set; }

        public double Rank { get; set; }

        public ExperimentStat[]? Stats { get; set; }

        public int[]? UsedBlockFormTypes { get; set; }

        public int[]? Requirements { get; set; }

        public int Icon { get; set; }

        public Pair<int>[]? Cover { get; set; }
    }

    #endregion Kingfar.Expert 中参数类型

    public record struct Pair<TValue>
    {
        public string? Name { get; set; }

        public TValue? Value { get; set; }
    }
}