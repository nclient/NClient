using NClient.Abstractions.HttpClients;

namespace NClient.Core.HttpClients
{
    internal class StubHttpClientProvider : IHttpClientProvider
    {
        public IHttpClient Create()
        {
            return new StubHttpClient();
        }
    }
}
