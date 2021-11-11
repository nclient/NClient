# <img src="logo.png" width="50" height="50" align="left" alt="logo">NClient: automatic type-safe .NET HTTP client

![Nuget](https://img.shields.io/nuget/v/NClient)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/NClient?label=nuget-pre)
![Nuget](https://img.shields.io/nuget/dt/NClient)
![GitHub last commit](https://img.shields.io/github/last-commit/nclient/NClient)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/nclient/NClient/Test:%20Full)
![GitHub](https://img.shields.io/github/license/nclient/NClient)

NClient is an automatic type-safe .NET HTTP client that allows you to call web service API methods using annotated interfaces or controllers. The client supports asynchronous calls, HTTP contexts, retry policies, and logging. The main difference between NClient and its analogues is that NClient allows you to annotate ASP.NET controllers via interfaces and then use these interfaces to create clients. Annotated interfaces allow you to get rid of unwanted dependencies on a client side and to reuse an API description in clients without boilerplate code.

Do you like it? Give us a star! ⭐

## Table of Contents
- [Why use NClient?](#why)  
- [How to install?](#install) 
- [Requirements](#requirements) 
- [How to use?](#usage)  
  - [Usage with ASP.NET Core](#usage-aspnet)  
  - [Usage with non ASP.NET web service](#usage-non-aspnet) 
  - [Samples of applications](#sample-applications)
- [Contributing](#contributing)
- [Features](#features)  
  - [Creating](#features-creating)
  - [Annotation](#features-annotation)
  - [Routing](#features-routing)
  - [Asynchronously calls](#features-async)
  - [HTTP response](#features-response)
  - [HTTP response status code](#features-status-code)
  - [Errors](#features-errors)
  - [HttpClient](#features-httpclient)
  - [Serialization](#features-serialization)
  - [Resilience](#features-resilience)
  - [Logging](#features-logging)
  - [Dependency injection](#features-di)
  - [Handling](#features-handling)
  - [File upload/download](#features-files)
  - [System.Net.Http](#features-system-httpclient)
- [Providers](#providers) 
  - [RestSharp](#providers-restsharp)
  - [Newtonsoft.Json](#providers-newtonsoft)
- [Documentation](#documentation)  
- [NuGet Packages](#nuget)  

<a name="why" />  

## Why use NClient?
Creating clients for web services can be quite a challenge because, in addition to data transfer, you need to implement query building, serialization, retry policy, error handling, logging — and this is not to mention the maintenance that comes with each update of your APIs. What if you could create clients with a fraction of the effort? This is exactly what NClient hopes to achieve by allowing you to create clients declaratively.

By the way, you can [contribute](#contributing) to the NClient, not just use it :smiley:

<a name="install" />  

## How to install?
The easiest way is to install [NClient package](https://www.nuget.org/packages?q=Tags%3A"NClient") using Nuget. To choose which package you need, see below in [NuGet Packages](#nuget) section.

<a name="requirements" />

## Requirements
Use of the NClient library requires .NET Standard 2.0 or higher. The NClient controllers can be used with ASP.NET Core and .NET Core 3.1 target or higher.

<a name="usage" />  

## How to use?
To generate a client, you just need to create an interface describing available endpoints and input/output data of a service. After that, you can generate and configure the client, using the `NClientProvider`.

<a name="usage-aspnet" />

### Usage with ASP.NET Core
If you want to generate a client for a ASP.NET web service, you need to extract an interface for your controller and annotate it with attributes from the `NClient.Annotations`. They are very similar to attributes for ASP.NET controllers. Follow the steps below:
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
public interface IWeatherForecastClient : IWeatherForecastController, INClient { }

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
    ...
    services.AddNClientControllers();
}
```
The `AddNClientControllers` method can be used in combination with the `AddControllers`.
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
If you decide to follow the 4th step, use the `IWeatherForecastClient` interface instead of `IWeatherForecastController`.
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

<a name="sample-applications"/>  

### Samples of applications
See the sample applications in the [NClient.Samples](https://github.com/nclient/NClient.Samples) project.

<a name="contributing"/>  

## Contributing
You’re thinking about contributing to NClient? Great! We love to receive contributions from the community! The simplest contribution is to give this project a star ⭐.  
Helping with documentation, pull requests, issues, commentary or anything else is also very welcome. Please review our [contribution guide](CONTRIBUTING.md).  
It's worth getting in touch with us to discuss changes in case of any questions. We can also give advice on the easiest way to do things.

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
Option with creating a builder instance. The `NClientBuilder` class implements the `INClientBuilder` interface, so it is suitable for dependency injection.
```C#
IMyClient myClient = new NClientBuilder()
    .Use<TInterface>(host: "http://localhost:8080")
    .Build();
```
#### NClientFactory
The factory will be convenient for creating client instances with the same settings.
```C#
IResiliencePolicyProvider resiliencePolicyProvider = ...;
ILoggerFactory loggerFactory = ...;

var clientFactory = new NClientFactory(resiliencePolicyProvider, loggerFactory);
IMyClient myClient = clientFactory.Create<IMyClient>(host: "http://localhost:8080");
```

For fluent creation of a factory, you can use the `NClientFactoryBuilder`:
```C#
var clientFactory = new NClientFactoryBuilder()
    .WithResiliencePolicy(resiliencePolicyProvider)
    .WithLogging(loggerFactory)
    .Build()
```

<a name="features-annotation"/>  

## Annotation
The client and controller interfaces are annotated with attributes. The following attributes can be used:

#### Base attributes
A client interface can be annotated with the `FacadeAttribute` if no other NClient attributes are used in the client interface. It is needed in the internal logic of the library to find the client interfaces.
```C#
[Facade] public interface IMyClient { ... }
```
The `Api` attribute is an equivalent of the `ApiControllerAttribute` from ASP.NET Core.
```C#
[Api] public interface IMyController { ... }
```
The base URL route for API can be set by the `PathAttribute`.
```C#
[Path("api")] public interface IMyClient { ... }
```
#### Versioning attributes
There are two attributes for API versioning: `VersionAttribute` and `ToVersionAttribute`. They are the equivalents of `ApiVersionAttribute` and `MapToApiVersionAttribute` (see [ASP.NET API Versioning](https://github.com/microsoft/aspnet-api-versioning)).
```C#
[Version("1.0"), Version("2.0"), Path("api/v{version:apiVersion}")]
public interface IMyController 
{
    [GetMethod] Entity[] Get(int id);                      // Available in versions 1.0 and 2.0
    [DeleteMethod, ToVersion("2.0")] void Delete(int id);  // Available in version 2.0
}
```
You can add `UseVersionAttribute` to set the API version for a client:
```C#
[UseVersion("1.0"), Path("api/v{version:apiVersion}")]
public interface IMyClient
{ 
    [GetMethod] Entity[] Get(int id);                       // Uses version 1.0
    [DeleteMethod, UseVersion("2.0")] void Delete(int id);  // Uses version 2.0
}
```
The `UseVersionAttribute` can be used together with the attributes for API versioning, for example:
```C#
[UseVersion("1.0")] public interface IMyClient : IMyController { } 
```
#### Method attributes
Each method must have an HTTP attribute that defines the request method. There are four types of such attributes: `GetMethodAttribute`, `HeadMethodAttribute`, `PostMethodAttribute`, `PutMethodAttribute`, `PatchMethodAttribute`, `DeleteMethodAttribute`, `OptionsMethodAttribute`.
```C#
public interface IMyClient { [GetMethod] Entity[] Get(); }
```
Optionally, you can specify a relative path.
```C#
public interface IMyClient { [GetMethod("entities")] Entity[] Get(); }
```
#### Override attribute
The methods of the interface can be overridden in the inheriting interfaces. Although this is not an override in the usual sense, because you can change the return type of the method:
```C#
public interface IMyController { [GetMethod] Task<int> GetAsync([RouteParam] long id); }
public interface IMyClient : IMyController { [Override] new Task<string> GetFileAsync(long id); }
```
Overridden methods inherit all method and parameters attributes from the method of the inherited interface. If necessary, you can replace the attributes or add new ones. It is worth noting that multiple inheritance is forbidden if you want to override the method.
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

The names of the properties of custom objects that are passed in the request body can be changed using the `JsonPropertyNameAttribute`.
```C#
public class Entity { [JsonPropertyName("id")] public int Id }
```
#### Static headers
Use the `HeaderAttribute` attribute to add a static header. Static headers can be added for all methods:
```C#
[Header(Name: "Common-Header", Value: "value")] public interface IMyClient { ... }
```
or for a specific method:
```C#
public interface IMyClient { [GetMethod, Header("Specific-Method-Header", Value: "value")] Entity[] Get(); }
```
#### Response type
`ResponseAttribute` specifies the type of the value and status code returned by the method. This is the equivalent of the [ProducesResponseTypeAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.producesresponsetypeattribute).
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
    [GetMethod] Entity Get(int id);              // Sync call
    [GetMethod] Task<Entity> GetAsync(int id);   // Async call
    [PostMethod] void Post(Entity entity);       // Sync call
    [PostMethod] Task PostAsync(Entity entity);  // Async call
}
```

<a name="features-response"/> 

## HTTP response
You can get the full HTTP response, not just the body. To do this, the client interface must inherit the `INClient` interface.
```C#
public interface IMyClient : INClient
{
    [GetMethod] Task<Entity> GetAsync(int id);
}

...
HttpResponse<Entity> response = await myClient.AsHttp().GetHttpResponse(x => x.GetAsync(id: 1));
```
If your interface is used only as a client and you want to always get an HTTP response, just make the return type the `HttpResponse`:
```C#
public interface IMyClient
{
    [GetMethod] Task<HttpResponse<Entity>> GetAsync(int id);
    [PostMethod] Task<HttpResponse> PostAsync(Entity entity);
}

...
HttpResponse<Entity> response = await myClient.GetAsync(x => x.GetAsync(id: 1));
Entity entity = response.EnsureSuccess().Value;
```
You can also specify the type of expected error that is returned with failed HTTP statuses:
```C#
public interface IMyClient
{
    [GetMethod] Task<HttpResponseWithError<Entity, Error>> GetAsync(int id);
    [PostMethod] Task<HttpResponseWithError<Error>> PostAsync(Entity entity);
}

...
HttpResponseWithError<Entity, Error> response = await myClient.GetAsync(x => x.GetAsync(id: 1));
Error? error = response.Error;
```

<a name="features-status-code"/> 

## HTTP response status code
It is not always convenient to use with the `IActionResult` in NClient controllers, so you can use the `HttpResponseException` to return an error object and HTTP status code.
To use these exceptions you need to add NClient controllers in ASP.NET startup as follows:
```C#
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddNClientControllers().WithResponseExceptions();
}
```
After that, you can use exceptions in methods of your NClient controllers:
```C#
public Entity[] Get()
{
    ...
    throw new HttpResponseException(HttpStatusCode.BadRequest, new { Error = "Error message." });
}
```
For information on how to get HTTP status code, see section [Http response](#features-response).

<a name="features-errors"/>  

## Errors
All expected exceptions are inherited from the `NClientException`. There are two types of errors: client-side errors (`ClientException`) and controller-side errors (`ControllerException`).
### Client-side errors
`ControllerValidationException` - errors that occur if a client interface is invalid.  
`ClientRequestException` - exceptions to return information about a failed client request.   
### Controller-side errors
`ControllerValidationException` - errors that occur if a controller is invalid.  

<a name="features-httpclient"/>  

## HttpClient
By default, `System.Net.Http.HttpClient` is used for HTTP requests. But you can also create your own implementation of the `IHttpClientProvider` and pass it to the `WithCustomHttpClient` method:
```C#
IHttpClientProvider httpClientProvider = ...;

IMyClient myClient = NClientProvider
    .Use<IMyClient>(host: "http://localhost:8080")
    .WithCustomHttpClient(httpClientProvider)
    .Build();
```

<a name="features-serialization"/>  

## Serialization
By default, `System.Text.Json` is used for serialization. But you can also create your own implementation of the `ISerializerProvider` and pass it to the `WithCustomSerializer` method:
```C#
ISerializerProvider serializerProvider = ...;

IMyClient myClient = NClientProvider
    .Use<IMyClient>(host: "http://localhost:8080")
    .WithCustomSerializer(serializerProvider)
    .Build();
```

<a name="features-resilience"/> 

## Resilience
By default, a request is executed once without retries. If the request ended with an unsuccessful HTTP status code, an exception will be thrown. You can change this logic to achieve better resilience - create a resilience policy using `Polly` library:
```C#
var basePolicy = Policy<ResponseContext>.HandleResult(x => !x.HttpResponse.IsSuccessful).Or<Exception>();
var retryPolicy = basePolicy.WaitAndRetryAsync(
    retryCount: 2,
    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
var fallbackPolicy = basePolicy.FallbackAsync(
    fallbackValue: default!,
    onFallbackAsync: x => throw (x.Exception ?? x.Result.HttpResponse.ErrorException!));

IMyClient myClient = NClientProvider
    .Use<IMyClient>(host: "http://localhost:8080")
    .WithResiliencePolicy(fallbackPolicy.WrapAsync(retryPolicy))
    .Build();
```
You can also create your own implementation of the `IResiliencePolicyProvider` and pass it to the `WithResiliencePolicy` method.  
### Provided policies
Use the `WithResiliencePolicy` method overload to retry requests for any methods:
```C#
IMyClient myClient = NClientProvider
    .Use<IMyClient>(host: "http://localhost:8080")
    .WithResiliencePolicy(retryCount: 4, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
    .Build();
```
The parameters `retryCount` and `sleepDurationProvider` are optional. By default, 3 attempts are used with a quadratic increase in the delay between attempts. 
To use retries for safe methods (GET, HEAD, OPTIONS), use the `WithResiliencePolicyForSafeMethods` method. To use retries for all methods except POST, use the `WithResiliencePolicyForIdempotentMethods` method.
### Specific policy for a method
Set specific resilience policy for a method using the `WithResiliencePolicy` method overload:
```C#
var nonIdempotentMethodPolicy = ...;

IMyClient myClient = NClientProvider
    .Use<IMyClient>(host: "http://localhost:8080")
    .WithResiliencePolicy(
        methodSelector: x => (Func<Entity, Task>)x.PostAsync,
        asyncPolicy: nonIdempotentMethodPolicy)
    .Build();
```
Or create your own implementation of the `IMethodResiliencePolicyProvider` abstraction and pass its instance to the `WithResiliencePolicy` method.
### Policy for a created client
Create or change a policy for an already created client using the `Invoke` method:
```C#
public class MyResiliencePolicyProvider : IResiliencePolicyProvider { ... }
...
await myClient.AsResilient().Invoke(x => x.PostAsync(id), new MyResiliencePolicyProvider());
```
Please note, the client interface must inherit the `INClient` interface.

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
The `NClient.Extensions.DependencyInjection` package contains the `AddNClient` extension methods:
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

<a name="features-handling"/>

## Handling
Create your own implementation of the `IClientHandler` abstraction to provide a custom handling functionality for HTTP requests and responses. For example you can implement authentication:
```C#
public class AuthHandler : IClientHandler
{
    private readonly IConfiguration _configuration;
    
    public AuthHandler(IConfiguration configuration) 
        => _configuration = configuration;

    public Task<HttpRequest> HandleRequestAsync(HttpRequest httpRequest, MethodInvocation methodInvocation)
    {
        httpRequest.AddHeader(name: "Authorization", value: _configuration["token"]);
        return Task.FromResult(httpRequest);
    }

    public Task<HttpResponse> HandleResponseAsync(HttpResponse httpResponse, MethodInvocation methodInvocation) 
        => Task.FromResult(httpResponse);
}

...
IConfiguration configuration = ...;
IMyClient client = NClientProvider
    .Use<IMyClient>(host: "http://localhost:8080")
    .WithCustomHandlers(new IClientHandler[] { new AuthHandler(configuration) })
    .Build();
```

<a name="features-files"/> 

## File upload/download
Clients and controllers are able to upload and download files.
### Controller side
On the controller side, you do not need to perform any special actions, work with files as in the original ASP.NET Core:
```C#
[Api, Path("api/[controller]")]
public interface IFileController
{
    [GetMethod("files/{id}")] Task<IActionResult> GetFileAsync([RouteParam] long id);
    [PostMethod("files")] Task PostFileAsync(byte[] fileBytes);
}

public class FileController : ControllerBase, IFileController
{
    public async Task<IActionResult> GetTextFileAsync(long id) => 
        PhysicalFile(physicalPath: "/TextFile.txt", contentType: "text/plain");

    public Task PostTextFileAsync(byte[] fileBytes) => 
        ...;
}
```
### Client side
Since one method from the `IFileController` returns the `IActionResult`, it is necessary to override it using the `HttpResponse` as the return value:
```C#
public interface IFileClient : IFileController
{
    [Override] new Task<HttpResponse> GetFileAsync([RouteParam] long id);
}
```
After creating an interface for a client with an overridden method, you can create a client for downloading and uploading files:
```C#
IFileClient client = NClientProvider
    .Use<IFileClient>(host: "http://localhost:8080")
    .Build();
    
var httpResponseWithFile = await client.GetFileAsync(id: 1);
var fileBytes = httpResponseWithFile.RawBytes;

var fileBytesForSave = ...;
await client.PostFileAsync(fileBytesForSave);
```

<a name="features-system-httpclient"/> 

## System.Net.Http.HttpClient
An `HttpClient` is created for each instance of a client. Keep this in mind, because the `HttpClient` has problems. Create an instance for every request and you will run into socket exhaustion. Make it a singleton and it will not respect DNS changes. The best way would be to use `IHttpClientFactory`. You can create it yourself and pass it to the builder:
```C#
IHttpClientFactory httpClientFactory = ...;

IMyClient myClient = NClientProvider
    .Use<IMyClient>(host)
    .WithCustomHttpClient(httpClientFactory)
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
    .Use<IMyClient>(host)
    .WithCustomHttpClient(httpClientFactory, httpClientName: nameof(IMyClient))
    .WithLogging(logger)
    .Build();
```

<a name="providers" />  

# Providers
Providers give additional implementations for sending HTTP requests, serialization, and resilience. You can use the providers listed below or implement your own.  

<a name="providers-restsharp" />  

## RestSharp
To use `RestSharp` client instead of the default one, you need to install `NClient.Providers.HttpClient.RestSharp` package and use the `RestSharpHttpClientProvider`:
```C#
IMyClient myClient = NClientProvider
    .Use<IMyClient>(host: "http://localhost:8080")
    .WithCustomHttpClient(new RestSharpHttpClientProvider())
    .Build();
```

<a name="providers-newtonsoft" />  

## Newtonsoft.Json
If you want to use `Newtonsoft.Json` for serialization, you need to install `NClient.Providers.Serialization.Newtonsoft` package and use `NewtonsoftSerializerProvider`:
```C#
IMyClient myClient = NClientProvider
    .Use<IMyClient>(host)
    .WithCustomSerializer(new NewtonsoftSerializerProvider())
    .Build();
```

<a name="documentation" />  

## Documentation
You can find NClient documentation and samples [on the website](https://nclient.github.io/).

<a name="nuget" />  

## NuGet Packages
| Package name                                             | Description                                            | Dependencies                                           |
| :------------------------------------------------------- | :----------------------------------------------------- |:------------------------------------------------------ |
| [NClient](https://www.nuget.org/packages/NClient) | Tools for creating clients from interfaces and controllers including third-party | Castle, System.Text.Json, Microsoft.Extensions.Logging.Abstractions, System.Net.Http, Polly |
| [NClient.Standalone](https://www.nuget.org/packages/NClient.Standalone) | The same as NClient package, but without third-party | Castle, Microsoft.Extensions.Logging.Abstractions |
| [NClient.AspNetCore](https://www.nuget.org/packages/NClient.AspNetCore) | Allows you to annotate controllers via interfaces | Castle, Microsoft.AspNetCore.Mvc.* (Core, ApiExplorer, Cors, DataAnnotations) |
| [NClient.Extensions.DependencyInjection](https://www.nuget.org/packages/NClient.Extensions.DependencyInjection) | Extension methods for registration of clients in ServiceCollection | Castle, Microsoft.Extensions.DependencyInjection, System.Text.Json, System.Net.Http, Microsoft.Extensions.Logging.Abstractions, Polly |
| [NClient.Abstractions](https://www.nuget.org/packages/NClient.Abstractions) | Abstractions for clients and providers | Microsoft.Extensions.Logging.Abstractions |
| [NClient.Annotations](https://www.nuget.org/packages/NClient.Annotations) | Attributes for annotation of client interfaces and controllers | - |
| [NClient.Providers.HttpClient.System](https://www.nuget.org/packages/NClient.Providers.HttpClient.System) | System.Net.Http.HttpClient based HTTP client provider | System.Text.Json, System.Net.Http, Microsoft.Extensions.Logging.Abstractions |
| [NClient.Providers.HttpClient.RestSharp](https://www.nuget.org/packages/NClient.Providers.HttpClient.RestSharp) | RestSharp based HTTP client provider | System.Text.Json, Microsoft.Extensions.Logging.Abstractions, RestSharp |
| [NClient.Providers.Serialization.System](https://www.nuget.org/packages/NClient.Providers.Serialization.System) | System.Text.Json based serialization provider for NClient | System.Text.Json, Microsoft.Extensions.Logging.Abstractions |
| [NClient.Providers.Serialization.Newtonsoft](https://www.nuget.org/packages/NClient.Providers.Serialization.Newtonsoft) | SNewtonsoft.Json based serialization provider for NClient | Newtonsoft.Json, Microsoft.Extensions.Logging.Abstractions |
| [NClient.Providers.Resilience](https://www.nuget.org/packages/NClient.Providers.Resilience) | Polly based resilience policy provider | Polly, Microsoft.Extensions.Logging.Abstractions |
