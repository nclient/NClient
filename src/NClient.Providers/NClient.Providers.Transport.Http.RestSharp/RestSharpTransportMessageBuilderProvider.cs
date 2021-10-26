using NClient.Providers.Serialization;
using NClient.Providers.Transport.Http.RestSharp.Builders;
using NClient.Providers.Transport.Http.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.Transport.Http.RestSharp
{
    public class RestSharpTransportMessageBuilderProvider : ITransportMessageBuilderProvider<IRestRequest, IRestResponse>
    {
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;

        public RestSharpTransportMessageBuilderProvider()
        {
            _restSharpMethodMapper = new RestSharpMethodMapper();
        }
        
        public ITransportMessageBuilder<IRestRequest, IRestResponse> Create(ISerializer serializer)
        {
            return new RestSharpTransportMessageBuilder(
                serializer,
                _restSharpMethodMapper,
                new FinalHttpRequestBuilder(_restSharpMethodMapper));
        }
    }
}
