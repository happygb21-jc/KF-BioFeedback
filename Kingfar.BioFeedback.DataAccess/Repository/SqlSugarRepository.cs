using SqlSugar.Extensions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Kingfar.BioFeedback.Entites;

namespace Kingfar.BioFeedback.DataAccess
{
    public class SqlSugarRepository<TEntity> : SimpleClient<TEntity>, ISqlSugarRepository<TEntity> where TEntity : class, new()
    {
        protected ITenant iTenant;

        public SqlSugarRepository(ISqlSugarClient context, Shared.ITenantService tenantService) : base(context)
        {
            iTenant = context.AsTenant();

            // 若实体贴有系统表特性，则返回默认库连接
            if (typeof(TEntity).IsDefined(typeof(SysTableAttribute), false))
            {
                base.Context = iTenant.GetConnectionScope(SqlSugarConst.MainConfigId);
                return;
            }

            // 若实体贴有多库特性，则返回指定库连接
            if (!string.IsNullOrEmpty(tenantService.CurrentTenantId))
            {
                if (!iTenant.IsAnyConnection(tenantService.CurrentTenantId))
                {
                    // 设置租户库连接配置
                    iTenant.AddConnection(tenantService.TenantConnConfig);
                    tenantService.TenantConnConfig!.SetDbConfig();
                    iTenant.GetConnectionScope(tenantService.CurrentTenantId).SetDbAop();
                    base.Context = iTenant.GetConnectionScope(tenantService.CurrentTenantId);
                }
                else
                {
                    base.Context = iTenant.GetConnectionScope(tenantService.CurrentTenantId);
                    return;
                }
            }
        }
    }
}