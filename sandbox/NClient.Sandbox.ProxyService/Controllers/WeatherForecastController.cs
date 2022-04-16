using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            _logger.LogInformation("Forecast with an id '{Id}' and date '{Date}' was requested", weatherForecastFilter.Id, weatherForecastFilter.Date);

            if (weatherForecastFilter.Id < 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            if (weatherForecastFilter.Id == 0)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return Task.FromResult(_thirdPartyWeatherForecastClient.Get().First());
        }

        public Task PostAsync(WeatherForecastDto weatherForecastDto)
        {
            _logger.LogInformation("Weather forecast with id '{Id}' was inserted (not really)", weatherForecastDto.Id);
            return Task.CompletedTask;
        }

        public Task PutAsync(WeatherForecastDto weatherForecastDto)
        {
            _logger.LogInformation("Weather forecast with id '{Id}' was updated (not really)", weatherForecastDto.Id);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int? id = null)
        {
            _logger.LogInformation("Weather forecast with id '{Id}' was deleted (not really)", id);
            return Task.CompletedTask;
        }
    }
}
