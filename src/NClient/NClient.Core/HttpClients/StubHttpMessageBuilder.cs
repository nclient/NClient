using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;

namespace NClient.Core.HttpClients
{
    public class StubHttpMessageBuilder : IHttpMessageBuilder<HttpRequest, HttpResponse>
    {
        public Task<HttpRequest> BuildRequestAsync(HttpRequest request)
        {
            return Task.FromResult(request);
        }
        public Task<HttpResponse> BuildResponseAsync(HttpRequest request, HttpResponse customResponse)
        {
            return Task.FromResult(customResponse);
        }
    }
}
