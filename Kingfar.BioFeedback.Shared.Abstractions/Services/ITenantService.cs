using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Shared
{
    public interface ITenantService
    {
        void SetTenantId(string tenantId, string ConnectionString, DbType dbType, bool enableUnderLine);
        string? CurrentTenantId { get; }
        DbConnectionConfig TenantConnConfig { get;}
    }
}
