using NClient.Providers.Serialization;
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
        
        public ITransportRequestBuilder<IRestRequest, IRestResponse> Create(ISerializer serializer)
        {
            return new RestSharpTransportRequestBuilder(serializer, _restSharpMethodMapper);
        }
    }
}
