using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NClient.Annotations;
using NClient.AspNetCore.Exceptions;
using NClient.Sandbox.ProxyService.Clients;
using NClient.Sandbox.ProxyService.Facade;
using NClient.Sandbox.ProxyService.Facade.Dto;

namespace NClient.Sandbox.ProxyService.Controllers
{
    [ApiController, Route("api/ignored/[controller]")] // Must be ignored
    public class WeatherForecastController : ControllerBase, IWeatherForecastController
    {
        private readonly IThirdPartyWeatherForecastClient _thirdPartyWeatherForecastClient;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            IThirdPartyWeatherForecastClient thirdPartyWeatherForecastClient,
            ILogger<WeatherForecastController> logger)
        {
            _thirdPartyWeatherForecastClient = thirdPartyWeatherForecastClient;
            _logger = logger;
        }

        public Task<WeatherForecastDto> GetAsync(WeatherForecastFilter weatherForecastFilter)
        {
            if (weatherForecastFilter.Id < 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            _logger.LogInformation($"Forecast with an id '{weatherForecastFilter.Id}' and date '{weatherForecastFilter.Date}' was requested.");
            return Task.FromResult(_thirdPartyWeatherForecastClient.Get().First());
        }

        public Task PostAsync(WeatherForecastDto weatherForecastDto)
        {
            _logger.LogInformation($"Weather forecast with id '{weatherForecastDto.Id}' was inserted (not really).");
            return Task.FromResult(0);
        }

        public Task PutAsync(WeatherForecastDto weatherForecastDto)
        {
            _logger.LogInformation($"Weather forecast with id '{weatherForecastDto.Id}' was updated (not really).");
            return Task.FromResult(0);
        }

        public Task DeleteAsync(int? id = null)
        {
            _logger.LogInformation($"Weather forecast with id '{id}' was deleted (not really).");
            return Task.FromResult(0);
        }
    }
}
