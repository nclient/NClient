# <img src="logo.png" width="50" height="50" align="left" alt="logo">NClient: automatic type-safe .NET HTTP client

[![Nuget](https://img.shields.io/nuget/v/NClient)](https://www.nuget.org/packages/NClient/)
[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/NClient?label=nuget-pre)](https://www.nuget.org/packages/NClient/)
[![Nuget](https://img.shields.io/nuget/dt/NClient)](https://www.nuget.org/packages/NClient/)
[![GitHub last commit](https://img.shields.io/github/last-commit/nclient/NClient)](https://github.com/nclient/NClient/branches)
[![GitHub Workflow Status](https://img.shields.io/github/workflow/status/nclient/NClient/Test:%20Full)](https://github.com/nclient/NClient/actions)
[![Documentation](https://img.shields.io/badge/doc-wiki-brightgreen)](https://github.com/nclient/NClient/wiki)
[![GitHub](https://img.shields.io/github/license/nclient/NClient)](https://github.com/nclient/NClient/blob/main/LICENSE)  
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=nclient_NClient&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=nclient_NClient)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=nclient_NClient&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=nclient_NClient)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=nclient_NClient&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=nclient_NClient)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=nclient_NClient&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=nclient_NClient)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=nclient_NClient&metric=coverage)](https://sonarcloud.io/summary/new_code?id=nclient_NClient)

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

Do you like it? Give us a star! ⭐

#### Advantages of NClient:
- **Integration with ASP.NET:** clients are available for all controllers out of the box.
- **Resilience:** resilience is provided by different strategies. There is Polly support.
- **Serialization selection:** various serializers are avaliable for use: System.Text.Json, Newtonsoft JSON, System.XML, MessagePack, Protobuf, your own.
- **Auto validation of responses:** preset or custom validation of responses can be set.
- **Auto mapping of responses:** native or your own models can be returned from the client instead of responses or DTO.
- **Extension using handlers:** handlers allow to add custom logic to the client parts
- **Extension using providers:** the client functionality can be extended with native or your own providers. 
- **Easy error analysis:** your logger can be used in clients, and certainly exceptions have all the required information to investigate.
- **Easy to use with DI:** extension methods allow to add a client to a collection of services easily.
- **Maximum flexibility:** any step of the request execution pipeline can be replaced with your own.
- **[WIP] All types of applications:** the library can be used on backend (ASP.NET) and frontend (Blazor), its planed tо support mobile/desktop (MAUI).
- **[WIP] Various protocols:** REST protocol is provided as a ready-made solution, its planed to add GraphQL and RPC.

**Features:** Dynamic templated routing; Static routing; Dynamic query parameters; Collections as query parameters; Dynamic headers; Static headers; Dynamic body; Auto serialization and deserialization; HTTP/Transport context; Authentication; Asynchronous requests; Timeouts; Cancellation requests; Resilience policy; Response validation; Response mapping; File upload/download; Generic interfaces; Interface inheritance; Client factory; Versioning; Handling; Logging.

## Table of Contents
- [Why use NClient?](#why)
- [How to install?](#install)
- [Requirements](#requirements)
- [How to use?](#usage)
  - [Usage with third-party service](#usage-non-aspnet)
  - [Usage with ASP.NET Core](#usage-aspnet)
- [Documentation](#documentation)
- [Samples of applications](#sample-applications)
- [Contributing](#contributing)
- [NuGet Packages](#nuget)

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
Read about all the features in the [Annotation](https://github.com/nclient/NClient/wiki/Annotation) and [Routing](https://github.com/nclient/NClient/wiki/Routing) sections.
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
        .Use(maxRetries: 2, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))))
    ...
    .Build();
```
After calling the `For` method, you can configure the client as you need, for example, you can replace the serializer with `Newtonsoft.Json`, add the retry policy and so on (see full [documentation](https://github.com/nclient/NClient/wiki)).
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
You should do it if you want your client type not to contain "Сontroller" in the name or if you want to override some methods for the client (see `OverrideAttribute` in [Annotation](https://github.com/nclient/NClient/wiki/Annotation#override) section). 
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

<a name="documentation" />  

## Documentation
You can find NClient documentation on the [Wiki](https://github.com/nclient/NClient/wiki).

<a name="sample-applications" />  

## Samples of applications
See samples of applications in the [NClient.Samples](https://github.com/nclient/NClient.Samples) project.

<a name="contributing" />  

## Contributing
You’re thinking about contributing to NClient? Great! We love to receive contributions from the community! The simplest contribution is to give this project a star ⭐.  
Helping with documentation, pull requests, issues, commentary or anything else is also very welcome. Please review our [contribution guide](CONTRIBUTING.md).  
It's worth getting in touch with us to discuss changes in case of any questions. We can also give advice on the easiest way to do things.

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
- [NClient.Providers.Api.Rest](https://www.nuget.org/packages/NClient.Providers.Api.Rest): The provider that allows you to create clients for the REST API.
- [NClient.Providers.Transport.SystemNetHttp](https://www.nuget.org/packages/NClient.Providers.Transport.SystemNetHttp): The provider that allows you to transport messages using System.Net.Http.
- [NClient.Providers.Transport.RestSharp](https://www.nuget.org/packages/NClient.Providers.Transport.RestSharp): The provider that allows you to transport messages using RestSharp.
- [NClient.Providers.Serialization.SystemTextJson](https://www.nuget.org/packages/NClient.Providers.Serialization.SystemTextJson): The provider that allows you to use the System.Text.Json serializer.
- [NClient.Providers.Serialization.NewtonsoftJson](https://www.nuget.org/packages/NClient.Providers.Serialization.NewtonsoftJson): The provider that allows you to use the Newtonsoft.Json serializer.
- [NClient.Providers.Serialization.SystemXml](https://www.nuget.org/packages/NClient.Providers.Serialization.SystemXml): The provider that allows you to use the System.Xml.XmlSerializer serializer.
- [NClient.Providers.Serialization.MessagePack](https://www.nuget.org/packages/NClient.Providers.Serialization.MessagePack): The provider that allows you to use the MessagePackCSharp serializer.
- [NClient.Providers.Serialization.ProtobufNet](https://www.nuget.org/packages/NClient.Providers.Serialization.ProtobufNet): The provider that allows you to use the ProtobufNet serializer.
- [NClient.Providers.Resilience.Polly](https://www.nuget.org/packages/NClient.Providers.Resilience.Polly): The provider that allows you to implement resilience policies using the Polly library.
- [NClient.Providers.Mapping.HttpResponses](https://www.nuget.org/packages/NClient.Providers.Mapping.HttpResponses): The provider that allows you to return native HTTP responses.
- [NClient.Providers.Mapping.LanguageExt](https://www.nuget.org/packages/NClient.Providers.Mapping.LanguageExt): The provider that allows you to return the LanguageExt of a monad as responses.
