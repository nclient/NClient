using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NClient.Providers.Transport.Http.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.Transport.Http.RestSharp
{
    internal class RestSharpTransportRequestBuilder : ITransportRequestBuilder<IRestRequest, IRestResponse>
    {
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;
        private readonly IToolSet _toolSet;

        public RestSharpTransportRequestBuilder(
            IRestSharpMethodMapper restSharpMethodMapper,
            IToolSet toolSet)
        {
            _restSharpMethodMapper = restSharpMethodMapper;
            _toolSet = toolSet;
        }

        public Task<IRestRequest> BuildAsync(IRequest request)
        {
            var method = _restSharpMethodMapper.Map(request.Type);
            var restRequest = new RestRequest(request.Endpoint, method, DataFormat.Json);

            foreach (var param in request.Parameters)
            {
                restRequest.AddParameter(param.Name, param.Value!, ParameterType.QueryString);
            }

            restRequest.AddHeader(HttpKnownHeaderNames.Accept, MediaTypeWithQualityHeaderValue.Parse(_toolSet.Serializer.ContentType).ToString());
            
            foreach (var metadata in request.Metadatas.SelectMany(x => x.Value))
            {
                restRequest.AddHeader(metadata.Name, metadata.Value);
            }

            if (request.Content is not null)
            {
                restRequest.AddParameter(_toolSet.Serializer.ContentType, request.Content.Bytes, ParameterType.RequestBody);

                foreach (var metadata in request.Content.Metadatas.SelectMany(x => x.Value))
                {
                    restRequest.AddHeader(metadata.Name, metadata.Value);
                }
            }

            return Task.FromResult((IRestRequest) restRequest);
        }
    }
}
