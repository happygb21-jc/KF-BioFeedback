using Kingfar.BioFeedback.Shared.Abstractions;
using Kingfar.BioFeedback.Shared.Abstractions.Common;
using NewLife.Common;

namespace Kingfar.BioFeedback.Shared
{
    public interface IApplicationContext
    {
        ICurrentUser? CurrentUser { get; }

        void SetCurrentUser(ICurrentUser user);

        void SetTenantId(string tenantId, string ConnectionString);

        string? CurrentTenantId { get; }
        DbConnectionConfig TenantConnConfig { get; }
        SysSetting SystemInfo { get; }
    }
}