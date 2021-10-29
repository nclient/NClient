using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NClient.Providers.Resilience;
using NClient.Providers.Serialization;
using NClient.Providers.Transport.Http.RestSharp.Builders;
using NClient.Providers.Transport.Http.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.Transport.Http.RestSharp
{
    internal class RestSharpTransportMessageBuilder : ITransportMessageBuilder<IRestRequest, IRestResponse>
    {
        private static readonly HashSet<string> ContentHeaderNames = new(new HttpContentKnownHeaderNames());

        private readonly ISerializer _serializer;
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;
        private readonly IFinalHttpRequestBuilder _finalHttpRequestBuilder;

        public RestSharpTransportMessageBuilder(
            ISerializer serializer,
            IRestSharpMethodMapper restSharpMethodMapper,
            IFinalHttpRequestBuilder finalHttpRequestBuilder)
        {
            _serializer = serializer;
            _restSharpMethodMapper = restSharpMethodMapper;
            _finalHttpRequestBuilder = finalHttpRequestBuilder;
        }

        public Task<IRestRequest> BuildTransportRequestAsync(IRequest request)
        {
            var method = _restSharpMethodMapper.Map(request.Type);
            var restRequest = new RestRequest(request.Endpoint, method, DataFormat.Json);

            foreach (var param in request.Parameters)
            {
                restRequest.AddParameter(param.Name, param.Value!, ParameterType.QueryString);
            }

            restRequest.AddHeader(HttpKnownHeaderNames.Accept, MediaTypeWithQualityHeaderValue.Parse(_serializer.ContentType).ToString());
            
            foreach (var metadata in request.Metadatas.SelectMany(x => x.Value))
            {
                restRequest.AddHeader(metadata.Name, metadata.Value);
            }

            if (request.Content is not null)
            {
                restRequest.AddParameter(_serializer.ContentType, request.Content.Bytes, ParameterType.RequestBody);

                foreach (var metadata in request.Content.Metadatas.SelectMany(x => x.Value))
                {
                    restRequest.AddHeader(metadata.Name, metadata.Value);
                }
            }

            return Task.FromResult((IRestRequest)restRequest);
        }
        
        public Task<IResponse> BuildResponseAsync(IRequest request, IResponseContext<IRestRequest, IRestResponse> responseContext)
        {
            var finalRequest = _finalHttpRequestBuilder.Build(request, responseContext.Response.Request);
            
            var allHeaders = responseContext.Response.Headers
                .Where(x => x.Name != null)
                .Select(x => new Metadata(x.Name!, x.Value?.ToString() ?? ""))
                .ToArray();
            var responseHeaders = allHeaders
                .Where(x => !ContentHeaderNames.Contains(x.Name))
                .ToArray();
            var contentHeaders = allHeaders
                .Where(x => ContentHeaderNames.Contains(x.Name))
                .ToArray();
            
            var response = new Response(finalRequest)
            {
                Content = new Content(responseContext.Response.RawBytes, responseContext.Response.ContentEncoding, new MetadataContainer(contentHeaders)),
                StatusCode = (int)responseContext.Response.StatusCode,
                Endpoint = responseContext.Response.ResponseUri.ToString(),
                Metadatas = new MetadataContainer(responseHeaders),
                ErrorMessage = responseContext.Response.ErrorMessage,
                ErrorException = responseContext.Response.ErrorException,
                ProtocolVersion = responseContext.Response.ProtocolVersion,
                IsSuccessful = responseContext.Response.IsSuccessful
            };

            return Task.FromResult<IResponse>(response);
        }
    }
}
