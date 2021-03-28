using System.Collections.Generic;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Sandbox.ProxyService.Facade.Dto;

namespace NClient.Sandbox.ProxyService.Facade
{
    [Path("[controller]")]
    public interface IWeatherForecastController
    {
        [GetMethod]
        IEnumerable<WeatherForecastDto> Get();
    }
}
