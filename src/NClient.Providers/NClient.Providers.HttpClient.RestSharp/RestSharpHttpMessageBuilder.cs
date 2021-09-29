using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Providers.HttpClient.RestSharp.Builders;
using NClient.Providers.HttpClient.RestSharp.Helpers;
using RestSharp;
using HttpHeader = NClient.Abstractions.HttpClients.HttpHeader;
using HttpResponse = NClient.Abstractions.HttpClients.HttpResponse;

namespace NClient.Providers.HttpClient.RestSharp
{
    internal class RestSharpHttpMessageBuilder : IHttpMessageBuilder<IRestRequest, IRestResponse>
    {
        private static readonly HashSet<string> ContentHeaderNames = new(new HttpContentKnownHeaderNames());

        private readonly ISerializer _serializer;
        private readonly IRestSharpMethodMapper _restSharpMethodMapper;
        private readonly IFinalHttpRequestBuilder _finalHttpRequestBuilder;
        private readonly IHttpClientExceptionFactory<IRestRequest, IRestResponse> _httpClientExceptionFactory;

        public RestSharpHttpMessageBuilder(
            ISerializer serializer,
            IRestSharpMethodMapper restSharpMethodMapper,
            IFinalHttpRequestBuilder finalHttpRequestBuilder,
            IHttpClientExceptionFactory<IRestRequest, IRestResponse> httpClientExceptionFactory)
        {
            _serializer = serializer;
            _restSharpMethodMapper = restSharpMethodMapper;
            _finalHttpRequestBuilder = finalHttpRequestBuilder;
            _httpClientExceptionFactory = httpClientExceptionFactory;
        }

        public Task<IRestRequest> BuildRequestAsync(HttpRequest httpRequest)
        {
            var method = _restSharpMethodMapper.Map(httpRequest.Method);
            var restRequest = new RestRequest(httpRequest.Resource, method, DataFormat.Json);

            foreach (var param in httpRequest.Parameters)
            {
                restRequest.AddParameter(param.Name, param.Value!, ParameterType.QueryString);
            }

            foreach (var header in httpRequest.Headers)
            {
                restRequest.AddHeader(header.Name, header.Value);
            }

            if (httpRequest.Body is not null)
            {
                var body = _serializer.Serialize(httpRequest.Body);
                restRequest.AddParameter(_serializer.ContentType, body, ParameterType.RequestBody);
            }

            return Task.FromResult((IRestRequest)restRequest);
        }
        
        public Task<HttpResponse> BuildResponseAsync(HttpRequest httpRequest, IRestResponse response)
        {
            var finalRequest = _finalHttpRequestBuilder.Build(httpRequest, response.Request);
            
            var allHeaders = response.Headers
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
                Content = new HttpResponseContent(response.RawBytes, new HttpResponseContentHeaderContainer(contentHeaders)),
                StatusCode = response.StatusCode,
                ResponseUri = response.ResponseUri,
                Headers = new HttpResponseHeaderContainer(responseHeaders),
                ErrorMessage = response.ErrorMessage,
                ProtocolVersion = response.ProtocolVersion
            };

            httpResponse.ErrorException = response.ErrorException is not null
                ? _httpClientExceptionFactory.HttpRequestFailed(response.Request, response, response.ErrorException)
                : null;

            return Task.FromResult(httpResponse);
        }
    }
}
