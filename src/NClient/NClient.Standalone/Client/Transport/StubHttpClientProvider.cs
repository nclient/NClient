using NClient.Providers.Serialization;
using NClient.Providers.Transport;

namespace NClient.Standalone.Client.Transport
{
    internal class StubHttpClientProvider : IHttpClientProvider<IHttpRequest, IHttpResponse>
    {
        public IHttpClient<IHttpRequest, IHttpResponse> Create(ISerializer? serializer = null)
        {
            return new StubHttpClient();
        }
    }
}
