using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NClient.Sandbox.ProxyService.Clients;
using NClient.Sandbox.ProxyService.Facade.Dto;

namespace NClient.Sandbox.ProxyService.Controllers
{
    [ApiController]
    [Route("api/native/weatherForecast")]
    public class NativeWeatherForecastController : ControllerBase
    {
        private readonly IThirdPartyWeatherForecastClient _thirdPartyWeatherForecastClient;
        private readonly ILogger<NativeWeatherForecastController> _logger;

        public NativeWeatherForecastController(
            IThirdPartyWeatherForecastClient thirdPartyWeatherForecastClient, 
            ILogger<NativeWeatherForecastController> logger)
        {
            _thirdPartyWeatherForecastClient = thirdPartyWeatherForecastClient;
            _logger = logger;
        }

        public Task<WeatherForecastDto> GetAsync([FromQuery(Name = "forecastId")] int id)
        {
            return Task.FromResult(_thirdPartyWeatherForecastClient.Get().First());
        }
    }
}
