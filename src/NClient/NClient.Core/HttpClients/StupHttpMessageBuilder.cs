using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;

namespace NClient.Core.HttpClients
{
    public class StupHttpMessageBuilder : IHttpMessageBuilder<HttpRequest, HttpResponse>
    {
        public Task<HttpRequest> BuildAsync(HttpRequest request)
        {
            return Task.FromResult(request);
        }
        public Task<HttpResponse> BuildAsync(HttpRequest request, HttpResponse customResponse)
        {
            return Task.FromResult(customResponse);
        }
    }
}
