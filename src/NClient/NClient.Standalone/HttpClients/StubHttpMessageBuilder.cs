using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;

namespace NClient.Standalone.HttpClients
{
    public class StubHttpMessageBuilder : IHttpMessageBuilder<IHttpRequest, IHttpResponse>
    {
        public Task<IHttpRequest> BuildRequestAsync(IHttpRequest httpRequest)
        {
            return Task.FromResult(httpRequest);
        }
        public Task<IHttpResponse> BuildResponseAsync(IHttpRequest httpRequest, IHttpResponse response)
        {
            return Task.FromResult(response);
        }
    }
}
