namespace Kingfar.BioFeedback.Services
{
    public class SchemeSetTemplateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SchemeSetType Type { get; set; }
        public virtual DateTime? CreationTime { get; set; }

        //差一个List<SchemeDto>
    }
}