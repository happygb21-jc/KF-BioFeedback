using Kingfar.BioFeedback.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Prism.Ioc;

namespace Kingfar.BioFeedback.WebHost;

public class WebApiServer : IWebApiServer
{
    private IHost? _webHost;
    private IContainerProvider? _containerProvider;
    private bool _isDispose = true;

    public void Start(string apiUrl, IContainerProvider containerProvider)
    {
        if (_isDispose)
        {
            _containerProvider = containerProvider;
            _webHost = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(apiUrl);
                    webBuilder.ConfigureServices(ConfigureServices);
                    webBuilder.Configure(Configure);
                })
                .Build();
            _isDispose = false;
            _webHost.Start();
        }
    }

    public void Stop()
    {
        if (_webHost is not null)
        {
            Task.Run(() => _webHost.StopAsync());
            _webHost.Dispose();
            _isDispose = true;
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
        // 添加 Swagger 服务
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
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

        // 启用 Swagger UI
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });
    }
}