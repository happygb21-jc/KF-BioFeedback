namespace Kingfar.BioFeedback.Entities.Entities
{
    public class ExperimentReport
    {
        public Guid ExperimentId { get; set; }
        public double[] Stats { get; set; }
        public DateTime BeginRunTime { get; set; }
        public DateTime EndRunTime { get; set; }
    }
}