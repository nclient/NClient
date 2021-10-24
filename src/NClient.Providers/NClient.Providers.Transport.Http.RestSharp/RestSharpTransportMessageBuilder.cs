using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NClient.Providers.Serialization;
using NClient.Providers.Transport.Http.RestSharp.Builders;
using NClient.Providers.Transport.Http.RestSharp.Helpers;
using ParameterType = RestSharp.ParameterType;
using DataFormat = RestSharp.DataFormat;
using RestRequest = RestSharp.RestRequest;
using IRestRequest = RestSharp.IRestRequest;
using IRestResponse = RestSharp.IRestResponse;

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
            // TODO: как быть?
            var method = _restSharpMethodMapper.Map(request.Method.Value);
            var restRequest = new RestRequest(request.Resource, method, DataFormat.Json);

            foreach (var param in request.Parameters)
            {
                restRequest.AddParameter(param.Name, param.Value!, ParameterType.QueryString);
            }

            restRequest.AddHeader(HttpKnownHeaderNames.Accept, MediaTypeWithQualityHeaderValue.Parse(_serializer.ContentType).ToString());
            
            foreach (var header in request.Headers)
            {
                restRequest.AddHeader(header.Name, header.Value);
            }

            if (request.Data is not null)
            {
                var body = _serializer.Serialize(request.Data);
                restRequest.AddParameter(_serializer.ContentType, body, ParameterType.RequestBody);
            }

            return Task.FromResult((IRestRequest)restRequest);
        }
        
        public Task<IResponse> BuildResponseAsync(IRequest transportRequest, IRestRequest restRequest, IRestResponse restResponse)
        {
            var finalRequest = _finalHttpRequestBuilder.Build(transportRequest, restResponse.Request);
            
            var allHeaders = restResponse.Headers
                .Where(x => x.Name != null)
                .Select(x => new Header(x.Name!, x.Value?.ToString() ?? ""))
                .ToArray();
            var responseHeaders = allHeaders
                .Where(x => !ContentHeaderNames.Contains(x.Name))
                .ToArray();
            var contentHeaders = allHeaders
                .Where(x => ContentHeaderNames.Contains(x.Name))
                .ToArray();
            
            var response
                = new Response(finalRequest)
            {
                Content = new Content(restResponse.RawBytes, new ContentHeaderContainer(contentHeaders)),
                StatusCode = (int)restResponse.StatusCode,
                ResponseUri = restResponse.ResponseUri,
                Headers = new HeaderContainer(responseHeaders),
                ErrorMessage = restResponse.ErrorMessage,
                ErrorException = restResponse.ErrorException,
                ProtocolVersion = restResponse.ProtocolVersion,
                IsSuccessful = restResponse.IsSuccessful
            };

            return Task.FromResult<IResponse>(response);
        }
    }
}
