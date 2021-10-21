using System.Net.Http;
using NClient.Abstractions.Providers.HttpClient;
using NClient.Abstractions.Providers.Serialization;
using NClient.Providers.HttpClient.System.Builders;

namespace NClient.Providers.HttpClient.System
{
    public class SystemHttpMessageBuilderProvider : IHttpMessageBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        public IHttpMessageBuilder<HttpRequestMessage, HttpResponseMessage> Create(ISerializer serializer)
        {
            return new SystemHttpMessageBuilder(serializer, new FinalHttpRequestBuilder(serializer));
        }
    }
}
