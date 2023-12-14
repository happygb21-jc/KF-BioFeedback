using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;

namespace Kingfar.BioFeedback.Shared
{
    public static class SharedContainerExtensions
    {
        public static void AddShared(this IContainerRegistry services)
        {
            services.RegisterSingleton<IAppMapper, AppMapper>();
            services.RegisterSingleton<ITenantService, TenantService>();
            services.RegisterSingleton<IApplicationContext, ApplicationContext>();
        }
    }
}
