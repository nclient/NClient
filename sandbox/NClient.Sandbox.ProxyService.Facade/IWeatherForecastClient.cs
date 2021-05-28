using NClient.Annotations.Versioning;

namespace NClient.Sandbox.ProxyService.Facade
{
    [UseVersion("3.0")]
    public interface IWeatherForecastClient : IWeatherForecastController
    {
    }
}
