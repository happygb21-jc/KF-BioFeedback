using Kingfar.BioFeedback.Shared;

namespace Kingfar.BioFeedback.Services
{
    public class SchemeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TrainType Type { get; set; }
        public List<TrainApplicationType> ApplicationTypes { get; set; }
        public DateTime CreationTime { get; set; }
    }
}