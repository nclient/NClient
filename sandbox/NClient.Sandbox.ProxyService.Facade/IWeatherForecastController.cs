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
    [XVersion("1.0"), XVersion("2.0"), XVersion("3.0")]
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

        [DeleteMethod]
        [XUseVersion("2.0"), XUseVersion("3.0")]
        Task Delete(int? id = null);
    }
}
