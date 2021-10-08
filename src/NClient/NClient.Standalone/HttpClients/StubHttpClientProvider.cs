using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Standalone.HttpClients
{
    internal class StubHttpClientProvider : IHttpClientProvider<IHttpRequest, object>
    {
        public IHttpClient<IHttpRequest, object> Create(ISerializer? serializer = null)
        {
            return new StubHttpClient();
        }
    }
}
