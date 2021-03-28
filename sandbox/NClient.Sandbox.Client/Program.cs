using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Extensions;
using NClient.Sandbox.ProxyService.Facade;
using Polly;

namespace NClient.Sandbox.Client
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var retryPolicy = Policy
                .Handle<Exception>()
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

            var weatherForecasts = client.Get();
            Console.WriteLine($"Forecast length: {weatherForecasts.Count()}.");
        }
    }
}
