using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NClient.AspNetCore.Extensions;
using NClient.Extensions.DependencyInjection;
using NClient.Sandbox.ProxyService.Clients;

namespace NClient.Sandbox.ProxyService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning(o => o.AssumeDefaultVersionWhenUnspecified = true);
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "ProxyApi", Version = "v1" });
                x.SwaggerDoc("v2", new OpenApiInfo { Title = "ProxyApi", Version = "v2" });
                x.SwaggerDoc("v3", new OpenApiInfo { Title = "ProxyApi", Version = "v3" });
            });

            services.AddLogging().AddHttpClient(nameof(IThirdPartyWeatherForecastClient));

            services.AddNClientControllers().WithResponseExceptions();
            services.AddNClient<IThirdPartyWeatherForecastClient>(
                host: "http://localhost:5001",
                httpClientName: nameof(IThirdPartyWeatherForecastClient));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("v1/swagger.json", "v1");
                x.SwaggerEndpoint("v2/swagger.json", "v2");
                x.SwaggerEndpoint("v3/swagger.json", "v3");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
