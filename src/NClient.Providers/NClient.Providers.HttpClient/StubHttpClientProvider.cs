using NClient.Providers.HttpClient.Abstractions;

namespace NClient.Providers.HttpClient
{
    public class StubHttpClientProvider : IHttpClientProvider
    {
        public IHttpClient Create()
        {
            return new StubHttpClient();
        }
    }
}
