using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Core.HttpClients
{
    internal class StubHttpClientProvider : IHttpClientProvider
    {
        public IHttpClient Create(ISerializer? serializer = null)
        {
            return new StubHttpClient();
        }
    }
}
