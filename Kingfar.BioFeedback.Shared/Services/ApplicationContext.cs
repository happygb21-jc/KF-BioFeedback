using Microsoft.Extensions.Configuration;
using System.Linq;
using Kingfar.BioFeedback.Shared.Abstractions;
using Kingfar.BioFeedback.Shared.Abstractions.Common;

namespace Kingfar.BioFeedback.Shared
{
    public class ApplicationContext : IApplicationContext
    {
        private ICurrentUser _currentUse = default!;
        public ICurrentUser CurrentUser => _currentUse;

        private string? _tenantId;
        private DbConnectionConfig? _tenantConnConfig;
        public string? CurrentTenantId => _tenantService.CurrentTenantId;
        public DbConnectionConfig TenantConnConfig => _tenantConnConfig!;
        private SysSetting _systemInfo = default!;
        public SysSetting SystemInfo => _systemInfo;

        private DbConnectionConfig _mainConnConfig;
        private readonly ITenantService _tenantService;

        public ApplicationContext(IConfiguration configuration, ITenantService tenantService)
        {
            _tenantService = tenantService;
            var dbOptions = configuration.GetSection(SqlSugarConst.DbSectionKey).Get<DbConnectionOptions>();
            _mainConnConfig = dbOptions!.ConnectionConfigs.First(u => u.ConfigId == SqlSugarConst.MainConfigId);
            _systemInfo = SysSetting.Current;
        }

        public void SetTenantId(string tenantId, string dbFilePath)
        {
            _tenantService.SetTenantId(tenantId, dbFilePath, _mainConnConfig.DbType, _mainConnConfig.DbSettings.EnableUnderLine);
        }

        public void SetCurrentUser(ICurrentUser user)
        {
            _currentUse = user;
        }
    }
}