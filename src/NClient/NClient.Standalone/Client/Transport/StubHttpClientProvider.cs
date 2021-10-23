using NClient.Abstractions.Providers.Serialization;
using NClient.Abstractions.Providers.Transport;

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
