using Prism.Ioc;

namespace Kingfar.BioFeedback.Services
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// 用于依赖注入 IServices 注册
        /// </summary>
        /// <param name="services"></param>
        public static void AddServices(this IContainerRegistry services)
        {
            services.RegisterScoped<IAccountService, AccountService>();
            services.RegisterScoped<ISchemeSetService, SchemeSetService>();
            services.RegisterScoped<ISchemeService, SchemeService>();
            services.RegisterScoped<ISchemeSetTemplateService, SchemeSetTemplateService>();
        }
    }
}