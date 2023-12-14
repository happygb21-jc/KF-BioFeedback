using Kingfar.BioFeedback.Entities.Entities;

namespace Kingfar.BioFeedback.Services
{
    public class ExperimentDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int ModelType { get; set; }
        public Pair<int>[]? Cover { get; set; }
    }
}