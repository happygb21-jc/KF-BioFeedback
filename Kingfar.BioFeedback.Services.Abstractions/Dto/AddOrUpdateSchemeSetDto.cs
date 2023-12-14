namespace Kingfar.BioFeedback.Services
{
    public class AddOrUpdateSchemeSetDto
    {
        public AddOrUpdateSchemeSetDto()
        { }

        public AddOrUpdateSchemeSetDto(string name, SchemeSetType type, Guid[] schemeIds)
        {
            Name = name;
            Type = type;
            SchemeIds = schemeIds;
        }

        public string Name { get; set; }

        public SchemeSetType Type { get; set; }

        public Guid[] SchemeIds { get; set; }
    }
}