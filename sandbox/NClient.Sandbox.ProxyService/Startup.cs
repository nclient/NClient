using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddLogging().AddHttpClient(nameof(IThirdPartyWeatherForecastClient));
            services.AddSwaggerDocument();
            services.AddNClientControllers();
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

            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseReDoc();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
