using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Extensions;
using NClient.Sandbox.ProxyService.Facade;
using NClient.Sandbox.ProxyService.Facade.Dto;
using Polly;

namespace NClient.Sandbox.Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var retryPolicy = Policy
                .HandleResult<HttpResponse>(x => !x.IsSuccessful)
                .Or<Exception>()
                .WaitAndRetryAsync(retryCount: 3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            var logger = new ServiceCollection()
                .AddLogging(x => x.AddConsole().SetMinimumLevel(LogLevel.Trace))
                .BuildServiceProvider()
                .GetRequiredService<ILogger<IWeatherForecastClient>>();

            var client = NClientProvider
                .Use<IWeatherForecastClient>(host: "http://localhost:5000")
                .WithResiliencePolicy(retryPolicy)
                .WithLogging(logger)
                .Build();

            var filter = new WeatherForecastFilter { Id = 1, Date = null };
            var weatherForecast = await client.GetAsync(filter);
            Console.WriteLine($"Forecast summary: {weatherForecast.Summary}.");

            var newWeatherForecast = new WeatherForecastDto { Id = 2, Date = DateTime.Now, Summary = "Cold", TemperatureC = -30 };
            await client.PostAsync(newWeatherForecast);
            Console.WriteLine("Forecast is posted.");

            var updatedWeatherForecast = new WeatherForecastDto { Id = 2, Date = DateTime.Now, Summary = "Cold", TemperatureC = -35 };
            await client.PutAsync(updatedWeatherForecast);
            Console.WriteLine("Forecast is put.");
            
            await client.DeleteAsync(newWeatherForecast.Id);
            Console.WriteLine("Forecast is deleted.");
        }
    }
}
