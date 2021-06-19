using System.Net;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Invocation;
using NClient.Core.Handling;
using NClient.Sandbox.ProxyService.Facade;

namespace NClient.Sandbox.Client.ClientHandlers
{
    public class WeatherForecastClientHandler : DefaultClientHandler
    {
        public override Task<HttpRequest> HandleRequestAsync(HttpRequest httpRequest, MethodInvocation methodInvocation)
        {
            return Task.FromResult(httpRequest);
        }

        public override Task<HttpResponse> HandleResponseAsync(HttpResponse httpResponse, MethodInvocation methodInvocation)
        {
            if (methodInvocation.MethodInfo.Name == nameof(IWeatherForecastClient.GetAsync) && httpResponse.StatusCode == HttpStatusCode.NotFound)
                return Task.FromResult(httpResponse);
            return base.HandleResponseAsync(httpResponse, methodInvocation);
        }
    }
}