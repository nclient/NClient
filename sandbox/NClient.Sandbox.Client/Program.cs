using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.Resilience;
using NClient.Sandbox.Client.ClientHandlers;
using NClient.Sandbox.FileService.Facade;
using NClient.Sandbox.ProxyService.Facade;
using NClient.Sandbox.ProxyService.Facade.Dto;
using Polly;

namespace NClient.Sandbox.Client
{
    public class Program
    {
        private static ILogger<Program> _programLogger = null!;
        private static IWeatherForecastClient _weatherForecastClient = null!;
        private static IFileClient _fileClient = null!;

        public static async Task Main(string[] args)
        {
            Init();

            await (args.Single() switch
            {
                "weather" => TestWeatherForecastClientAsync(),
                "file" => TestFileClientAsync(),
                { } testName => throw new NotSupportedException($"The test name '{testName}' not supported.")
            });
        }

        private static void Init()
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
                fallbackValue: default!,
                onFallbackAsync: x => throw (x.Exception ?? x.Result.HttpResponse.ErrorException!));

            var handlerLogger = serviceProvider.GetRequiredService<ILogger<LoggingClientHandler>>();
            var weatherForecastClientLogger = serviceProvider.GetRequiredService<ILogger<IWeatherForecastClient>>();
            var fileClientLogger = serviceProvider.GetRequiredService<ILogger<IFileClient>>();
            _programLogger = serviceProvider.GetRequiredService<ILogger<Program>>();

            _weatherForecastClient = NClientProvider
                .Use<IWeatherForecastClient>(host: "http://localhost:5000")
                .WithCustomHandlers(new IClientHandler[]
                {
                    new LoggingClientHandler(handlerLogger),
                })
                .WithResiliencePolicy(fallbackPolicy.WrapAsync(retryPolicy))
                .WithResiliencePolicy(
                    methodSelector: x => (Func<WeatherForecastDto, Task>)x.PostAsync,
                    asyncPolicy: fallbackPolicy)
                .WithLogging(weatherForecastClientLogger)
                .Build();

            _fileClient = NClientProvider
                .Use<IFileClient>(host: "http://localhost:5002")
                .WithCustomHandlers(new IClientHandler[]
                {
                    new LoggingClientHandler(handlerLogger),
                })
                .WithResiliencePolicy(fallbackPolicy.WrapAsync(retryPolicy))
                .WithResiliencePolicy(
                    methodSelector: x => (Func<byte[], Task>)x.PostTextFileAsync,
                    asyncPolicy: fallbackPolicy)
                .WithLogging(fileClientLogger)
                .Build();
        }

        private static async Task TestWeatherForecastClientAsync()
        {
            var weatherForecast = await _weatherForecastClient.GetAsync(new WeatherForecastFilter { Id = 1, Date = null });
            _programLogger.LogInformation($"The forecast summary: {weatherForecast.Summary}.");

            var nonExistingWeatherForecast = await _weatherForecastClient.GetAsync(new WeatherForecastFilter { Id = 0, Date = null });
            _programLogger.LogInformation($"The forecast not found.");

            var newWeatherForecast = new WeatherForecastDto { Id = 2, Date = DateTime.Now, Summary = "Cold", TemperatureC = -30 };
            await _weatherForecastClient.PostAsync(newWeatherForecast);
            _programLogger.LogInformation("The forecast is posted.");

            var updatedWeatherForecast = new WeatherForecastDto { Id = 2, Date = DateTime.Now, Summary = "Cold", TemperatureC = -35 };
            await _weatherForecastClient.PutAsync(updatedWeatherForecast);
            _programLogger.LogInformation("The forecast is put.");

            await _weatherForecastClient.DeleteAsync(newWeatherForecast.Id);
            _programLogger.LogInformation("The forecast is deleted.");
        }

        private static async Task TestFileClientAsync()
        {
            const string tmpFolderName = "tmp";
            const string receivedFilesFolderName = "receivedFiles";
            var receivedFilesDirPath = Path.Combine(tmpFolderName, receivedFilesFolderName);
            Directory.CreateDirectory(receivedFilesDirPath);

            var httpResponseWithText = await _fileClient.GetTextFileAsync(id: 1);
            _programLogger.LogInformation($"The text file was received.");
            await using (var textFileStream = File.Create(Path.Combine(receivedFilesDirPath, "TextFileFromBytes.txt")))
            {
                await textFileStream.WriteAsync(httpResponseWithText.RawBytes);
                _programLogger.LogInformation($"The text file was saved.");
            }

            await _fileClient.PostTextFileAsync(httpResponseWithText.RawBytes!);
            _programLogger.LogInformation($"The text file has been sent.");

            var httpResponseWithImage = await _fileClient.GetImageAsync(id: 1);
            _programLogger.LogInformation($"The image was received.");
            await using (var imageStream = File.Create(Path.Combine(receivedFilesDirPath, "ImageFromBytes.jpeg")))
            {
                await imageStream.WriteAsync(httpResponseWithImage.RawBytes);
                _programLogger.LogInformation($"The image was saved.");
            }

            await _fileClient.PostImageFileAsync(httpResponseWithImage.RawBytes!);
            _programLogger.LogInformation($"The image has been sent.");

            Directory.Delete(Path.GetFullPath(tmpFolderName), recursive: true);
        }
    }
}
