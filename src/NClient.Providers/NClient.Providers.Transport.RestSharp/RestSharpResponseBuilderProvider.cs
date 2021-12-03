using NClient.Providers.Transport.RestSharp.Builders;
using NClient.Providers.Transport.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.Transport.RestSharp
{
    public class RestSharpResponseBuilderProvider : IResponseBuilderProvider<IRestRequest, IRestResponse>
    {
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;

        public RestSharpResponseBuilderProvider()
        {
            _restSharpMethodMapper = new RestSharpMethodMapper();
        }
        
        public IResponseBuilder<IRestRequest, IRestResponse> Create(IToolset toolset)
        {
            return new RestSharpResponseBuilder(new FinalHttpRequestBuilder(_restSharpMethodMapper));
        }
    }
}
