using NClient.Abstractions.Providers.HttpClient;
using NClient.Abstractions.Providers.Serialization;
using NClient.Providers.Transport.Http.RestSharp.Builders;
using NClient.Providers.Transport.Http.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.Transport.Http.RestSharp
{
    public class RestSharpHttpMessageBuilderProvider : IHttpMessageBuilderProvider<IRestRequest, IRestResponse>
    {
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;

        public RestSharpHttpMessageBuilderProvider()
        {
            _restSharpMethodMapper = new RestSharpMethodMapper();
        }
        
        public IHttpMessageBuilder<IRestRequest, IRestResponse> Create(ISerializer serializer)
        {
            return new RestSharpHttpMessageBuilder(
                serializer,
                _restSharpMethodMapper,
                new FinalHttpRequestBuilder(serializer, _restSharpMethodMapper));
        }
    }
}
