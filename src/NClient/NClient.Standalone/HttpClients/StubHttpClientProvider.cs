using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Standalone.HttpClients
{
    internal class StubHttpClientProvider : IHttpClientProvider<HttpRequest, HttpResponse>
    {
        public IHttpClient<HttpRequest, HttpResponse> Create(ISerializer? serializer = null)
        {
            return new StubHttpClient();
        }
    }
}
