using NClient.Providers.Serialization;
using NClient.Providers.Transport.Http.RestSharp.Builders;
using NClient.Providers.Transport.Http.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.Transport.Http.RestSharp
{
    public class RestSharpResponseBuilderProvider : IResponseBuilderProvider<IRestRequest, IRestResponse>
    {
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;

        public RestSharpResponseBuilderProvider()
        {
            _restSharpMethodMapper = new RestSharpMethodMapper();
        }
        
        public IResponseBuilder<IRestRequest, IRestResponse> Create(ISerializer serializer)
        {
            return new RestSharpResponseBuilder(new FinalHttpRequestBuilder(_restSharpMethodMapper));
        }
    }
}
