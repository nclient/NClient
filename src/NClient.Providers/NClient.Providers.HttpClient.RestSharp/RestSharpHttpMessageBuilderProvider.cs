using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Providers.HttpClient.RestSharp.Builders;
using NClient.Providers.HttpClient.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.HttpClient.RestSharp
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
