using System;
using System.Text.Json.Serialization;
using NClient.Annotations.Http;

namespace NClient.Sandbox.ProxyService.Facade.Dto
{
    public class WeatherForecastFilter
    {
        [QueryParam(Name = "id")]
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
    }
}
