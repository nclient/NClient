using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Standalone.HttpClients
{
    internal class StubHttpClientProvider : IHttpClientProvider<IHttpRequest, IHttpResponse>
    {
        public IHttpClient<IHttpRequest, IHttpResponse> Create(ISerializer? serializer = null)
        {
            return new StubHttpClient();
        }
    }
}
