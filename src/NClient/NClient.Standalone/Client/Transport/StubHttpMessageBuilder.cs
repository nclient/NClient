using System.Threading.Tasks;
using NClient.Abstractions.Providers.Transport;

namespace NClient.Standalone.Client.Transport
{
    internal class StubHttpMessageBuilder : IHttpMessageBuilder<IHttpRequest, IHttpResponse>
    {
        public Task<IHttpRequest> BuildRequestAsync(IHttpRequest httpRequest)
        {
            return Task.FromResult(httpRequest);
        }
        public Task<IHttpResponse> BuildResponseAsync(IHttpRequest httpRequest, IHttpRequest request, IHttpResponse response)
        {
            return Task.FromResult(response);
        }
    }
}
