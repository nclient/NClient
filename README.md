# NClient
![Nuget](https://img.shields.io/nuget/v/NClient)
![GitHub Release Date](https://img.shields.io/github/release-date/nclient/nclient)
![GitHub last commit](https://img.shields.io/github/last-commit/nclient/nclient)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/nclient/nclient/Test)

NClient is an HTTP client that allows you to call web service API methods through annotated controllers or interfaces. The client supports asynchronous calls, retry policies and logging. All this is  simple and flexible to configure.

## Usage with ASP.NET Core
#### Step 1: Create controller
```C#
[ApiController, Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public async Task<WeatherForecast> GetAsync(DateTime date) =>
        new WeatherForecast(date: date, temperatureC: -25);
}
```
#### Step 2: Extract interface for your controller and add `INClient` interface
```C#
public interface IWeatherForecastClient : INClient
{
    Task<WeatherForecast> GetAsync(DateTime date);
}

[ApiController, Route("[controller]")]
public class WeatherForecastController : ControllerBase, IWeatherForecastClient { ... }
```
#### Step 3: Create client
```C#
var client = new AspNetClientProvider()
    .Use<IWeatherForecastClient, WeatherForecastController>(host: new Uri("http://localhost:8080"))
    .SetDefaultHttpClientProvider()
    .WithoutResiliencePolicy()
    .Build();
```
#### Step 4: Send an http request
```C#
// GET http://localhost:8080/WeatherForecast?date=2021-03-13T00:15Z
var forecast = await client.GetAsync(DateTime.Now);
Console.WriteLine($"Date {forecast.Date}: {forecast.TemperatureC}°C");
```
## Usage with non .Net web service
#### Step 1: Create interface for your service and add `INClient` interface
```C#
[Api(template: "api")]
public interface IProductServiceClient : INClient
{
    [AsHttpPost(template: "products")]
    Task PostAsync(Product product);
}
```
#### Step 2: Create client
```C#
var client = new ClientProvider()
    .Use<IProductServiceClient>(host: new Uri("http://localhost:8080"))
    .SetDefaultHttpClientProvider()
    .WithoutResiliencePolicy()
    .Build();
```
#### Step 3: Send an http request
```C#
// POST http://localhost:8080/api/products with json body: { id: 1 }
await client.PostAsync(new Product(id: 1));
```

## NuGet Packages
| Package name                                                                                                     | Description                                              |
| ---------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------- |
| [NClient](https://www.nuget.org/packages/NClient)                                                                | Provides a **complete set** of tools for creating http clients. Including third-party solutions as follows: [RestSharp](https://github.com/restsharp/RestSharp) and [Polly](https://github.com/App-vNext/Polly) |
| [	NClient.Standalone](https://www.nuget.org/packages/NClient.Standalone)                                         | Provides the entire set of tools for creating clients but **no third-party** solutions. You can choose an http client and a tool for retry policy yourself. |
| [NClient.AspNetProxy](https://www.nuget.org/packages/NClient.AspNetProxy)                                        | Provides a set of tools for creating **controller-based** clients. Including third-party solutions as follows: [RestSharp](https://github.com/restsharp/RestSharp) and [Polly](https://github.com/App-vNext/Polly) |
| [NClient.AspNetProxy.Standalone](https://www.nuget.org/packages/NClient.AspNetProxy.Standalone)                  | Provides a set of tools for creating **controller-based** clients but **no third-party** solutions. You can choose an http client and a tool for retry policy yourself.|
| [NClient.InterfaceProxy](https://www.nuget.org/packages/NClient.InterfaceProxy)                                  | Provides a set of tools for creating **interface-based** clients. Including third-party solutions as follows: [RestSharp](https://github.com/restsharp/RestSharp) and [Polly](https://github.com/App-vNext/Polly)  |
| [NClient.InterfaceProxy.Standalone](https://www.nuget.org/packages/NClient.InterfaceProxy.Standalone)            | Provides a set of tools for creating **interface-based** clients but **no third-party** solutions. You can choose an http client and a tool for retry policy yourself.|
| [NClient.Extensions.DependencyInjection](https://www.nuget.org/packages/NClient.Extensions.DependencyInjection)  | Provides an **extensions** to extend ASP.NET Core with interface-based clients. |
