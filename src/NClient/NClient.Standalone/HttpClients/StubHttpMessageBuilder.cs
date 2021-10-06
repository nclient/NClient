using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;

namespace NClient.Standalone.HttpClients
{
    public class StubHttpMessageBuilder : IHttpMessageBuilder<HttpRequest, HttpResponse>
    {
        public Task<HttpRequest> BuildRequestAsync(HttpRequest httpRequest)
        {
            return Task.FromResult(httpRequest);
        }
        public Task<HttpResponse> BuildResponseAsync(HttpRequest httpRequest, HttpResponse response)
        {
            return Task.FromResult(response);
        }
    }
}
