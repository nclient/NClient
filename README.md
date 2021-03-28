# <img src="logo.png" width="50" height="50" align="left" alt="logo">NClient

![Nuget](https://img.shields.io/nuget/v/NClient)
![GitHub last commit](https://img.shields.io/github/last-commit/nclient/nclient)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/nclient/nclient/Test)

NClient is an HTTP client that allows you to call web service API methods usnig annotated interfaces or controllers. The client supports asynchronous calls, retry policies and logging. All this is  simple and flexible to configure.

## Why use NClient?
Creating clients for web services can be quite a challenge because, in addition to data transfer, you need to implement query building, serialization, retry policy, error handling, logging — and this is not to mention the maintenance that comes with each update of your APIs. What if you could create clients with a fraction of the effort? This is exactly what NClient hopes to achieve by allowing you to create clients declaratively.

## How to install?
The easiest way is to install [NClient package](https://www.nuget.org/packages?q=Tags%3A"NClient") using Nuget. How to choose which package you need, see below in "NuGet packages" section. If you do not know how to install NuGet package you will need links:  
- [Install and use a package in Visual Studio](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio)  
- [Install and manage packages with the Package Manager Console in Visual Studio](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-powershell)

## How to use?
To generate a client, you just need to create an interface describing available endpoints and input/output data of a service. After that, you can generate and configure the client, using the `NClientProvider`.
### Usage with ASP.NET Core
If you want to generate a client for a ASP.NET web service, then you need to extract an interface for your controller and annotate it with attributes from `NClient.Annotations`. They are very similar to attributes for ASP.NET controllers. Follow the steps below:
#### Step 1: Install `NClient.AspNetCore` on server-side
```
dotnet add package NClient.AspNetCore
```
#### Step 2: Create controller
```C#
public class WeatherForecastController : ControllerBase
{
    public async Task<WeatherForecast> GetAsync(DateTime date) =>
        new WeatherForecast(date: date, temperatureC: -25);
}
```
Note that you don't need to annotate it with ASP.NET attributes.
#### Step 3: Extract interface for your controller
```C#
[Path("[controller]")]                                            // equivalent to [ApiController, Route("[controller]")]
public interface IWeatherForecastController
{
    [GetMethod]                                                   // equivalent to [HttpGet]
    Task<WeatherForecast> GetAsync([QueryParam] DateTime date);   // equivalent to [FromQuery]
}

public class WeatherForecastController : ControllerBase, IWeatherForecastController { ... }
```
The annotation in the interface instead of the controller allows you to put the interface in a separate assembly. 
Therefore, the client that will use this interface will not depend on the service.
#### Step 4 (optional): Create interface for client
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
This should be done if you want your client type to not contain "Сontroller" in the name. If you add `INClient` interface, you will get additional NClient features: receive a full http response and change a resilience policy for requests.
#### Step 6: Add controller to ServiceCollection in Startup.cs
```C#
public void ConfigureServices(IServiceCollection services)
{
    // ...
    services.AddNClientControllers();
}
```
`AddNClientControllers` method can be used in combination with `AddControllers`.
#### Step 7: Install `NClient` on client-side
```
dotnet add package NClient
```
#### Step 8: Create client
```C#
IWeatherForecastController client = NClientProvider
    .Use<IWeatherForecastController>(host: "http://localhost:8080")
    .Build();
```
If you decide to follow the 4 step, use `IWeatherForecastClient` interface instead of `IWeatherForecastController`.
#### Step 9: Send an http request
```C#
// Equivalent to the following request: 
// curl -X GET -H "Content-type: application/json" http://localhost:8080/WeatherForecast?date=2021-03-13T00:15Z
var forecast = await client.GetAsync(DateTime.Now);
Console.WriteLine($"Date {forecast.Date}: {forecast.TemperatureC}°C");
```
### Usage with non .Net web service
If you do not have the source code of the ASP.NET web service or you want to send requests to the non .Net service, then you just need to create an interface that describes the service you want to make requests to. Follow the steps below:
#### Step 1: Install `NClient` on client-side
```
dotnet add package NClient
```
#### Step 2: Create interface for your service
```C#
[Path("api")]
public interface IProductServiceClient : INClient
{
    [PostMethod("products")]
    Task PostAsync([BodyParam] Product product);
}
```
#### Step 3: Create client
```C#
IProductServiceClient client = NClientProvider
    .Use<IProductServiceClient>(host: "http://localhost:8080")
    .Build();
```
#### Step 4: Send an http request
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
| [NClient](https://www.nuget.org/packages/NClient) | Tools for creating clients from interfaces and controllers including third-party | Castle, Json.Net, RestSharp, Polly |
| [NClient.Standalone](https://www.nuget.org/packages/NClient.Standalone) | The same as NClient package, but without third-party | Castle |
| [NClient.AspNetCore](https://www.nuget.org/packages/NClient.AspNetCore) | Allows you to annotate controllers via interfaces | Castle, ASP.NET |
| [NClient.Extensions.DependencyInjection](https://www.nuget.org/packages/NClient.Extensions.DependencyInjection) | Extension methods for registration of clients in ServiceCollection | Castle, DependencyInjection, Json.Net, RestSharp, Polly |
| [NClient.Abstractions](https://www.nuget.org/packages/NClient.Abstractions) | Abstractions for clients and providers | Polly |
| [NClient.Annotations](https://www.nuget.org/packages/NClient.Annotations) | Attributes for annotation of client interfaces and controllers | Polly |
| [NClient.Providers.Resilience](https://www.nuget.org/packages/NClient.Providers.Resilience) | Polly based resilience policy provider | Polly |
| [NClient.Providers.HttpClient.RestSharp](https://www.nuget.org/packages/NClient.Providers.HttpClient.RestSharp) | RestSharp based HTTP client provider | Json.Net, RestSharp |
