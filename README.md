# <img src="logo.png" width="50" height="50" align="left" alt="logo">NClient

![Nuget](https://img.shields.io/nuget/v/NClient)
![GitHub last commit](https://img.shields.io/github/last-commit/nclient/nclient)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/nclient/nclient/Test)

NClient is an HTTP client that allows you to call web service API methods usnig annotated controllers or interfaces. The client supports asynchronous calls, retry policies and logging. All this is  simple and flexible to configure.

## Why use NClient?
Creating clients for web services can be quite a challenge because, in addition to data transfer, you need to implement query building, serialization, retry policy, error handling, logging — and this is not to mention the maintenance that comes with each update of your APIs. What if you could create clients with a fraction of the effort? This is exactly what NClient hopes to achieve by allowing you to create clients declaratively.

## How to install?
The easiest way is to install [NClient package](https://www.nuget.org/packages?q=Tags%3A"NClient") using Nuget. How to choose which package you need, see below in "NuGet packages" section. If you do not know how to install NuGet package you will need links:  
- [Install and use a package in Visual Studio](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio)  
- [Install and manage packages with the Package Manager Console in Visual Studio](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-powershell)

## How to use?
To generate a client, you just need to create an interface describing available endpoints and input/output data. After that, you can generate and configure the client, using the `ClientProvider`.
### Usage with ASP.NET Core
If you want to generate a client for a ASP.NET web service, then you need to extract an interface for your controller and annotate it with attributes from `NClient.Core.Attributes`. They are very similar to attributes for ASP.NET controllers. Follow the steps below:
#### Step 1: Create controller
```C#
public class WeatherForecastController : ControllerBase
{
    public async Task<WeatherForecast> GetAsync(DateTime date) =>
        new WeatherForecast(date: date, temperatureC: -25);
}
```
Note that you don't need to annotate it with ASP.NET attributes.
#### Step 2: Extract interface for your controller and inherit `INClient` interface
```C#
[Path("[controller]")] // equivalent to [ApiController, Route("[controller]")]
public interface IWeatherForecastController : INClient
{
    [GetMethod] // equivalent to [HttpGet]
    Task<WeatherForecast> GetAsync([QueryParam] DateTime date); // equivalent to [FromQuery]
}

public class WeatherForecastController : ControllerBase, IWeatherForecastController { ... }
```
The annotation in the interface instead of the controller allows you to put the interface in a separate assembly. Therefore, the client that will use this interface will not depend on the service.
#### Step 3 (optional): Create interface for client
```C#
public interface IWeatherForecastClient : IWeatherForecastController, INClient
{
}

[Path("[controller]")]
public interface IWeatherForecastController
{
    [GetMethod]
    Task<WeatherForecast> GetAsync([QueryParam] DateTime date);
}

public class WeatherForecastController : ControllerBase, IWeatherForecastController { ... }
```
This should be done if you want your client type to not contain "Сontroller" in the name.
#### Step 4: Create client
```C#
IWeatherForecastController client = new ClientProvider()
    .Use<IWeatherForecastController>(host: new Uri("http://localhost:8080"))
    .SetDefaultHttpClientProvider()
    .WithoutResiliencePolicy()
    .Build();
```
If you decide to follow the 3 step, use `IWeatherForecastClient` interface instead of `IWeatherForecastController`.
#### Step 5: Send an http request
```C#
// Equivalent to the following request: 
// curl -X GET -H "Content-type: application/json" http://localhost:8080/WeatherForecast?date=2021-03-13T00:15Z
var forecast = await client.GetAsync(DateTime.Now);
Console.WriteLine($"Date {forecast.Date}: {forecast.TemperatureC}°C");
```
### Usage with non .Net web service
If you do not have the source code of the ASP.NET web service or you want to send requests to the non .Net service, then you just need to create an interface that describes the service you want to make requests to. Follow the steps below:
#### Step 1: Create interface for your service and inherit `INClient` interface
```C#
[Path("api")]
public interface IProductServiceClient : INClient
{
    [PostMethod("products")]
    Task PostAsync([BodyParam] Product product);
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

## Documentation
You can find NClient documentation [on the website](https://nclient.github.io/).

## NuGet Packages
| Package name                                             | Description                                            | Dependencies                                           |
| :------------------------------------------------------- | :----------------------------------------------------- |:------------------------------------------------------ |
| [NClient](https://www.nuget.org/packages/NClient) | **Complete set** of tools **including third-party** | Castle, ASP.NET, Json.Net, RestSharp, Polly |
| [	NClient.Standalone](https://www.nuget.org/packages/NClient.Standalone) | **Complete set** of tools **without third-party**  | Castle, ASP.NET |
| [NClient.AspNetProxy](https://www.nuget.org/packages/NClient.AspNetProxy) | Tools for **controller-based** clients **including third-party** | Castle, ASP.NET, Json.Net, RestSharp, Polly |
| [NClient.AspNetProxy.Standalone](https://www.nuget.org/packages/NClient.AspNetProxy.Standalone) | Tools for **controller-based** clients **without third-party** | Castle, ASP.NET |
| [NClient.InterfaceProxy](https://www.nuget.org/packages/NClient.InterfaceProxy) | Tools for **interface-based** clients **including third-party** | Castle, Json.Net, RestSharp, Polly |
| [NClient.InterfaceProxy.Standalone](https://www.nuget.org/packages/NClient.InterfaceProxy.Standalone) | Tools for **interface-based** clients **without third-party** | Castle |
| [NClient.Extensions.DependencyInjection](https://www.nuget.org/packages/NClient.Extensions.DependencyInjection) | **Extensions** for registration interface-based clients in DI container | Castle, DependencyInjection, Json.Net, RestSharp, Polly |
