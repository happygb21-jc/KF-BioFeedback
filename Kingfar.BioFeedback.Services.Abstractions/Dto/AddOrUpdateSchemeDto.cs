namespace Kingfar.BioFeedback.Services
{
    public class AddOrUpdateSchemeDto
    {
        public AddOrUpdateSchemeDto()
        { }

        public void SetValue(string name, TrainType type, List<TrainApplicationType> applicationTypes)
        {
            Name = name;
            Type = type;
            ApplicationTypes = applicationTypes;
        }

        public Guid? Id { get; set; }
        public string Name { get; set; }
        public TrainType Type { get; set; }

        public List<TrainApplicationType> ApplicationTypes { get; set; }
    }
}