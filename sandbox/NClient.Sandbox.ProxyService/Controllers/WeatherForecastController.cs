﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NClient.Sandbox.ProxyService.Clients;
using NClient.Sandbox.ProxyService.Facade;
using NClient.Sandbox.ProxyService.Facade.Dto;

namespace NClient.Sandbox.ProxyService.Controllers
{
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

        public IEnumerable<WeatherForecastDto> Get()
        {
            return _thirdPartyWeatherForecastClient.Get();
        }
    }
}
