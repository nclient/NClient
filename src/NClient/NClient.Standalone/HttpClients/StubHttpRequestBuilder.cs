using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Providers.Results.HttpMessages;

namespace NClient.Standalone.HttpClients
{
    public class StubHttpRequestBuilder : IHttpMessageBuilder<IHttpRequest, IHttpResponse>
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
