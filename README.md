# NClient
![Nuget](https://img.shields.io/nuget/v/NClient)
![GitHub Release Date](https://img.shields.io/github/release-date/nclient/nclient)
![GitHub last commit](https://img.shields.io/github/last-commit/nclient/nclient)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/nclient/nclient/Test)

NClient is an HTTP client that allows you to call web service API methods through annotated controllers or interfaces. The client supports asynchronous calls, retry policies and logging. All this is  simple and flexible to configure.

## Why use NClient?
Creating clients for web services can be quite a challenge because, in addition to data transfer, you need to implement query building, serialization, retry policy, error handling, logging — and this is not to mention the maintenance that comes with each update of your APIs. What if you could create clients with a fraction of the effort? This is exactly what NClient hopes to achieve by allowing you to create clients declaratively.

## How do I use NClient?
To generate a client, you just need to create an interface describing available endpoints and input/output data. After that, you can generate and configure the client, using the `ClientProvider`.
### Usage with ASP.NET Core
If you want to generate a client for a ASP.NET web service, then you don't even have to add attributes to the service interface. Everything you need:
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
IWeatherForecastClient client = new AspNetClientProvider()
    .Use<IWeatherForecastClient, WeatherForecastController>(host: new Uri("http://localhost:8080"))
    .SetDefaultHttpClientProvider()
    .WithoutResiliencePolicy()
    .Build();
```
#### Step 4: Send an http request
```C#
// Equivalent to the following request: 
// curl -X GET -H "Content-type: application/json" http://localhost:8080/WeatherForecast?date=2021-03-13T00:15Z
var forecast = await client.GetAsync(DateTime.Now);
Console.WriteLine($"Date {forecast.Date}: {forecast.TemperatureC}°C");
```
### Usage with non .Net web service
If you do not have the source code of the ASP.NET web service or you want to send requests to the non .Net service, then you need to create an interface and additionally declare it with attributes from `NClient.InterfaceProxy.Attributes`. They are very similar to attributes for ASP.NET controllers. Follow the steps below:
#### Step 1: Create interface for your service and add `INClient` interface
```C#
[Api(template: "api")]
public interface IProductServiceClient : INClient
{
    [AsHttpPost(template: "products")]
    Task PostAsync([ToBody] Product product);
}
```
#### Step 2: Create client
```C#
IProductServiceClient client = new ClientProvider()
    .Use<IProductServiceClient>(host: new Uri("http://localhost:8080"))
    .SetDefaultHttpClientProvider()
    .WithoutResiliencePolicy()
    .Build();
```
#### Step 3: Send an http request
```C#
// Equivalent to the following request: 
// curl -X POST -H "Content-type: application/json" --data "{ id: 1 }" http://localhost:8080/api/products
await client.PostAsync(new Product(id: 1));
```

## NuGet Packages
| Package name                                                                                                     | Description                                              |
| ---------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------- |
| [NClient](https://www.nuget.org/packages/NClient)                                                                | Provides a **complete set** of tools for creating http clients including third-party solutions: [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json), [RestSharp](https://github.com/restsharp/RestSharp) and [Polly](https://github.com/App-vNext/Polly) |
| [	NClient.Standalone](https://www.nuget.org/packages/NClient.Standalone)                                         | Provides the entire set of tools for creating clients but **no third-party** solutions. You can choose your own an http client and a retry policy tool. |
| [NClient.AspNetProxy](https://www.nuget.org/packages/NClient.AspNetProxy)                                        | Provides a set of tools for creating **controller-based** clients including third-party solutions: [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json), [RestSharp](https://github.com/restsharp/RestSharp) and [Polly](https://github.com/App-vNext/Polly) |
| [NClient.AspNetProxy.Standalone](https://www.nuget.org/packages/NClient.AspNetProxy.Standalone)                  | Provides a set of tools for creating **controller-based** clients but **no third-party** solutions. You can choose your own an http client and a retry policy tool. |
| [NClient.InterfaceProxy](https://www.nuget.org/packages/NClient.InterfaceProxy)                                  | Provides a set of tools for creating **interface-based** clients. Including third-party solutions as follows: [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json), [RestSharp](https://github.com/restsharp/RestSharp) and [Polly](https://github.com/App-vNext/Polly)  |
| [NClient.InterfaceProxy.Standalone](https://www.nuget.org/packages/NClient.InterfaceProxy.Standalone)            | Provides a set of tools for creating **interface-based** clients but **no third-party** solutions. You can choose your own an http client and a retry policy tool. |
| [NClient.Extensions.DependencyInjection](https://www.nuget.org/packages/NClient.Extensions.DependencyInjection)  | Provides an **extensions** for registration interface-based clients in ASP.NET app. |
