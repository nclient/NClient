using System.Net;
using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Sandbox.ProxyService.Facade.Dto;

namespace NClient.Sandbox.ProxyService.Facade
{
    [Api]
    [Path("api/nclient/[controller]")]
    [Header("client", "NClient")]
    public interface IWeatherForecastController
    {
        [GetMethod("{filter.id}")]
        [Response(typeof(WeatherForecastDto), HttpStatusCode.OK)]
        [Response(typeof(void), HttpStatusCode.BadRequest)]
        Task<WeatherForecastDto> GetAsync([QueryParam(Name = "filter")] WeatherForecastFilter weatherForecastFilter);

        [PostMethod]
        Task PostAsync(WeatherForecastDto weatherForecastDto);

        [PutMethod("{weatherForecastDto.id}")]
        Task PutAsync(WeatherForecastDto weatherForecastDto);
    }
}
