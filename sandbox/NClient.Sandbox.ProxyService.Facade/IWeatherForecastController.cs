using System.Net;
using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Annotations.Versioning;
using NClient.Sandbox.ProxyService.Facade.Dto;

namespace NClient.Sandbox.ProxyService.Facade
{
    [Api]
    [Path("api/nclient/v{version:apiVersion}/[controller]")]
    [Version("1.0"), Version("2.0"), Version("3.0")]
    public interface IWeatherForecastController
    {
        [GetMethod("{filter.id}")]
        [Response(typeof(WeatherForecastDto), HttpStatusCode.OK)]
        [Response(typeof(void), HttpStatusCode.BadRequest)]
        [Response(typeof(void), HttpStatusCode.NotFound)]
        Task<WeatherForecastDto> GetAsync([QueryParam(Name = "filter")] WeatherForecastFilter weatherForecastFilter);

        [PostMethod]
        Task PostAsync(WeatherForecastDto weatherForecastDto);

        [PutMethod("{weatherForecastDto.id}")]
        Task PutAsync(WeatherForecastDto weatherForecastDto);

        [DeleteMethod]
        [ToVersion("2.0"), ToVersion("3.0")]
        Task DeleteAsync(int? id = null);
    }
}
