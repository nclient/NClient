using System.Net.Http;
using Flurl.Http.Configuration;
using RichardSzalay.MockHttp;

namespace NClient.Benchmark.Client.Clients.Flurl
{
    public class FlurlHttpClientFactory : DefaultHttpClientFactory
    {
        private readonly MockHttpMessageHandler _mockHttpMessageHandler;
        
        public FlurlHttpClientFactory(MockHttpMessageHandler mockHttpMessageHandler)
        {
            _mockHttpMessageHandler = mockHttpMessageHandler;
        }
        
        public override HttpMessageHandler CreateMessageHandler()
        {
            return _mockHttpMessageHandler;
        }
    }
}
