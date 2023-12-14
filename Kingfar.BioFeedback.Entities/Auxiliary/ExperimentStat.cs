namespace Kingfar.BioFeedback.Entities.Entities
{
    public class ExperimentStat
    {
        public int Type { get; set; }

        public string Name { get; set; }

        public string? BlockName { get; set; }

        public ExperimentStat Clone()
        {
            return (ExperimentStat)this.MemberwiseClone();
        }
    }
}