# <img src="logo.png" width="50" height="50" align="left" alt="logo">NClient: automatic type-safe .Net HTTP client

![Nuget](https://img.shields.io/nuget/v/NClient)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/NClient?label=nuget-pre)
![Nuget](https://img.shields.io/nuget/dt/NClient)
![GitHub last commit](https://img.shields.io/github/last-commit/nclient/NClient)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/nclient/NClient/Test)
![GitHub](https://img.shields.io/github/license/nclient/NClient)

NClient is an automatic type-safe .Net HTTP client that allows you to call web service API methods using annotated interfaces or controllers. The client supports asynchronous calls, HTTP contexts, retry policies, and logging. The main difference between NClient and its analogues is that NClient allows you to annotate ASP.NET controllers via interfaces and then use these interfaces to create clients. Annotated interfaces allow you to get rid of unwanted dependencies on a client side and to reuse an API description in clients without boilerplate code.

## Table of Contents
- [Why use NClient?](#why)  
- [How to install?](#install)  
- [How to use?](#usage)  
  - [Usage with ASP.NET Core](#usage-aspnet)  
  - [Usage with non ASP.NET web service](#usage-non-aspnet) 
- [Features](#features)  
  - [Creating](#features-creating)
  - [Annotation](#features-annotation)
  - [Routing](#features-routing)
  - [Asynchronously calls](#features-async)
  - [HTTP response](#features-response)
  - [HTTP response status code](#features-status-code)
  - [Resilience](#features-resilience)
  - [Logging](#features-logging)
  - [Dependency injection](#features-di)
  - [HttpClient](#features-httpclient)
- [Documentation](#documentation)  
- [NuGet Packages](#nuget)  

<a name="why" />  

## Why use NClient?
Creating clients for web services can be quite a challenge because, in addition to data transfer, you need to implement query building, serialization, retry policy, error handling, logging — and this is not to mention the maintenance that comes with each update of your APIs. What if you could create clients with a fraction of the effort? This is exactly what NClient hopes to achieve by allowing you to create clients declaratively.

<a name="install" />  

## How to install?
The easiest way is to install [NClient package](https://www.nuget.org/packages?q=Tags%3A"NClient") using Nuget. To choose which package you need, see below in [NuGet Packages](#nuget) section.

<a name="usage" />  

## How to use?
To generate a client, you just need to create an interface describing available endpoints and input/output data of a service. After that, you can generate and configure the client, using the `NClientProvider`.

<a name="usage-aspnet" />

### Usage with ASP.NET Core
If you want to generate a client for a ASP.NET web service, you need to extract an interface for your controller and annotate it with attributes from `NClient.Annotations`. They are very similar to attributes for ASP.NET controllers. Follow the steps below:
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
[Api, Path("[controller]")]                                       // equivalent to [ApiController, Route("[controller]")]
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

[Api, Path("[controller]")]
public interface IWeatherForecastController
{
    [GetMethod]
    Task<WeatherForecast> GetAsync([QueryParam] DateTime date);
}

public class WeatherForecastController : ControllerBase, IWeatherForecastController { ... }
```
This should be done if you want your client type not to contain "Сontroller" in the name. If you add `INClient` interface, you will get additional NClient features: receive a full http response and change a resilience policy for requests.
#### Step 5: Add controller to ServiceCollection in Startup.cs
```C#
public void ConfigureServices(IServiceCollection services)
{
    // ...
    services.AddNClientControllers();
}
```
`AddNClientControllers` method can be used in combination with `AddControllers`.
#### Step 6: Install `NClient` on client-side
```
dotnet add package NClient
```
#### Step 7: Create client
```C#
IWeatherForecastController client = NClientProvider
    .Use<IWeatherForecastController>(host: "http://localhost:8080")
    .Build();
```
If you decide to follow the 4th step, use `IWeatherForecastClient` interface instead of `IWeatherForecastController`.
#### Step 8: Send an http request
```C#
// Equivalent to the following request: 
// curl -X GET -H "Content-type: application/json" http://localhost:8080/WeatherForecast?date=2021-03-13T00:15Z
var forecast = await client.GetAsync(DateTime.Now);
Console.WriteLine($"Date {forecast.Date}: {forecast.TemperatureC}°C");
```

<a name="usage-non-aspnet" />

### Usage with non ASP.NET web service
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

<a name="features"/>  

# Features
The list of the main features of NClient library:

<a name="features-creating"/>  

## Creating
There are several ways to create a client, you can choose the most suitable one.

#### NClientProvider
Static option for creating a client instance:
```C#
IMyClient myClient = NClientProvider
    .Use<IMyClient>(host: "http://localhost:8080")
    .Build();
```
#### NClientBuilder
Option with creating a builder instance. `NClientBuilder` implements `INClientBuilder` interface, so it is suitable for dependency injection.
```C#
IMyClient myClient = new NClientBuilder()
    .Use<TInterface, TController>(host: "http://localhost:8080")
    .Build();
```
#### NClientFactory
The factory will be convenient for creating client instances with the same settings.
```C#
IHttpClientProvider httpClientProvider = ...;
IResiliencePolicyProvider resiliencePolicyProvider = ...;
ILoggerFactory loggerFactory = ...;

var clientFactory = new NClientFactory(httpClientProvider, resiliencePolicyProvider, loggerFactory);
IMyClient myClient = clientFactory.Create<IMyClient>(host: "http://localhost:8080");
```

For fluent creation of a factory, you can use the `NClientFactoryBuilder`:
```C#
var clientFactory = new NClientFactoryBuilder()
    .WithCustomHttpClient(httpClientProvider)
    .WithResiliencePolicy(resiliencePolicyProvider)
    .WithLogging(loggerFactory)
    .Build()
```

<a name="features-annotation"/>  

## Annotation
The client and controller interfaces are annotated with attributes. The following attributes can be used:

#### Base attributes
A client interface can be annotated with `Facade` attribute if no other NClient attributes are used in the client interface. It is needed in the internal logic of the library to find the client interfaces.
```C#
[Facade] public interface IMyClient { ... }
```
`Api` attribute is an equivalent of `ApiControllerAttribute` from ASP.NET Core.
```C#
[Api] public interface IMyController { ... }
```
The base URL route for API can be set by `PathAttribute`.
```C#
[Path("api")] public interface IMyClient { ... }
```
#### Method attributes
Each method must have an HTTP attribute that defines the request method. There are four types of such attributes: `GetMethodAttribute`, `PostMethodAttribute`, `PutMethodAttribute`, `DeleteMethodAttribute`.
```C#
public interface IMyClient { [GetMethod] Entity[] Get(); }
```
Optionally, you can specify a relative path.
```C#
public interface IMyClient { [GetMethod("entities")] Entity[] Get(); }
```
#### Parameter attributes
By default, parameters that are custom objects are passed in the request body and primitive parameters are passed in a URL query. You can explicitly specify how to pass a parameter using attributes: `QueryParamAttribute`, `BodyParamAttribute`, `HeaderParamAttribute`, `RouteParamAttribute`.
```C#
public interface IMyClient { [PostMethod] void Post([QueryParam] Entity entity); }
```
It is also possible to change a parameter name:
```C#
public interface IMyClient { [PostMethod] void Post([QueryParam(Name = "myEntity")] Entity entity); }
```
#### Property attributes
`QueryParamAttribute` allows you to change a property name of a custom object that is passed in URL query.
```C#
public class Entity { [QueryParam(Name = "id")] public int Id }
```
The same effect will occur if you use the `FromQueryAttribute` from ASP.NET Core.

The names of the properties of custom objects that are passed in the request body can be changed using `JsonPropertyNameAttribute`.
```C#
public class Entity { [JsonPropertyName("id")] public int Id }
```
#### Static headers
Use `HeaderAttribute` attribute to add a static header. Static headers can be added for all methods:
```C#
[Header(Name: "Common-Header", Value: "value")] public interface IMyClient { ... }
```
or for a specific method:
```C#
public interface IMyClient { [GetMethod, Header("Specific-Method-Header", Value: "value")] Entity[] Get(); }
```
#### Response type
`ResponseAttribute` specifies the type of the value and status code returned by the method. This is the equivalent of [ProducesResponseTypeAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.producesresponsetypeattribute).
```C#
public interface IMyClient { [GetMethod, Response(typeof(Entity[]), HttpStatusCode.OK)] Entity[] Get(); }
```
This attribute is optional.
#### Authorization
In ASP.NET there are `AuthorizeAttribute` and `AllowAnonymousAttribute` attributes. Their equivalents are `AuthorizedAttribute` and `AnonymousAttribute`.


<a name="features-routing"/> 

## Routing
Routes are set using route templates that are passed to attributes. Route templates can contain the following tokens:

#### Class and interface names
Relative routes can be set in the following attributes: `PathAttribute`, `QueryParamAttribute`, `BodyParamAttribute`, `HeaderParamAttribute`, `RouteParamAttribute`. You can use names of classes and interfaces in the paths:
```C#
[Path("api/[controller]")] public interface IEntitiesClient { ... } // the route will be: api/entities
```
The name of the class/interface will be substituted without prefixes (`I`) and postfixes (`Controller`, `Client`, `Facade`).
#### Method parameters
In routes you can use values of a method parameters:
```C#
public interface IMyClient { [GetMethod("entities/{id}")] Entity[] Get(int id); }
```
You can also use property values of custom objects in route templates:
```C#
public interface IMyClient { [PutMethod("entities/{entity.Id}")] void Put(Entity entity); }
```

<a name="features-async"/> 

## Asynchronously calls
To execute a request to the web-service asynchronously, you should define the returned type as `Task` or `Task<>`:
```C#
public interface IMyClient : INClient
{
    [GetMethod]
    Entity Get(int id);             // sync call
    [GetMethod]
    Task<Entity> GetAsync(int id);  // async call
    [PostMethod]
    void Post(Entity entity);       // sync call
    [PostMethod]
    Task PostAsync(Entity entity);  // async call
}
```

<a name="features-response"/> 

## HTTP response
You can get the full HTTP response, not just the body. To do this, the client interface must inherit `INClient` interface.
```C#
public interface IMyClient : INClient
{
    [GetMethod]
    Task<Entity> GetAsync(int id);
}
...
HttpResponse<Entity> response = await myClient.AsHttp().GetHttpResponse(x => x.GetAsync(id: 1));
```
If your interface is used only as a client and you want to always get an HTTP response, just make the return type `HttpResponse`:
```C#
public interface IMyClient
{
    [GetMethod]
    Task<HttpResponse<Entity>> GetAsync(int id);
    [PostMethod]
    Task<HttpResponse> PostAsync(Entity entity);
}
```

<a name="features-status-code"/> 

## HTTP response status code
It is not always convenient to use with `IActionResult` in NClient controllers, so you can use `HttpResponseException` to return an error object and HTTP status code.
To use these exceptions you need to add NClient controllers in ASP.NET startup as follows:
```C#
public void ConfigureServices(IServiceCollection services)
{
    // ...
    services.AddNClientControllers().WithResponseExceptions();
}
```
After that, you can use exceptions in methods of your NClient controllers:
```C#
public Entity[] Get()
{
    // ...
    throw new HttpResponseException(HttpStatusCode.BadRequest, new { Error = "Error message." });
}
```
For information on how to get HTTP status code, see section [Http response](#features-response).

<a name="features-resilience"/> 

## Resilience
To achieve better resilience, you can create a resilience policy using Polly library:
```C#
var policy = Policy
    .HandleResult<HttpResponse>(x => !x.IsSuccessful)
    .Or<Exception>()
    .WaitAndRetryAsync(maxRetryAttempts: 3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

IMyClient myClient = NClientProvider
    .Use<IMyClient>(host: "http://localhost:8080")
    .WithResiliencePolicy(policy)
    .Build();
```
You can also create your own implementation of `IResiliencePolicyProvider` and pass it to `WithResiliencePolicy` method.  
To set specific resilience policy for a method, the client interface must inherit `INClient` interface:
```C#
public class MyResiliencePolicyProvider : IResiliencePolicyProvider { ... }
...
await myClient.AsResilience().InvokeResiliently(x => x.PostAsync(id), new MyResiliencePolicyProvider());
```

<a name="features-logging"/> 

## Logging
You can optionally set a logger for a client:
```C#
ILogger<IMyClient> logger = ...;

IMyClient client = NClientProvider
    .Use<IMyClient>(host: "http://localhost:8080")
    .WithLogging(logger)
    .Build();
```

<a name="features-di"/> 

## Dependency injection
`NClient.Extensions.DependencyInjection` package contains `AddNClient` extension methods:
```C#
var serviceProvider = new ServiceCollection()
    .AddHttpClient()
    .AddLogging()
    .AddNClient<IMyClient>(host: "http://localhost:8080")
    .BuildServiceProvider();
```
and `AddNClientFactory`:
```C#
var serviceProvider = new ServiceCollection()
    .AddHttpClient()
    .AddLogging()
    .AddNClientFactory()
    .BuildServiceProvider();
```

<a name="features-httpclient"/> 

## HttpClient
An `HttpClient` is created for each instance of a client. Keep this in mind, because `HttpClient` has problems. Create an instance for every request and you will run into socket exhaustion. Make it a singleton and it will not respect DNS changes. The best way would be to use `IHttpClientFactory`. You can create it yourself and pass it to the builder:
```C#
var httpClientFactory = ...;

IMyClient myClient = NClientProvider
    .Use<IMyClient>(host, httpClientFactory)
    .WithLogging(logger)
    .Build();
```
or just use DI extensions form `NClient.Extensions.DependencyInjection`:
```C#
var serviceProvider = new ServiceCollection()
    .AddHttpClient()
    .AddNClient<IMyClient>(host: "http://localhost:8080")
    .BuildServiceProvider();
```
For more fine-tuning, you can use a named `HttpClient`:
```C#
IMyClient myClient = NClientProvider
    .Use<IMyClient>(host, httpClientFactory, httpClientName: nameof(IMyClient))
    .WithLogging(logger)
    .Build();
```

<a name="documentation" />  

## Documentation
You can find NClient documentation and samples [on the website](https://nclient.github.io/).

<a name="nuget" />  

## NuGet Packages
| Package name                                             | Description                                            | Dependencies                                           |
| :------------------------------------------------------- | :----------------------------------------------------- |:------------------------------------------------------ |
| [NClient](https://www.nuget.org/packages/NClient) | Tools for creating clients from interfaces and controllers including third-party | Castle, System.Text.Json, System.Net.Http, Polly |
| [NClient.Standalone](https://www.nuget.org/packages/NClient.Standalone) | The same as NClient package, but without third-party | Castle |
| [NClient.AspNetCore](https://www.nuget.org/packages/NClient.AspNetCore) | Allows you to annotate controllers via interfaces | Castle, Microsoft.AspNetCore.Mvc.* (Core, ApiExplorer, Cors, DataAnnotations) |
| [NClient.Extensions.DependencyInjection](https://www.nuget.org/packages/NClient.Extensions.DependencyInjection) | Extension methods for registration of clients in ServiceCollection | Castle, Microsoft.Extensions.DependencyInjection, System.Text.Json, System.Net.Http, Polly |
| [NClient.Abstractions](https://www.nuget.org/packages/NClient.Abstractions) | Abstractions for clients and providers | - |
| [NClient.Annotations](https://www.nuget.org/packages/NClient.Annotations) | Attributes for annotation of client interfaces and controllers | - |
| [NClient.Providers.Resilience](https://www.nuget.org/packages/NClient.Providers.Resilience) | Polly based resilience policy provider | Polly |
| [NClient.Providers.HttpClient.System](https://www.nuget.org/packages/NClient.Providers.HttpClient.System) | System.Net.Http.HttpClient based HTTP client provider | System.Text.Json, System.Net.Http |
| [NClient.Providers.HttpClient.RestSharp](https://www.nuget.org/packages/NClient.Providers.HttpClient.RestSharp) | RestSharp based HTTP client provider | System.Text.Json, RestSharp |
