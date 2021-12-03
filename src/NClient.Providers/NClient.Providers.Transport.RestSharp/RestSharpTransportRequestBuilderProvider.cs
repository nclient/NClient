using NClient.Providers.Transport.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.Transport.RestSharp
{
    public class RestSharpTransportRequestBuilderProvider : ITransportRequestBuilderProvider<IRestRequest, IRestResponse>
    {
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;

        public RestSharpTransportRequestBuilderProvider()
        {
            _restSharpMethodMapper = new RestSharpMethodMapper();
        }
        
        public ITransportRequestBuilder<IRestRequest, IRestResponse> Create(IToolset toolset)
        {
            return new RestSharpTransportRequestBuilder(_restSharpMethodMapper, toolset);
        }
    }
}
