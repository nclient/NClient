using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.Resilience;
using NClient.Sandbox.Client.ClientHandlers;
using NClient.Sandbox.ProxyService.Facade;
using NClient.Sandbox.ProxyService.Facade.Dto;
using Polly;

namespace NClient.Sandbox.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(x => x.AddConsole().SetMinimumLevel(LogLevel.Trace))
                .BuildServiceProvider();

            var basePolicy = Policy<ResponseContext>.HandleResult(x =>
            {
                if (x.MethodInvocation.MethodInfo.Name == nameof(IWeatherForecastClient.GetAsync) && x.HttpResponse.StatusCode == HttpStatusCode.NotFound)
                    return false;
                return !x.HttpResponse.IsSuccessful;
            }).Or<Exception>();
            var retryPolicy = basePolicy.WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            var fallbackPolicy = basePolicy.FallbackAsync(
                fallbackValue: default(ResponseContext)!,
                onFallbackAsync: x => throw (x.Exception ?? x.Result.HttpResponse.ErrorException!));
            var policy = fallbackPolicy.WrapAsync(retryPolicy);

            var handlerLogger = serviceProvider.GetRequiredService<ILogger<LoggingClientHandler>>();
            var clientLogger = serviceProvider.GetRequiredService<ILogger<IWeatherForecastClient>>();
            var programLogger = serviceProvider.GetRequiredService<ILogger<Program>>();

            var client = NClientProvider
                .Use<IWeatherForecastClient>(host: "http://localhost:5000")
                .WithCustomHandlers(new IClientHandler[]
                {
                    new LoggingClientHandler(handlerLogger),
                })
                .WithResiliencePolicy(policy)
                .WithResiliencePolicy(x => (Func<WeatherForecastDto, Task>)x.PostAsync, policy)
                .WithLogging(clientLogger)
                .Build();

            var weatherForecast = await client.GetAsync(new WeatherForecastFilter { Id = 1, Date = null });
            programLogger.LogInformation($"Forecast summary: {weatherForecast.Summary}.");

            var nonExistingWeatherForecast = await client.GetAsync(new WeatherForecastFilter { Id = 0, Date = null });
            programLogger.LogInformation($"Forecast not found.");

            var newWeatherForecast = new WeatherForecastDto { Id = 2, Date = DateTime.Now, Summary = "Cold", TemperatureC = -30 };
            await client.PostAsync(newWeatherForecast);
            programLogger.LogInformation("Forecast is posted.");

            var updatedWeatherForecast = new WeatherForecastDto { Id = 2, Date = DateTime.Now, Summary = "Cold", TemperatureC = -35 };
            await client.PutAsync(updatedWeatherForecast);
            programLogger.LogInformation("Forecast is put.");

            await client.DeleteAsync(newWeatherForecast.Id);
            programLogger.LogInformation("Forecast is deleted.");
        }
    }
}
