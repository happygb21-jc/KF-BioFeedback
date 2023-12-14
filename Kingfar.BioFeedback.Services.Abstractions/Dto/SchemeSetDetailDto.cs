namespace Kingfar.BioFeedback.Services
{
    public class SchemeSetDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SchemeSetType Type { get; set; }
        public List<TrainDetialDto> Trains { get; set; }
        public bool IsPublished { get; set; }
    }
}