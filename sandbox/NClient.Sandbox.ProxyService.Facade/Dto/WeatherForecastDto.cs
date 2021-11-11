using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace NClient.Sandbox.ProxyService.Facade.Dto
{
    public class WeatherForecastDto
    {
        [FromQuery(Name = "id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
        public string? Summary { get; set; }
    }
}
