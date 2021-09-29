using System.Net.Http;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Providers.HttpClient.System.Builders;

namespace NClient.Providers.HttpClient.System
{
    public class SystemHttpMessageBuilderProvider : IHttpMessageBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly ISystemHttpClientExceptionFactory _httpClientExceptionFactory;
        public SystemHttpMessageBuilderProvider()
        {
            _httpClientExceptionFactory = new SystemHttpClientExceptionFactory();
        }
        
        public IHttpMessageBuilder<HttpRequestMessage, HttpResponseMessage> Create(ISerializer serializer)
        {
            return new SystemHttpMessageBuilder(
                serializer,
                new FinalHttpRequestBuilder(serializer),
                _httpClientExceptionFactory);
        }
    }
}
