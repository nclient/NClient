using System.Net.Http;
using NClient.Providers.Transport.Http.System.Helpers;

namespace NClient.Providers.Transport.Http.System
{
    public class SystemHttpTransportRequestBuilderProvider : ITransportRequestBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly ISystemHttpMethodMapper _systemHttpMethodMapper;
        
        public SystemHttpTransportRequestBuilderProvider()
        {
            _systemHttpMethodMapper = new SystemHttpMethodMapper();
        }
        
        public ITransportRequestBuilder<HttpRequestMessage, HttpResponseMessage> Create(IToolset toolset)
        {
            return new SystemHttpTransportRequestBuilder(_systemHttpMethodMapper);
        }
    }
}
