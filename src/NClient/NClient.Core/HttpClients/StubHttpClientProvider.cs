using NClient.Abstractions.HttpClients;

namespace NClient.Core.HttpClients
{
    public class StubHttpClientProvider : IHttpClientProvider
    {
        public IHttpClient Create()
        {
            return new StubHttpClient();
        }
    }
}
