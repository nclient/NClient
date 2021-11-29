using System.Net.Http;
using NClient.Providers.Transport.SystemNetHttp.Builders;
using NClient.Providers.Transport.SystemNetHttp.Helpers;

namespace NClient.Providers.Transport.SystemNetHttp
{
    public class SystemNetHttpResponseBuilderProvider : IResponseBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly ISystemNetHttpMethodMapper _systemNetHttpMethodMapper;
        
        public SystemNetHttpResponseBuilderProvider()
        {
            _systemNetHttpMethodMapper = new SystemNetHttpMethodMapper();
        }
        
        public IResponseBuilder<HttpRequestMessage, HttpResponseMessage> Create(IToolset toolset)
        {
            return new SystemNetHttpResponseBuilder(new FinalHttpRequestBuilder(_systemNetHttpMethodMapper));
        }
    }
}
