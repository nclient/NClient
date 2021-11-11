using System.Collections.Generic;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Sandbox.ProxyService.Facade.Dto;

namespace NClient.Sandbox.ProxyService.Clients
{
    [Path("WeatherForecast")]
    public interface IThirdPartyWeatherForecastClient
    {
        [GetMethod]
        IEnumerable<WeatherForecastDto> Get();
    }
}
