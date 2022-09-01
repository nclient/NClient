using NClient.Providers.Transport.RestSharp.Helpers;
using RestSharp;
using NClient.Providers.Transport.Common;

namespace NClient.Providers.Transport.RestSharp
{
    /// <summary>The provider that can create builder for transforming a NClient request to RestSharp request.</summary>
    public class RestSharpTransportRequestBuilderProvider : ITransportRequestBuilderProvider<IRestRequest, IRestResponse>
    {
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;

        /// <summary>Initializes provider that can create builder for transforming a NClient request to RestSharp request.</summary>
        public RestSharpTransportRequestBuilderProvider()
        {
            _restSharpMethodMapper = new RestSharpMethodMapper();
        }
        
        /// <summary>Creates builder for transforming a NClient request to RestSharp request.</summary>
        public ITransportRequestBuilder<IRestRequest, IRestResponse> Create(IToolset toolset)
        {
            return new RestSharpTransportRequestBuilder(_restSharpMethodMapper, toolset);
        }

        public ITransportRequestBuilder<IRestRequest, IRestResponse> Create(IToolset toolset, IPipelineCanceller pipelineCanceller)
        {
            return new RestSharpTransportRequestBuilder(_restSharpMethodMapper, toolset);
        }

        
    }
}
