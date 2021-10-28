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
        
        public ITransportMessageBuilder<HttpRequestMessage, HttpResponseMessage> Create(ISerializer _)
        {
            return new SystemHttpTransportMessageBuilder(
                _systemHttpMethodMapper, 
                new FinalHttpRequestBuilder(_systemHttpMethodMapper));
        }
    }
}
