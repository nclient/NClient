using NClient.Abstractions.Providers.HttpClient;
using NClient.Abstractions.Providers.Serialization;

namespace NClient.Standalone.Client.HttpClient
{
    internal class StubHttpClientProvider : IHttpClientProvider<IHttpRequest, IHttpResponse>
    {
        public IHttpClient<IHttpRequest, IHttpResponse> Create(ISerializer? serializer = null)
        {
            return new StubHttpClient();
        }
    }
}
