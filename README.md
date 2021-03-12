# NClient
![Nuget](https://img.shields.io/nuget/v/NClient)
![GitHub Release Date](https://img.shields.io/github/release-date/nclient/nclient)
![GitHub last commit](https://img.shields.io/github/last-commit/nclient/nclient)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/nclient/nclient/Test)

NClient is a HTTP client that allows you to call web service API methods through annotated controllers or interfaces. The client supports asynchronous calls, retry policies and logging. All this is  simple and flexible to configure.

## Usage with Asp.Net Core
### Step 1: Create controller
```C#
[ApiController, Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public async Task<WeatherForecast> GetAsync(DateTime date) =>
        new WeatherForecast(date: date, temperatureC: -25);
}
```
### Step 2: Extract interface for your controller and add `INClient` interface
```C#
public interface IWeatherForecastController : INClient
{
    Task<WeatherForecast> GetAsync(DateTime date);
}

[ApiController, Route("[controller]")]
public class WeatherForecastController : ControllerBase, IWeatherForecastController { ... }
```
### Step 3: Create client
```C#
var client = new AspNetClientProvider()
    .Use<IWeatherForecastController, WeatherForecastController>(host: new Uri("http://localhost:8080"))
    .SetDefaultHttpClientProvider()
    .WithoutResiliencePolicy()
    .Build();
```
### Step 4: Send an http request
```
var forecast = await client.GetAsync(DateTime.Now);
Console.WriteLine($"Date {forecast.Date}: {forecast.TemperatureC}°C");
```
## Usage with non .Net web service
### Step 1: Create interface for your service and add `INClient` interface
```C#
[Api(template: "api")]
public interface IProductServiceClient : INClient
{
    [AsHttpGet(template: "products")]
    Task PostAsync(Product product);
}
```
### Step 2: Create client
```C#
var client = new ClientProvider()
    .Use<IProductServiceClient>(host: new Uri("https://localhost:8080"))
    .SetDefaultHttpClientProvider()
    .WithoutResiliencePolicy()
    .Build();
```
### Step 3: Send an http request
```
await client.PostAsync(new Product(id: 1));
```
