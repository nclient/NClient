using System.Net.Http;
using NClient.Providers.Transport.SystemNetHttp.Helpers;

namespace NClient.Providers.Transport.SystemNetHttp
{
    public class SystemNetHttpTransportRequestBuilderProvider : ITransportRequestBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly ISystemNetHttpMethodMapper _systemNetHttpMethodMapper;
        
        public SystemNetHttpTransportRequestBuilderProvider()
        {
            _systemNetHttpMethodMapper = new SystemNetHttpMethodMapper();
        }
        
        public ITransportRequestBuilder<HttpRequestMessage, HttpResponseMessage> Create(IToolset toolset)
        {
            return new SystemNetHttpTransportRequestBuilder(_systemNetHttpMethodMapper);
        }
    }
}
