using System.Net.Http;
using NClient.Providers.Serialization;
using NClient.Providers.Transport.Http.System.Builders;
using NClient.Providers.Transport.Http.System.Helpers;

namespace NClient.Providers.Transport.Http.System
{
    public class SystemHttpResponseBuilderProvider : IResponseBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly ISystemHttpMethodMapper _systemHttpMethodMapper;
        
        public SystemHttpResponseBuilderProvider()
        {
            _systemHttpMethodMapper = new SystemHttpMethodMapper();
        }
        
        public IResponseBuilder<HttpRequestMessage, HttpResponseMessage> Create(ISerializer _)
        {
            return new SystemHttpResponseBuilder(new FinalHttpRequestBuilder(_systemHttpMethodMapper));
        }
    }
}
