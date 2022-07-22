﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Providers.Transport;
using NClient.Sandbox.Client.ClientHandlers;
using NClient.Sandbox.FileService.Facade;
using NClient.Sandbox.ProxyService.Facade;
using NClient.Sandbox.ProxyService.Facade.Dto;
using Polly;

namespace NClient.Sandbox.Client
{
    [SuppressMessage("ReSharper", "DateTimeNow")]
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

            var basePolicy = Policy<IResponseContext<HttpRequestMessage, HttpResponseMessage>>
                .HandleResult(x => !x.Response.IsSuccessStatusCode)
                .Or<Exception>();
            var retryPolicy = basePolicy.WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            var fallbackPolicy = basePolicy.FallbackAsync(
                fallbackAction: (delegateResult, _, _) => Task.FromResult(delegateResult.Result),
                onFallbackAsync: (delegateResult, _) =>
                {
                    if (delegateResult.Exception is not null)
                        throw delegateResult.Exception;
                    delegateResult.Result.Response.EnsureSuccessStatusCode();
                    return Task.CompletedTask;
                });

            var handlerLogger = serviceProvider.GetRequiredService<ILogger<LoggingClientHandler>>();
            var weatherForecastClientLogger = serviceProvider.GetRequiredService<ILogger<IWeatherForecastClient>>();
            var fileClientLogger = serviceProvider.GetRequiredService<ILogger<IFileClient>>();
            _programLogger = serviceProvider.GetRequiredService<ILogger<Program>>();

            _weatherForecastClient = NClientGallery.Clients
                .GetRest()
                .For<IWeatherForecastClient>(host: new Uri("http://localhost:5000"))
                .WithHandling(new LoggingClientHandler(handlerLogger))
                .WithResilience(selector => selector
                    .ForAllMethods().UsePolly(fallbackPolicy.WrapAsync(retryPolicy))
                    .ForMethod(x => (Func<WeatherForecastDto, Task>) x.PostAsync).UsePolly(fallbackPolicy))
                .WithLogging(weatherForecastClientLogger)
                .Build();

            _fileClient = NClientGallery.Clients
                .GetRest()
                .For<IFileClient>(host: new Uri("http://localhost:5002"))
                .WithHandling(new LoggingClientHandler(handlerLogger))
                .WithResilience(selector => selector
                    .ForAllMethods().UsePolly(fallbackPolicy.WrapAsync(retryPolicy))
                    .ForMethod(x => (Func<byte[], Task>) x.PostTextFileAsync).UsePolly(fallbackPolicy))
                .WithLogging(fileClientLogger)
                .Build();
        }

        private static async Task TestWeatherForecastClientAsync()
        {
            var weatherForecast = await _weatherForecastClient.GetAsync(new WeatherForecastFilter { Id = 1, Date = null });
            _programLogger.LogInformation("The forecast summary: {Summary}", weatherForecast.Summary);

            await _weatherForecastClient.GetAsync(new WeatherForecastFilter { Id = 0, Date = null });
            _programLogger.LogInformation("The forecast not found");

            var newWeatherForecast = new WeatherForecastDto { Id = 2, Date = DateTime.Now, Summary = "Cold", TemperatureC = -30 };
            await _weatherForecastClient.PostAsync(newWeatherForecast);
            _programLogger.LogInformation("The forecast is posted");

            var updatedWeatherForecast = new WeatherForecastDto { Id = 2, Date = DateTime.Now, Summary = "Cold", TemperatureC = -35 };
            await _weatherForecastClient.PutAsync(updatedWeatherForecast);
            _programLogger.LogInformation("The forecast is put");

            await _weatherForecastClient.DeleteAsync(newWeatherForecast.Id);
            _programLogger.LogInformation("The forecast is deleted");
        }

        private static async Task TestFileClientAsync()
        {
            const string tmpFolderName = "tmp";
            const string receivedFilesFolderName = "receivedFiles";
            var receivedFilesDirPath = Path.Combine(tmpFolderName, receivedFilesFolderName);
            Directory.CreateDirectory(receivedFilesDirPath);

            var httpResponseWithText = await _fileClient.GetTextFileAsync(id: 1);
            _programLogger.LogInformation("The text file was received");

            await using var textMemoryStream = new MemoryStream();
            await httpResponseWithText.Content.Stream.CopyToAsync(textMemoryStream);
            var textBytes = textMemoryStream.ToArray();
                
            await using (var textFileStream = File.Create(Path.Combine(receivedFilesDirPath, "TextFileFromBytes.txt")))
            {
                await textFileStream.WriteAsync(textBytes);
                _programLogger.LogInformation("The text file was saved");
            }

            await _fileClient.PostTextFileAsync(textBytes);
            _programLogger.LogInformation("The text file has been sent");

            var httpResponseWithImage = await _fileClient.GetImageAsync(id: 1);
            
            await using var imageMemoryStream = new MemoryStream();
            await httpResponseWithImage.Content.Stream.CopyToAsync(imageMemoryStream);
            var imageBytes = imageMemoryStream.ToArray();
            
            _programLogger.LogInformation("The image was received");
            await using (var imageStream = File.Create(Path.Combine(receivedFilesDirPath, "ImageFromBytes.jpeg")))
            {
                await imageStream.WriteAsync(imageBytes);
                _programLogger.LogInformation("The image was saved");
            }

            await _fileClient.PostImageFileAsync(imageBytes);
            _programLogger.LogInformation("The image has been sent");

            Directory.Delete(Path.GetFullPath(tmpFolderName), recursive: true);
        }
    }
}
