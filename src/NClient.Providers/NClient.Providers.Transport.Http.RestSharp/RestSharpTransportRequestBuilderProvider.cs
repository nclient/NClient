using NClient.Providers.Transport.Http.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.Transport.Http.RestSharp
{
    public class RestSharpTransportRequestBuilderProvider : ITransportRequestBuilderProvider<IRestRequest, IRestResponse>
    {
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;

        public RestSharpTransportRequestBuilderProvider()
        {
            _restSharpMethodMapper = new RestSharpMethodMapper();
        }
        
        public ITransportRequestBuilder<IRestRequest, IRestResponse> Create(IToolSet toolSet)
        {
            return new RestSharpTransportRequestBuilder(_restSharpMethodMapper, toolSet);
        }
    }
}
