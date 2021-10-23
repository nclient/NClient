using System.Net.Http;
using NClient.Providers.Serialization;
using NClient.Providers.Transport.Http.System.Builders;

namespace NClient.Providers.Transport.Http.System
{
    public class SystemHttpMessageBuilderProvider : IHttpMessageBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        public IHttpMessageBuilder<HttpRequestMessage, HttpResponseMessage> Create(ISerializer serializer)
        {
            return new SystemHttpMessageBuilder(serializer, new FinalHttpRequestBuilder(serializer));
        }
    }
}
