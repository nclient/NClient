using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Sandbox.ProxyService.Facade.Dto;

namespace NClient.Sandbox.ProxyService.Facade
{
    [Path("[controller]")]
    public interface IWeatherForecastController
    {
        [GetMethod]
        Task<WeatherForecastDto> GetAsync([QueryParam(Name = "forecastId")] int id);
    }
}
