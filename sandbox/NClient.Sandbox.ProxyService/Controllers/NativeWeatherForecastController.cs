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

        [HttpGet("{filter.id}")]
        public Task<WeatherForecastDto> GetAsync([FromQuery(Name = "filter")] WeatherForecastFilter weatherForecastFilter)
        {
            _logger.LogInformation($"Forecast with an id '{weatherForecastFilter.Id}' and date '{weatherForecastFilter.Date}' was requested.");
            return Task.FromResult(_thirdPartyWeatherForecastClient.Get().First());
        }

        [HttpPost]
        public Task PostAsync(WeatherForecastDto weatherForecastDto)
        {
            _logger.LogInformation($"Weather forecast with id '{weatherForecastDto.Id}' was saved (not really).");
            return Task.FromResult(0);
        }
        
        [HttpPut("{weatherForecastDto.id}")]
        public Task PutAsync(WeatherForecastDto weatherForecastDto)
        {
            _logger.LogInformation($"Weather forecast with id '{weatherForecastDto.Id}' was saved (not really).");
            return Task.FromResult(0);
        }
    }
}
