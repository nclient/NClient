using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NClient.Abstractions.Exceptions.Factories;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Providers.HttpClient.RestSharp.Helpers;
using RestSharp;
using HttpHeader = NClient.Abstractions.HttpClients.HttpHeader;
using HttpResponse = NClient.Abstractions.HttpClients.HttpResponse;

namespace NClient.Providers.HttpClient.RestSharp.Builders
{
    internal class HttpMessageBuilder : IHttpMessageBuilder<IRestRequest, IRestResponse>
    {
        private static readonly HashSet<string> ContentHeaderNames = new(new HttpContentKnownHeaderNames());

        private readonly ISerializer _serializer;
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;
        private readonly IFinalHttpRequestBuilder _finalHttpRequestBuilder;
        private readonly IClientHttpRequestExceptionFactory _clientHttpRequestExceptionFactory;

        public HttpMessageBuilder(
            ISerializer serializer,
            IRestSharpMethodMapper restSharpMethodMapper,
            IFinalHttpRequestBuilder finalHttpRequestBuilder,
            IClientHttpRequestExceptionFactory clientHttpRequestExceptionFactory)
        {
            _serializer = serializer;
            _restSharpMethodMapper = restSharpMethodMapper;
            _finalHttpRequestBuilder = finalHttpRequestBuilder;
            _clientHttpRequestExceptionFactory = clientHttpRequestExceptionFactory;
        }

        public Task<IRestRequest> BuildAsync(HttpRequest request)
        {
            var method = _restSharpMethodMapper.Map(request.Method);
            var restRequest = new RestRequest(request.Resource, method, DataFormat.Json);

            foreach (var param in request.Parameters)
            {
                restRequest.AddParameter(param.Name, param.Value!, ParameterType.QueryString);
            }

            foreach (var header in request.Headers)
            {
                restRequest.AddHeader(header.Name, header.Value);
            }

            if (request.Body is not null)
            {
                var body = _serializer.Serialize(request.Body);
                restRequest.AddParameter(_serializer.ContentType, body, ParameterType.RequestBody);
            }

            return Task.FromResult((IRestRequest)restRequest);
        }
        
        public Task<HttpResponse> BuildAsync(HttpRequest request, IRestResponse restResponse)
        {
            var finalRequest = _finalHttpRequestBuilder.Build(request, restResponse.Request);
            
            var allHeaders = restResponse.Headers
                .Where(x => x.Name != null)
                .Select(x => new HttpHeader(x.Name!, x.Value?.ToString() ?? ""))
                .ToArray();
            var responseHeaders = allHeaders
                .Where(x => !ContentHeaderNames.Contains(x.Name))
                .ToArray();
            var contentHeaders = allHeaders
                .Where(x => ContentHeaderNames.Contains(x.Name))
                .ToArray();
            
            var httpResponse = new HttpResponse(finalRequest)
            {
                Content = new HttpResponseContent(restResponse.RawBytes, new HttpResponseContentHeaderContainer(contentHeaders)),
                StatusCode = restResponse.StatusCode,
                ResponseUri = restResponse.ResponseUri,
                Headers = new HttpResponseHeaderContainer(responseHeaders),
                ErrorMessage = restResponse.ErrorMessage,
                ProtocolVersion = restResponse.ProtocolVersion
            };

            httpResponse.ErrorException = restResponse.ErrorException is not null
                ? _clientHttpRequestExceptionFactory.HttpRequestFailed(httpResponse)
                : null;

            return Task.FromResult(httpResponse);
        }
    }
}
