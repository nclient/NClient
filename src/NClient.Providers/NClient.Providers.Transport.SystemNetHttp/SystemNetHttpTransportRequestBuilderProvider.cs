using System.Net.Http;
using NClient.Providers.Transport.Common;
using NClient.Providers.Transport.SystemNetHttp.Helpers;

namespace NClient.Providers.Transport.SystemNetHttp
{
    /// <summary>The provider that can create builder for transforming a NClient request to System.Net.Http request.</summary>
    public class SystemNetHttpTransportRequestBuilderProvider : ITransportRequestBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly ISystemNetHttpMethodMapper _systemNetHttpMethodMapper;

        /// <summary>Creates provider that can create builder for transforming a NClient request to System.Net.Http request.</summary>
        public SystemNetHttpTransportRequestBuilderProvider()
        {
            _systemNetHttpMethodMapper = new SystemNetHttpMethodMapper();
        }
        
        /// <summary>Creates builder for transforming a NClient request to System.Net.Http request.</summary>
        public ITransportRequestBuilder<HttpRequestMessage, HttpResponseMessage> Create(IToolset toolset)
        {
            return new SystemNetHttpTransportRequestBuilder(_systemNetHttpMethodMapper);
        }

        public ITransportRequestBuilder<HttpRequestMessage, HttpResponseMessage> Create(IToolset toolset, IPipelineCanceller pipelineCanceller)
        {
            return new SystemNetHttpTransportRequestBuilder(_systemNetHttpMethodMapper);
        }
    }
}
