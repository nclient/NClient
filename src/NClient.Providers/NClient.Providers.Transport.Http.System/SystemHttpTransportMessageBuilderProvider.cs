using System.Net.Http;
using NClient.Providers.Serialization;
using NClient.Providers.Transport.Http.System.Builders;
using NClient.Providers.Transport.Http.System.Helpers;

namespace NClient.Providers.Transport.Http.System
{
    public class SystemHttpTransportMessageBuilderProvider : ITransportMessageBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly ISystemHttpMethodMapper _systemHttpMethodMapper;
        
        public SystemHttpTransportMessageBuilderProvider()
        {
            _systemHttpMethodMapper = new SystemHttpMethodMapper();
        }
        
        public ITransportMessageBuilder<HttpRequestMessage, HttpResponseMessage> Create(ISerializer serializer)
        {
            return new SystemHttpTransportMessageBuilder(
                serializer, 
                _systemHttpMethodMapper, 
                new FinalHttpRequestBuilder(serializer, _systemHttpMethodMapper));
        }
    }
}
