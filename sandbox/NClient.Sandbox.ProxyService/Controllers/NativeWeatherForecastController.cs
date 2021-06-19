using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NClient.AspNetCore.Exceptions;
using NClient.Sandbox.ProxyService.Clients;
using NClient.Sandbox.ProxyService.Facade.Dto;

namespace NClient.Sandbox.ProxyService.Controllers
{
    [ApiController]
    [ApiVersion("1.0"), ApiVersion("2.0"), ApiVersion("3.0")]
    [Route("api/native/v{version:apiVersion}/weatherForecast")]
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
        [ProducesResponseType(typeof(WeatherForecastDto), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public Task<WeatherForecastDto> GetAsync([FromQuery(Name = "filter")] WeatherForecastFilter weatherForecastFilter)
        {
            if (weatherForecastFilter.Id < 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            if (weatherForecastFilter.Id == 0)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            
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

        [HttpDelete]
        [MapToApiVersion("2.0"), MapToApiVersion("3.0")]
        public Task DeleteAsync(int? id = null)
        {
            _logger.LogInformation($"Weather forecast with id '{id}' was deleted (not really).");
            return Task.FromResult(0);
        }
    }
}
