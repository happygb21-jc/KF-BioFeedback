namespace Kingfar.BioFeedback.Services
{
    public class TrainDetialDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ExperimentDto>? Experiments { get; set; }
    }
}