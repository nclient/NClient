using System.Net.Http;
using NClient.Providers.Transport.SystemNetHttp.Builders;
using NClient.Providers.Transport.SystemNetHttp.Helpers;

namespace NClient.Providers.Transport.SystemNetHttp
{
    public class SystemHttpResponseBuilderProvider : IResponseBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly ISystemHttpMethodMapper _systemHttpMethodMapper;
        
        public SystemHttpResponseBuilderProvider()
        {
            _systemHttpMethodMapper = new SystemHttpMethodMapper();
        }
        
        public IResponseBuilder<HttpRequestMessage, HttpResponseMessage> Create(IToolset toolset)
        {
            return new SystemHttpResponseBuilder(new FinalHttpRequestBuilder(_systemHttpMethodMapper));
        }
    }
}
