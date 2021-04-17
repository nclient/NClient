using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Sandbox.ProxyService.Facade.Dto;

namespace NClient.Sandbox.ProxyService.Facade
{
    [Api]
    [Path("api/nclient/[controller]")]
    public interface IWeatherForecastController
    {
        [GetMethod("{filter.id}")]
        Task<WeatherForecastDto> GetAsync([QueryParam(Name = "filter")] WeatherForecastFilter weatherForecastFilter);

        [PostMethod]
        Task PostAsync(WeatherForecastDto weatherForecastDto);
        
        [PutMethod("{weatherForecastDto.id}")]
        Task PutAsync(WeatherForecastDto weatherForecastDto);
    }
}
