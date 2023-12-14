using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.Shared
{
    /// <summary>
    /// 租户服务 TODO 增加Cache 用户存储DbConnectionConfig (放到SqlSugarClient)
    /// </summary>
    public class TenantService : ITenantService
    {
        private string? _tenantId;
        private DbConnectionConfig? _tenantConnConfig;
        public string? CurrentTenantId => _tenantId;
        public DbConnectionConfig TenantConnConfig => _tenantConnConfig!;
        public TenantService(IConfiguration configuration) {

        }

        public void SetTenantId(string tenantId, string dbFilePath,DbType dbType, bool enableUnderLine)
        {
            _tenantId = tenantId;
            _tenantConnConfig = new DbConnectionConfig
            {
                ConfigId = tenantId,
                DbType = dbType,
                IsAutoCloseConnection = true,
                ConnectionString = dbFilePath,
                DbSettings = new DbSettings()
                {
                    EnableUnderLine = enableUnderLine,
                }
            };
        }
    }
}
