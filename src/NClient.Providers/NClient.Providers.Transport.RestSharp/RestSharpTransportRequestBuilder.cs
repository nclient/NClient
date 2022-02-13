using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Transport.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.Transport.RestSharp
{
    internal class RestSharpTransportRequestBuilder : ITransportRequestBuilder<IRestRequest, IRestResponse>
    {
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;
        private readonly IToolset _toolset;

        public RestSharpTransportRequestBuilder(
            IRestSharpMethodMapper restSharpMethodMapper,
            IToolset toolset)
        {
            _restSharpMethodMapper = restSharpMethodMapper;
            _toolset = toolset;
        }

        public async Task<IRestRequest> BuildAsync(IRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var method = _restSharpMethodMapper.Map(request.Type);
            var restRequest = new RestRequest(request.Endpoint, method, DataFormat.Json);

            foreach (var param in request.Parameters)
            {
                restRequest.AddParameter(param.Name, param.Value!, ParameterType.QueryString);
            }

            restRequest.AddHeader(HttpKnownHeaderNames.Accept, MediaTypeWithQualityHeaderValue.Parse(_toolset.Serializer.ContentType).ToString());
            
            foreach (var metadata in request.Metadatas.SelectMany(x => x.Value))
            {
                restRequest.AddHeader(metadata.Name, metadata.Value);
            }

            if (request.Content is not null)
            {
                var stringContent = await new StreamReader(request.Content.Stream, request.Content.Encoding).ReadToEndAsync().ConfigureAwait(false);
                restRequest.AddParameter(_toolset.Serializer.ContentType, stringContent, ParameterType.RequestBody);

                foreach (var metadata in request.Content.Metadatas.SelectMany(x => x.Value))
                {
                    restRequest.AddHeader(metadata.Name, metadata.Value);
                }
            }

            return restRequest;
        }
    }
}
