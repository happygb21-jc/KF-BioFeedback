using Kingfar.BioFeedback.DataAccess;
using Kingfar.BioFeedback.Services;
using Prism.Ioc;

namespace Kingfar.BioFeedback.WebHost;

public class WebApiServer : IWebApiServer
{
    private IHost? webHost;
    private IContainerProvider _containerProvider;

    public void Start(string apiUrl, IContainerProvider containerProvider)
    {
        if (webHost == null)
        {
            _containerProvider = containerProvider;
            webHost = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(apiUrl);
                    webBuilder.ConfigureServices(ConfigureServices);
                    webBuilder.Configure(Configure);
                })
                .Build();

            webHost.Start();
        }
    }

    public void Stop()
    {
        if (webHost is not null)
        {
            webHost.StopAsync().GetAwaiter().GetResult();
            webHost.Dispose();
            webHost = null;
        }
    }

    private void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        services.AddControllers();
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOrigin", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
        services.AddScoped<IAccountService>(provider => _containerProvider.Resolve<IAccountService>());
        services.AddScoped<ISchemeSetService>(provider => _containerProvider.Resolve<ISchemeSetService>());
        services.AddScoped<ITrainResultService>(provider => _containerProvider.Resolve<ITrainResultService>());
        services.AddScoped<ITrainService>(provider => _containerProvider.Resolve<ITrainService>());
        services.AddMvcCore().AddApiExplorer();
    }

    private void Configure(WebHostBuilderContext context, IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseCors("AllowAnyOrigin");
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}