using NClient.Annotations;
using NClient.Annotations.Versioning;

namespace NClient.Sandbox.ProxyService.Facade
{
    [UseVersion("3.0")]
    [Header("client", "NClient")]
    public interface IWeatherForecastClient : IWeatherForecastController
    {
    }
}
