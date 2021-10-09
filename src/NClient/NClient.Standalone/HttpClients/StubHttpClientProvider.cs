using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Providers.Results.HttpMessages;

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
