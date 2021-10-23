using System.Net.Http;
using NClient.Providers.Serialization;
using NClient.Providers.Transport.Http.System.Builders;

namespace NClient.Providers.Transport.Http.System
{
    public class SystemHttpTransportMessageBuilderProvider : ITransportMessageBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        public ITransportMessageBuilder<HttpRequestMessage, HttpResponseMessage> Create(ISerializer serializer)
        {
            return new SystemHttpTransportMessageBuilder(serializer, new FinalHttpRequestBuilder(serializer));
        }
    }
}
