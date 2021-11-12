# <img src="logo.png" width="50" height="50" align="left" alt="logo">NClient: automatic type-safe .NET HTTP client

![Nuget](https://img.shields.io/nuget/v/NClient)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/NClient?label=nuget-pre)
![Nuget](https://img.shields.io/nuget/dt/NClient)
![GitHub last commit](https://img.shields.io/github/last-commit/nclient/NClient)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/nclient/NClient/Test:%20Full)
![GitHub](https://img.shields.io/github/license/nclient/NClient)

NClient is an automatic type-safe .NET HTTP client that can call web API methods using annotated interfaces. 
The main difference between NClient and its analogues is that NClient lets you annotate ASP.NET controllers via interfaces and then use these interfaces to create clients. 
It allows you to get rid of unwanted dependencies on a client side and to reuse an API description in clients without boilerplate code. 

```C#
// WebService.dll:
public class WeatherController : ControllerBase, IWeatherFacade {
    public Task<Weather> GetAsync(string city, DateTime date) => ...;
}

// WebService.Facade.dll:
[HttpFacade, Path("api/[facade]")] 
public interface IWeatherFacade {
    [GetMethod("{city}")] 
    Task<Weather> GetAsync([RouteParam] string city, [QueryParam] DateTime date);
}

// Client.dll:
IWeatherFacade weatherFacade = NClientGallery.Clients.GetRest()
    .For<IWeatherFacade>(host: "http://localhost:5000")
    .WithSafeResilience(maxRetries: 3)
    .Build();
Weather todaysWeather = await weatherFacade.GetAsync(city: "Chelyabinsk", date: DateTime.Today);
```

#### Advantages of NClient:
- **Integration with ASP.NET:** clients are available for all controllers out of the box.
- **Asynchronous requests:** asynchronous and synchronous requests are supported.
- **Resilience:** resilience is provided by different strategies. There is Polly support.
- **Serialization selection:** various serializers are avaliable for use: Newtonsoft JSON, system JSON, system XML, your own.
- **Response validation:** preset or custom validation of responses can be set.
- **Auto mapping of responses:** native or your own models can be returned from the client instead of responses or DTO.
- **Extension using handlers:** handlers allow to add custom logic to the client parts
- **Extension using providers:** the client functionality can be extended with native or your own providers. 
- **Easy error analysis:** your logger can be used in clients, and certainly exceptions have all the required information to investigate.
- **Easy to use with DI:** extension methods allow to add a client to a collection of services easily.
- **Maximum flexibility:** any step of the request execution pipeline can be replaced with your own.
- **[WIP] All types of applications:** the library can be used on backend (ASP.NET) and frontend (Blazor), its planed tо support mobile/desktop (MAUI).
- **[WIP] Various protocols:** REST protocol is provided as a ready-made solution, its planed to add GraphQL and RPC.

Do you like it? Give us a star! ⭐

## Table of Contents
- [Why use NClient?](#why)
- [How to install?](#install)
- [Requirements](#requirements)
- [How to use?](#usage)
  - [Usage with third-party service](#usage-non-aspnet)
  - [Usage with ASP.NET Core](#usage-aspnet)
- [Features](#features)
  - [Creating](#features-creating)
  - [Annotation](#features-annotation)
  - [Routing](#features-routing)
  - [Asynchronously calls](#features-async)
  - [HTTP response](#features-response)
  - [Errors](#features-errors)
  - [Api protocols](#features-api)
  - [Transport](#features-transport)
  - [Serialization](#features-serialization)
  - [Response validation](#features-validation)
  - [Resilience](#features-resilience)
  - [Response mapping](#features-mapping)
  - [Handling](#features-handling)
  - [Logging](#features-logging)
- [Tips](#tips)
  - [Specifics of using SystemHttpTransport](#tips-system-httpclient)
  - [Return HTTP response status code](#tips-status-code)
  - [File upload/download](#tips-files)
- [Extensions](#extensions)
  - [Dependency injection](#features-di)
- [Providers](#providers)
- [Documentation](#documentation)
- [Samples of applications](#sample-applications)
- [NuGet Packages](#nuget)
- [Contributing](#contributing)

<a name="why" />  

## Why use NClient?
Creating clients for web services can be quite a challenge because, in addition to data transfer, you need to implement query building, 
serialization, retry policy, mapping, error handling, logging — and this is not to mention the maintenance that comes with each update of your APIs.
What if you could create clients with a fraction of the effort? This is exactly what NClient hopes to achieve by allowing you to create clients declaratively.

By the way, you can [contribute](#contributing) to the NClient, not just use it :smiley:

<a name="install" />  

## How to install?
The easiest way is to install [NClient package](https://www.nuget.org/packages/NClient) using Nuget:
```
dotnet add package NClient
```

<a name="requirements" />

## Requirements
Use of the NClient library requires .NET Standard 2.0 or higher. The NClient controllers can be used with ASP.NET Core and .NET Core 3.1 target or higher.

<a name="usage" />  

## How to use?
First you have to create an interface describing available endpoints and input/output data of a service via annotations. After that, you can select the required type of client in `NClientGallery` and then set additional settings for it if it`s necessary.

<a name="usage-non-aspnet" />

### Usage with third-party service
If you want to send requests to a third-party service, you should create an interface that describes the service you want to make requests to. Follow the steps below:
#### Step 1: Install `NClient` on client-side
```
dotnet add package NClient
```
#### Step 2: Create the interface describing the API of the web service
```C#
[Path("api")]
public interface IProductServiceClient
{
    [PostMethod("products")]
    Task<Product> CreateAsync(Product product);
    
    [GetMethod("products/{id}")]
    Task<Product> GetAsync([RouteParam] int id);
}
```
Interface annotation is very similar to the annotation of controllers in ASP.NET. The `PathAttribute` defines the base path for all interface methods. The `PostMethodAttribute` specifies the type of HTTP method and the path to endpoint. 
Moreover, implicit annotations work as in ASP.NET controllers, for example, the `BodyParamAttribute` attribute will be implicitly set to the `product` parameter in `CreateAsync` method. And certainly the route templates are also supported. 
Read about all the features in the [Annotation](#features-annotation) and [Routing](#features-routing) sections.
#### Step 3: Create the client
```C#
IProductServiceClient client = NClientGallery.Clients.GetRest()
    .For<IProductServiceClient>(host: "http://localhost:8080")
    .Build();
```
The `GetRest` method creates a REST client with `System.Text.Json` serialization and without resilience policy.
#### Step 4 (optional): Configure the client
```C#
IProductServiceClient client = NClientGallery.Clients.GetRest()
    .For<IProductServiceClient>(host: "http://localhost:8080")
    .WithNewtonsoftJsonSerialization()
    .WithResilience(x => x
        .ForMethod(client => (Func<Product, Task<Product>>) client.CreateAsync)
        .For(maxRetries: 2, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))))
    ...
    .Build();
```
After calling the `For` method, you can configure the client as you need, for example, you can replace the serializer with `Newtonsoft.Json`, add the retry policy and so on (see [Features](#features) section).
#### Step 5: Send an http request
```C#
// Equivalent to the following request: 
// curl -X POST -H "Content-type: application/json" --data "{ id: 1 }" http://localhost:8080/api/products
Product product = await client.CreateAsync(new Product(name: "MyProduct"));
```

<a name="usage-aspnet" />

### Usage with ASP.NET Core
If you want to generate a client for your ASP.NET web service, you need to extract an interface for your controller and annotate it with NClient attributes. They are very similar to attributes for ASP.NET controllers. Follow the steps below:
#### Step 1: Install `NClient.AspNetCore` package on server-side
```
dotnet add package NClient.AspNetCore
```
#### Step 2: Create the controller
```C#
public class WeatherForecastController : ControllerBase
{
    public async Task<WeatherForecast> GetAsync(DateTime date) =>
        new WeatherForecast(date: date, temperatureC: -25);
}
```
Don't annotate your controller with ASP.NET attributes that may semantically conflict with the NClient attributes you are going to use.
Other attributes (including your own) can be used without restrictions.
#### Step 3: Extract the interface for your controller and annotate it with NClient attributes
```C#
[HttpFacade, Path("[controller]")]                                // equivalent to [ApiController, Route("[controller]")]
public interface IWeatherForecastController
{
    [GetMethod]                                                   // equivalent to [HttpGet]
    Task<WeatherForecast> GetAsync([QueryParam] DateTime date);   // equivalent to [FromQuery]
}

public class WeatherForecastController : ControllerBase, IWeatherForecastController { ... }
```
The annotation in the interface instead of the controller allows you to put the interface in a separate assembly. 
Therefore, the client useing this interface doesn`t depend on the ASP.NET application.
#### Step 4 (optional): Create the interface for the client
```C#
public interface IWeatherForecastClient : IWeatherForecastController { }
```
You should do it if you want your client type not to contain "Сontroller" in the name or if you want to override some methods for the client (see `OverrideAttribute` in [Annotation](#features-annotation-override) section). 
There is no need to duplicate interface attributes, they are inherited.
#### Step 5: Add NClient controllers to ServiceCollection in Startup.cs
```C#
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddNClientControllers();
}
```
The `AddNClientControllers` method can be used in combination with the `AddControllers`. 
This will allow you to use standard ASP.NET controllers together with NClient controllers in the same application.
#### Step 6: Install `NClient` package on client-side
```
dotnet add package NClient
```
#### Step 7: Create the client
```C#
IWeatherForecastController client = NClientGallery.Clients.GetRest()
    .For<IWeatherForecastController>(host: "http://localhost:8080")
    .Build();
```
If you decide to follow the 4th step, use the `IWeatherForecastClient` interface instead of `IWeatherForecastController`.
#### Step 8: Send an http request
```C#
// Equivalent to the following request: 
// curl -X GET -H "Content-type: application/json" http://localhost:8080/WeatherForecast?date=2021-03-13T00:15Z
WeatherForecast forecast = await client.GetAsync(DateTime.Now);
```

<a name="features" />  

## Features
The list of the main features of NClient library:

<a name="features-creating" />  

### Creating
There are several ways to create a client, so you can choose the most suitable one.

#### NClientGallery
The `NClientGallery` class contains already configured clients and factories. There are two types of gallery usage:
Static option:
```C#
IMyClient myClient = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host: "http://localhost:8080")
    .Build();
```
Creating the `NClientGallery` instance:
```C#
INClientGallery nclientGallery = new NClientGallery();
IMyClient myClient = nclientGallery.GetRest()
    .For<IMyClient>(host: "http://localhost:8080")
    .Build();
```

The gallery also has pre-configured client factories, that will be convenient for creating clients with the same settings. Create them using `NClientGallery.ClientFactories.*` methods:
```C#
INClientFactory clientFactory = NClientGallery.ClientFactories.GetRest()
    .For(factoryName: "myFactory")
    .Build();

IMyClient myClient = clientFactory.Create<IMyClient>(host: "http://localhost:8080");
```

Created through the gallery pre-configured clients and factories can be configured with your own settings using optional methods `With...`:
```C#
ILoggerFactory loggerFactory = ...;

IMyClient myClient = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host: "http://localhost:8080")
    .WithLogging(loggerFactory)
    .Build();
```

The gallery contains several types of clients:
- Rest: REST client with System.Text.Json serializer. The request is considered unsuccessful if the response code is not equal to 200.
The request is considered unsuccessful if the response code is not equal to 200.
- Custom: without pre-configuration.

The custom client is a special case because it has no presets, so you will have to fully configure it by yourself:
```C#
IMyClient myClient = NClientGallery.Clients.GetCustom()
    .For<IMyClient>(host: "http://localhost:8080")
    .UsingRestApi()
    .UsingHttpTransport()
    .UsingJsonSerializer()
    ...
    .Build();
```
Methods starting with `Using...` are required, you will not be able to skip them. 
After all the required settings are set, the optional methods will become available.
Optional methods start with `With...`.

#### Without NClientGallery
You can create builders for clients without a gallery, there is a set of classes for this: `NClientRestBuilder` (Rest) and `NClientBuilder` (Custom).
Usage example:
```C#
INClientRestBuilder nclientRestBuilder = new NClientRestBuilder();

IMyClient myClient = nclientRestBuilder
    .For<IMyClient>(host: "http://localhost:8080")
    .Build();
```
There are similar classes for factories: `NClientRestFactoryBuilder` and `NClientFactoryBuilder`.

<a name="features-annotation" />  

### Annotation
The interfaces of clients and controllers can be annotated with attributes to describe endpoints and add additional functionality. The following attributes can be used:

#### Base attributes
The `HttpFacade` attribute is an equivalent of the `ApiControllerAttribute` from ASP.NET Core.
```C#
[HttpFacade] public interface IMyController { ... }
```
The base URL route for API can be set by the `PathAttribute`.
```C#
[Path("api")] public interface IMyClient { ... }
```
A client interface can be annotated with optional the `FacadeAttribute` if no other NClient attributes are used in the client interface. It can be useful for improving code readability or for detecting client interfaces if it does not have any other NClient attribute.
```C#
[Facade] public interface IMyClient { ... }
```
#### HTTP methods
Each interface method must have an HTTP attribute that defines the HTTP method for request. There are seven types of such attributes: `GetMethodAttribute`, `HeadMethodAttribute`, `PostMethodAttribute`, `PutMethodAttribute`, `PatchMethodAttribute`, `DeleteMethodAttribute`, `OptionsMethodAttribute`.
```C#
public interface IMyClient { [GetMethod] Entity[] Get(); }
```
Optionally, you can specify a relative path by passing it to the constructor:
```C#
public interface IMyClient { [GetMethod("entities")] Entity[] Get(); }
```
#### HTTP parameters
By default, custom type parameters are passed in the request body and primitive type parameters are passed in a URL query. You can explicitly specify how to pass a parameter using attributes: `QueryParamAttribute`, `BodyParamAttribute`, `HeaderParamAttribute`, `RouteParamAttribute`.
```C#
public interface IMyClient { [PostMethod] void Post([QueryParam] Entity entity); }
```
It is also possible to change a parameter name:
```C#
public interface IMyClient { [PostMethod] void Post([QueryParam(Name = "myEntity")] Entity entity); }
```
#### Properties
In addition to its main purpose, the `QueryParamAttribute` allows you to change a property name of a custom object that is passed in URL query:
```C#
public class Entity { [QueryParam(Name = "id")] public int Id }
```
The same effect will occur if you use the `FromQueryAttribute` from ASP.NET Core. But in this case you will have a dependency on ASP.NET, which is probably unwanted for client assemblies.

The names of the properties of custom objects that are passed in the request body can be changed using the `JsonPropertyNameAttribute`:
```C#
public class Entity { [JsonPropertyName("id")] public int Id }
```
#### Static HTTP headers
Use the `HeaderAttribute` attribute to add a static header. Static headers can be added for all methods:
```C#
[Header(Name: "Common-Header", Value: "value")] public interface IMyClient { ... }
```
or for a specific method:
```C#
public interface IMyClient { [GetMethod, Header("Specific-Method-Header", Value: "value")] Entity[] Get(); }
```
#### Override
<a name="features-annotation-override" /> 

The interface methods can be overridden in the inheriting interfaces. Although this is not an override in the usual sense, because you can change the return type of the method:
```C#
public interface IMyController { [GetMethod] Task<int> GetAsync([RouteParam] long id); }
public interface IMyClient : IMyController { [Override] new Task<string> GetFileAsync(long id); }
```
Overridden methods inherit all method and parameters attributes from the method of the inherited interface. If necessary, you can replace the attributes or add new ones. It is worth noting that multiple inheritance is forbidden if you want to override the method.
#### Authorization
ASP.NET has two methods for configuring authentication: `AuthorizeAttribute` and `AllowAnonymousAttribute` attributes. There are two equivalents in the client: `AuthorizedAttribute` and `AnonymousAttribute`.
#### Response type
The `ResponseAttribute` is optional attribute that specifies the type of the value and status code returned by the method. This is the equivalent of the [ProducesResponseTypeAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.producesresponsetypeattribute).
```C#
public interface IMyClient { [GetMethod, Response(typeof(Entity[]), HttpStatusCode.OK)] Entity[] Get(); }
```
#### Versioning
There are two attributes for API versioning: `VersionAttribute` and `ToVersionAttribute`. They are the equivalents of `ApiVersionAttribute` and `MapToApiVersionAttribute` from ASP.NET (see [ASP.NET API Versioning](https://github.com/microsoft/aspnet-api-versioning)). They are needed only for controllers and therefore will be ignored when generating the client. Here is an example of their use for the controller interface:
```C#
[Version("1.0"), Version("2.0"), Path("api/v{version:apiVersion}")]
public interface IMyController 
{
    [GetMethod] Entity[] Get(int id);                      // Available in versions 1.0 and 2.0
    [DeleteMethod, ToVersion("2.0")] void Delete(int id);  // Available in version 2.0
}
```
For the client, there is the `UseVersionAttribute`. You can add it to set the API version for a client:
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

<a name="features-routing" /> 

### Routing
To generate the path to the endpoint, the client has support for route templates as in ASP.NET. Route templates are passed to the following attributes: `PathAttribute`, `QueryParamAttribute`, `BodyParamAttribute`, `HeaderParamAttribute`, `RouteParamAttribute`. The `PathAttribute` sets the base path for all interface methods, the other attributes set the relative path to a specific endpoint.

Routes can contain two types of tokens that are inserted into the path in runtime:
#### Controller token
The name of interface can be substituted in the path:
```C#
[Path("api/[controller]")] public interface IEntitiesClient { ... } // the route will be: api/entities
```
The name of the interface will be substituted without prefixes (`I`) and postfixes (`Controller`, `Client`, `Facade`).
#### Parameter token
Method parameters can be completely substituted in the path:
```C#
public interface IMyClient { [GetMethod("entities/{id}")] Entity[] Get(int id); }
```
If a complex object is passed as a parameter, you can insert its property into the path:
```C#
public interface IMyClient { [PutMethod("entities/{entity.Id}")] void Put(Entity entity); }
```

<a name="features-async" /> 

### Asynchronously calls
To execute a request to the web-service asynchronously, you should define the returned type as `Task` or `Task<>`:
```C#
public interface IMyClient
{
    [GetMethod] Entity Get(int id);              // Sync call
    [GetMethod] Task<Entity> GetAsync(int id);   // Async call
    [PostMethod] void Post(Entity entity);       // Sync call
    [PostMethod] Task PostAsync(Entity entity);  // Async call
}
```

<a name="features-response" /> 

### HTTP response
You can get the full HTTP response from the client's method, not just the body.
If your interface is used only as a client and you want to always get an HTTP response, you have the following options: return the NClient `IHttpResponse` or return the original transport response.
#### IHttpResponse
If you use a default transport (`NClient.Providers.Transport.Http.System`) and want to receive an HTTP response along with a deserialized body, return `IHttpResponse` or `IHttpResponse<T>`:
```C#
public interface IMyClient
{
    [GetMethod] Task<IHttpResponse<Entity>> GetAsync(int id);
    [PostMethod] Task<IHttpResponse> PostAsync(Entity entity);
}
...

HttpResponse<Entity> response = await myClient.GetAsync(x => x.GetAsync(id: 1));
Entity entity = response.EnsureSuccess().Value;
```
If you want to specify the type of expected error that is returned in body with failed HTTP statuses, then use `IHttpResponseWithError<TValue, TError>` or `IHttpResponseWithError<TError>`:
```C#
public interface IMyClient
{
    [GetMethod] Task<IHttpResponseWithError<Entity, Error>> GetAsync(int id);
    [PostMethod] Task<IHttpResponseWithError<Error>> PostAsync(Entity entity);
}
...

IHttpResponseWithError<Entity, Error> response = await myClient.GetAsync(x => x.GetAsync(id: 1));
Error? error = response.Error;
```
Oviously controller cannot implemetn the interface that returns HTTP response, so the controller should return what it should, and you need to create a client interface, inherit it from the controller interface and override the method by changing the return type to HTTP response (see `OverrideAttribute` in [Annotation](#features-annotation-override) section).
#### Transport response
There are cases when there is not enough functionality of `IHttpResponse`, then you can return transport responses. If you use `NClient.Providers.Transport.Http.System` transport package, you can do this:
```C#
public interface IMyClient
{
    [GetMethod] Task<HttpResponseMessage> GetAsync(int id);
}
```
or if you use `NClient.Providers.Transport.Http.RestSharp` transport package:
```C#
public interface IMyClient
{
    [GetMethod] Task<IRestResponse> GetAsync(int id);
}
```
Unlike the first option with `IHttpResponse` return, you can get a transport response without overriding and additional methods in the client. To do this, the client interface must inherit the `INClient` interface, then you will be able to do this:
```C#
public interface IMyClient : INClient
{
    [GetMethod] Task<Entity> GetAsync(int id);
}
...
IHttpResponse<Entity> response = await myClient.AsTransport().GetTransportResponse(x => x.GetAsync(id: 1));
```
In general, this way is not recommended because it complicates unit testing, but sometimes it can be useful.

<a name="features-errors" />  

### Errors
All expected exceptions thrown by the client are inherited from the `NClientException`. In turn, they are divided into two types: client-side errors (inherited from `ClientException`) and controller-side errors (inherited from `ControllerException`).
#### Client-side errors
`ControllerValidationException` - errors that occur if a client interface is invalid.  
`ClientRequestException` - exceptions to return information about a failed client request.   
#### Controller-side errors
`ControllerValidationException` - errors that occur if a controller is invalid.

<a name="features-api" />  

### Api protocols
Currently, only the REST API protocol is provided as a ready-made implementation. To use it, you just need to create a client via `NClientGallery.Clients.GetRest` method. To use it in your custom client configuration, you need to call the `UsingRestApi` method:
```C#
IMyClient myClient = NClientGallery.Clients.GetCustom()
    .For<IMyClient>(host: "http://localhost:8080")
    .UsingRestApi()
    ...
    .Build();
```
If you need a non-REST protocol, you have the opportunity to create your own implementation of converting an interface method call into a request. To do this, you will need to implement the `IRequestBuilderProvider` interface and pass its implementation to the `UsingCustomApi` method:
```C#
IRequestBuilderProvider myRequestBuilderProvider = ...;

IMyClient myClient = NClientGallery.Clients.GetCustom()
    .For<IMyClient>(host: "http://localhost:8080")
    .UsingCustomApi(myRequestBuilderProvider)
    ...
    .Build();
```

<a name="features-transport" />  

### Transport
To deliver the request to the endpoint, you need transport. By default, HTTP transport (`NClient.Providers.Transport.Http.System` package) is used for message delivery in pre-configurated clients. To use it for your custom client, you should call the `UsingHttpTransport` method:
```C#
IMyClient myClient = NClientGallery.Clients.GetCustom()
    .For<IMyClient>(host: "http://localhost:8080")
    .UsingRestApi()
    .UsingHttpTransport()
    ...
    .Build();
```
If you are not satisfied with HTTP transport, you can implement your own and use it in the client. To do this, you will need to create your custom implementation of transport and pass it to the `UsingCustomTransport` method:
```C#
ITransportProvider<TRequest, TResponse> myTransportProvider = ...;
ITransportRequestBuilderProvider<TRequest, TResponse> myTransportRequestBuilderProvider = ...;
IResponseBuilderProvider<TRequest, TResponse> myResponseBuilderProvider = ...;

IMyClient myClient = NClientGallery.Clients.GetCustom()
    .For<IMyClient>(host: "http://localhost:8080")
    .UsingRestApi()
    .UsingCustomTransport(
        myTransportProvider, 
        myTransportRequestBuilderProvider, 
        myResponseBuilderProvider)
    ...
    .Build();
```

<a name="features-serialization" />  

### Serialization
Serialization is needed to convert data when sending requests and receiving responses. By default, `System.Text.Json` is used for serialization. But you can replace it with other options:
```C#
IMyClient myClient = NClientGallery.Clients.GetCustom()
    .For<IMyClient>(host: "http://localhost:8080")
    .WithSystemJsonSerialization()     // It is already in use, so it is not necessary to call this method
    // or
    .WithJsonSerialization()           // This is the alias for WithJsonSerialization
    // or
    .WithNewtonsoftJsonSerialization()
    // or
    .WithSystemXmlSerialization()
    ...
    .Build();
```
You can also create your own implementation of the `ISerializerProvider` and pass it to the `WithCustomSerializer` method:
```C#
ISerializerProvider serializerProvider = ...;

IMyClient myClient = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host: "http://localhost:8080")
    .WithCustomSerialization(serializerProvider)
    .Build();
```

<a name="features-validation" /> 

### Response validation
Validation of responses is needed, for example, to make sure that only responses with a successful code will be returned from the client. By default, the client checks that the response code is between 200 and 299 values and throws an exception if it is not. If you need a different logic, you can use the `WithResponseValidation` method:
```C#
IMyClient myClient = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host: "http://localhost:8080")
    .WithResponseValidation(
        isSuccess: responseContext => responseContext.Response.StatusCode == HttpStatusCode.OK,
        onFailure: responseContext => throw new Exception($"Response with code: {responseContext.Response.StatusCode}"))
    .Build();
```
An important point: if the client method returns a transport response, for example, HttpResponseMessage, then validation will be skipped.Use the `WithoutResponseValidation` method to remove the validation of responses in all cases. 

<a name="features-resilience" /> 

### Resilience
Requests can end with errors for many reasons, so it is necessary to ensure resilience. By default, a request is executed once without retries. If the request ended with an unsuccessful HTTP status code and the returned method value is not HTTP response, an exception will be thrown. To change the logic of retries, you can use the `WithResilience` methods.
#### Common policy
Use the `WithFullResilience` method to retry requests for any methods:
```C#
IMyClient myClient = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host: "http://localhost:8080")
    .WithFullResilience(
        maxRetries: 4, 
        getDelay: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
        shouldRetry: responseContext => !responseContext.Response.IsSuccessStatusCode)
    .Build();
```
The parameters `maxRetries`, `getDelay` and `shouldRetry` are optional. By default, 3 attempts are used with a quadratic increase in the delay between attempts for responses with unsuccessful codes. 
To use retries for safe methods (GET, HEAD, OPTIONS), use the `WithSafeResilience` method. To use retries for all methods except POST, use the `WithIdempotentResilience` method.
#### Specific policy for a method
Set specific resilience policy for a method using the `WithResilience` method:
```C#
IMyClient myClient = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host: "http://localhost:8080")
    .WithResilience(builder => builder
        .ForMethod(client => ((Func<Entity, Task>)x.PostAsync).Use(maxRetries: 4))
    .Build();
```
#### Policy combinations
You can flexibly configure resilience using a combination of policies:
```C#
IMyClient myClient = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host: "http://localhost:8080")
    .WithFullResilience(maxRetries: 4)
    .WithResilience(builder => builder
        .ForMethod(client => ((Func<Entity, Task>)x.PostAsync).DoNotUse())
    .Build();
```
#### Polly policy
For more complex cases, you can use the `Polly` library:
```C#
var retryPolicy = Policy<ResponseContext>
    .HandleResult(x => !x.HttpResponse.IsSuccessful)
        .Or<Exception>()
    .WaitAndRetryAsync(
        retryCount: 2,
        sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

IMyClient myClient = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host: "http://localhost:8080")
    .WithResilience(builder => builder
        .ForAllMethods().UsePolly(retryPolicy))
    .Build();
```
You can also create your own implementation of the `IResiliencePolicyProvider` and pass it to the `Use` method. 
#### Runtime policy change
You may need to set policies in runtime. To create or change a policy for an already created client use the `Invoke` method:
```C#
public class MyResiliencePolicyProvider : IResiliencePolicyProvider { ... }
...
await myClient.AsResilient().Invoke(x => x.PostAsync(id), new MyResiliencePolicyProvider());
```
Please note, the client interface must inherit the `INClient` interface.

<a name="features-mapping" /> 

### Response mapping
Response mapping allows you to transform the data received from the transport response into other models. This can be useful, for example, if you want to reduce dependence on transport and NClient. To use mapping, you need to implement the `IResponseMapper<TRequest, TResponse>` interface and pass implementation to the `WithResponseMapping` method:
```C#
public interface IMyClient
{
    [GetMethod] Task<MyModel> GetAsync(int id);
}
...

IResponseMapper<HttpRequestMessage, HttpResponseMessage> myMapper = ...; // Mapper for MyModel
IMyClient myClient = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host: "http://localhost:8080")
    .WithResponseMapping(myMapper)
    .Build();

MyModel result = await myClient.GetAsync(id: 1);
```
By default, mappers for `IHttpResponse` and `IResult` models are already set in the client. Use the `WithoutResponseMapping` method to remove all mappers from client. 

<a name="features-handling" />

### Handling
To add your own logic for executing requests, create your own implementation of the `IClientHandler` interface and pass implementation to the `WithHandling` method. For example you can implement authentication using handlers:
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

    public Task<IHttpResponse> HandleResponseAsync(IHttpResponse httpResponse, MethodInvocation methodInvocation) 
        => Task.FromResult(httpResponse);
}
...

IConfiguration configuration = ...;
IMyClient client = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host: "http://localhost:8080")
    .WithHandling(new IClientHandler[] { new AuthHandler(configuration) })
    .Build();
```

<a name="features-logging" /> 

### Logging
To log information about the execution of the request, the status of responses and errors, you can set a logger or logger factory for a client:
```C#
ILogger<IMyClient> logger = ...;

IMyClient client = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host: "http://localhost:8080")
    .WithLogging(logger)
    .Build();
```

<a name="tips" /> 

## Tips
This section contains some tips and examples that may be useful.

<a name="tips-system-httpclient" /> 

### Specifics of using SystemHttpTransport
An `HttpClient` is created for each instance of a client. Keep this in mind, because the `HttpClient` has problems. Create an instance for every request and you will run into socket exhaustion. Make it a singleton and it will not respect DNS changes. The best way would be to use `IHttpClientFactory`. You can create it yourself and pass it to the builder:
```C#
IHttpClientFactory httpClientFactory = ...;

IMyClient myClient = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host)
    .UsingSystemHttpTransport(httpClientFactory)
    .Build();
```
For more fine-tuning, you can use a named `HttpClient`:
```C#
IMyClient myClient = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host)
    .UsingSystemHttpTransport(httpClientFactory, httpClientName: "myHttpClient")
    .Build();
```
But perhaps the best option is to use DI extensions  (see [Dependency injection](#features-di) section):
```C#
var serviceProvider = new ServiceCollection()
    .AddHttpClient()
    .AddNClient<IMyClient>(host: "http://localhost:8080")
    .BuildServiceProvider();

IMyClient myClient = serviceProvider.GetRequiredService<IMyClient>();
```

<a name="tips-status-code" /> 

### Return HTTP status code
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

<a name="tips-files" /> 

### File upload/download
Clients and controllers are able to upload and download files. Here's how you can implement this on the client and on the server:
#### Controller side
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
#### Client side
Since one method from the `IFileController` returns the `IActionResult`, it is necessary to override it using the `IHttpResponse` as the return value:
```C#
public interface IFileClient : IFileController
{
    [Override] new Task<IHttpResponse> GetFileAsync([RouteParam] long id);
}
```
After creating an interface for a client with an overridden method, you can create a client for downloading and uploading files:
```C#
IFileClient client = NClientGallery.Clients.GetRest()
    .For<IFileClient>(host: "http://localhost:8080")
    .Build();
    
var httpResponseWithFile = await client.GetFileAsync(id: 1);
var fileBytes = httpResponseWithFile.Content.Bytes;

var fileBytesForSave = ...;
await client.PostFileAsync(fileBytesForSave);
```
To work with streams, return a `HttpResponseMessage` instead of a `IHttpResponse`.

<a name="extensions" />  

## Extensions
NClient has packages that extend the capabilities of other libraries and make working with them more convenient.

<a name="features-di" /> 

### Dependency injection
The `NClient.Extensions.DependencyInjection` package contains methods for adding clients to the `ServiceCollection`.
package contains the `AddNClient` extension methods:
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

<a name="providers" />  

## Providers
Providers give additional implementations for sending HTTP requests, serialization, and resilience. You can use the providers listed below or implement your own.  

<a name="providers-rest" />  

### NClient.Providers.Api.Rest
The package [NClient.Providers.Api.Rest](https://www.nuget.org/packages/NClient.Providers.Api.Rest) implements the conversion of an interface method call into a REST request. This package is included in the [NClient](https://www.nuget.org/packages/NClient) package dependencies and is the default provider for REST clients. To use this provider, you need to call the `UsingRestApi` method when building a client.

<a name="providers-http-system" />  

### NClient.Providers.Transport.Http.System
The package [NClient.Providers.Transport.Http.System](https://www.nuget.org/packages/NClient.Providers.Transport.Http.System) implements transport to the endpoint for requests over HTTP using `System.Net.Http`. This package is included in the [NClient](https://www.nuget.org/packages/NClient) package dependencies and is the default provider for the HTTP transport implementation. To use this provider, you need to call the `UsingSystemHttpTransport` method (or its alias `UsingHttpTransport`) when building a client.

<a name="providers-restsharp" />  

### NClient.Providers.Transport.Http.RestSharp
The package [NClient.Providers.Transport.Http.RestSharp](https://www.nuget.org/packages/NClient.Providers.Transport.Http.RestSharp) implements transport to the endpoint for requests over HTTP using the RestSharp library. This can be useful, for example, to switch from the RestSharp library to NClient - you can return standard RestSharp (IRestResponse) responses from client interface methods. 
To use `RestSharp` client instead of the default one, you need to install package and use the `UsingRestSharpTransport` method:
```C#
IMyClient myClient = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host: "http://localhost:8080")
    .UsingRestSharpTransport()
    .Build();
```
Keep in mind that RestSharp uses legacy `HttpWebRequest`, so it won't work in Blazor.

<a name="providers-json-system" />  

### NClient.Providers.Serialization.Json.System
The package [NClient.Providers.Serialization.Json.System](https://www.nuget.org/packages/NClient.Providers.Serialization.Json.System) implements JSON serialization using `System.Text.Json`. This package is included in the [NClient](https://www.nuget.org/packages/NClient) package dependencies and is the default provider for the serialization implementation. To use this provider, you need to call the `UsingSystemJsonSerialization` method (or its alias `UsingJsonSerializer`) when building a client.

<a name="providers-newtonsoft" />  

### NClient.Providers.Serialization.Json.Newtonsoft
The package [NClient.Providers.Serialization.Json.System](https://www.nuget.org/packages/NClient.Providers.Serialization.Json.System) implements JSON serialization using `Newtonsoft.Json`. This package will be useful if you need deserialization of generics or some Newtonsoft functionality. If you want to use `Newtonsoft.Json` for serialization, you need to install package and use the `UsingNewtonsoftJsonSerialization` method:
```C#
IMyClient myClient = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host)
    .UsingNewtonsoftJsonSerialization()
    .Build();
```

<a name="providers-xml-system" />  

### NClient.Providers.Serialization.Xml.System
The package [NClient.Providers.Serialization.Json.System](https://www.nuget.org/packages/NClient.Providers.Serialization.Json.System) implements XML serialization using `System.Xml.Serialization`. If you want to use `System.Xml.Serialization` for serialization, you need to install package and use the `WithSystemXmlSerialization` method:
```C#
IMyClient myClient = NClientGallery.Clients.GetRest()
    .For<IMyClient>(host)
    .WithSystemXmlSerialization()
    .Build();
```

<a name="providers-polly" />  

### NClient.Providers.Resilience.Polly
The package [NClient.Providers.Resilience.Polly](https://www.nuget.org/packages/NClient.Providers.Resilience.Polly) implements resilience for client method calls. This package is included in the [NClient](https://www.nuget.org/packages/NClient) package dependencies and is the default provider for resilience. To use this provider, you need to call the `WithPolly*` method and other with the prefix `Polly`.

<a name="providers-http-response" />  

### NClient.Providers.Mapping.HttpResponses
The package [NClient.Providers.Mapping.HttpResponses](https://www.nuget.org/packages/NClient.Providers.Mapping.HttpResponses) implements mapping to a set of models representing HTTP responses with deserialized data that can be used as return types of client methods. This package is included in the [NClient](https://www.nuget.org/packages/NClient) and the mapper from the package is already in the client. To use this provider for custom clients, you need to call the `WithResponseToHttpResponseMapping` method when building a client.

<a name="providers-language-ext" />  

### NClient.Providers.Mapping.LanguageExt
The package [NClient.Providers.Mapping.LanguageExt](https://www.nuget.org/packages/NClient.Providers.Mapping.LanguageExt) implements mapping to a set of monads from LanguageExt library that can be used as return types of client methods. To use this provider, you need to call the `WithResponseToMonadMapping` method when building a client.

<a name="documentation" />  

### Documentation
You can find NClient documentation and samples [on the website](https://nclient.github.io/).

<a name="sample-applications" />  

## Samples of applications
See samples of applications in the [NClient.Samples](https://github.com/nclient/NClient.Samples) project.

<a name="nuget" />  

## NuGet Packages

### Main
- [NClient](https://www.nuget.org/packages/NClient): Tools for creating clients from interfaces including third-party.
- [NClient.Standalone](https://www.nuget.org/packages/NClient.Standalone): The same as NClient package, but without third-party.
- [NClient.AspNetCore](https://www.nuget.org/packages/NClient.AspNetCore): Allows you to annotate controllers via interfaces.
- [NClient.Abstractions](https://www.nuget.org/packages/NClient.Abstractions): Abstractions for clients and providers.
- [NClient.Annotations](https://www.nuget.org/packages/NClient.Annotations): Attributes for annotation of clients and controllers interfaces.

### Extensions
- [NClient.Extensions.DependencyInjection](https://www.nuget.org/packages/NClient.Extensions.DependencyInjection): Extension methods for registration of clients in ServiceCollection.

### Providers
- [NClient.Providers.Api.Rest](https://www.nuget.org/packages/NClient.Providers.Api.Rest): Rest based api provider.
- [NClient.Providers.Transport.Http.System](https://www.nuget.org/packages/NClient.Providers.Transport.Http.System): System.Net.Http based transport provider.
- [NClient.Providers.Transport.Http.RestSharp](https://www.nuget.org/packages/NClient.Providers.Transport.Http.RestSharp): RestSharp based HTTP client provider.
- [NClient.Providers.Serialization.Json.System](https://www.nuget.org/packages/NClient.Providers.Serialization.Json.System): System.Text.Json based serialization provider.
- [NClient.Providers.Serialization.Json.Newtonsoft](https://www.nuget.org/packages/NClient.Providers.Serialization.Json.Newtonsoft): Newtonsoft.Json based serialization provider.
- [NClient.Providers.Serialization.Json.Newtonsoft](https://www.nuget.org/packages/NClient.Providers.Serialization.Json.Newtonsoft): SSystem.Xml.XmlSerializer based serialization provider.
- [NClient.Providers.Resilience.Polly](https://www.nuget.org/packages/NClient.Providers.Resilience.Polly): Polly based resilience policy provider
- [NClient.Providers.Mapping.HttpResponses](https://www.nuget.org/packages/NClient.Providers.Mapping.HttpResponses): Native NClient HttpResponse results provider.
- [NClient.Providers.Mapping.LanguageExt](https://www.nuget.org/packages/NClient.Providers.Mapping.LanguageExt): LanguageExt based results provider

<a name="contributing" />  

## Contributing
You’re thinking about contributing to NClient? Great! We love to receive contributions from the community! The simplest contribution is to give this project a star ⭐.  
Helping with documentation, pull requests, issues, commentary or anything else is also very welcome. Please review our [contribution guide](CONTRIBUTING.md).  
It's worth getting in touch with us to discuss changes in case of any questions. We can also give advice on the easiest way to do things.
