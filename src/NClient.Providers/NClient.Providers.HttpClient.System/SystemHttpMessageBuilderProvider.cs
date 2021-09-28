using System.Net.Http;
using NClient.Abstractions.Exceptions.Factories;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Providers.HttpClient.System.Builders;

namespace NClient.Providers.HttpClient.System
{
    public class SystemHttpMessageBuilderProvider : IHttpMessageBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly IClientHttpRequestExceptionFactory _clientHttpRequestExceptionFactory;
        
        public SystemHttpMessageBuilderProvider()
        {
            _clientHttpRequestExceptionFactory = new ClientHttpRequestExceptionFactory();
        }
        
        public IHttpMessageBuilder<HttpRequestMessage, HttpResponseMessage> Create(ISerializer serializer)
        {
            return new SystemHttpMessageBuilder(
                serializer,
                new FinalHttpRequestBuilder(serializer),
                _clientHttpRequestExceptionFactory);
        }
    }
}
