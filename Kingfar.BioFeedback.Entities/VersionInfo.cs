namespace Kingfar.BioFeedback.DataAccess
{
    [SugarTable("sys_VersionInfo")]
    public class VersionInfo : Entity<int>
    {
        public string Version { get; set; } = default!;
    }
}