using System.Collections.Generic;
using System.Linq;
using NClient.Abstractions.Exceptions.Factories;
using NClient.Abstractions.HttpClients;
using RestSharp;
using HttpHeader = NClient.Abstractions.HttpClients.HttpHeader;
using HttpResponse = NClient.Abstractions.HttpClients.HttpResponse;

namespace NClient.Providers.HttpClient.RestSharp.Builders
{
    internal interface IHttpResponseBuilder
    {
        HttpResponse Build(HttpRequest request, IRestResponse restResponse);
    }

    internal class HttpResponseBuilder : IHttpResponseBuilder
    {
        private static readonly HashSet<string> ContentHeadeNames = new(new HttpContentKnownHeaderNames());
        
        private readonly IFinalHttpRequestBuilder _finalHttpRequestBuilder;
        private readonly IClientHttpRequestExceptionFactory _clientHttpRequestExceptionFactory;

        public HttpResponseBuilder(
            IFinalHttpRequestBuilder finalHttpRequestBuilder,
            IClientHttpRequestExceptionFactory clientHttpRequestExceptionFactory)
        {
            _finalHttpRequestBuilder = finalHttpRequestBuilder;
            _clientHttpRequestExceptionFactory = clientHttpRequestExceptionFactory;
        }

        public HttpResponse Build(HttpRequest request, IRestResponse restResponse)
        {
            var finalRequest = _finalHttpRequestBuilder.Build(request, restResponse.Request);
            
            var allHeaders = restResponse.Headers
                .Where(x => x.Name != null)
                .Select(x => new HttpHeader(x.Name!, x.Value?.ToString() ?? ""))
                .ToArray();
            var responseHeaders = allHeaders
                .Where(x => !ContentHeadeNames.Contains(x.Name))
                .ToArray();
            var contentHeaders = allHeaders
                .Where(x => ContentHeadeNames.Contains(x.Name))
                .ToArray();
            
            var httpResponse = new HttpResponse(finalRequest)
            {
                ContentType = string.IsNullOrEmpty(restResponse.ContentType) ? null : restResponse.ContentType,
                ContentLength = restResponse.ContentLength,
                ContentEncoding = string.IsNullOrEmpty(restResponse.ContentEncoding) ? null : restResponse.ContentEncoding,
                Content = new HttpResponseContent(restResponse.RawBytes, new HttpResponseContentHeaderContainer(contentHeaders)),
                StatusCode = restResponse.StatusCode,
                ResponseUri = restResponse.ResponseUri,
                Server = string.IsNullOrEmpty(restResponse.Server) ? null : restResponse.Server,
                Headers = new HttpResponseHeaderContainer(responseHeaders),
                ErrorMessage = restResponse.ErrorMessage,
                ProtocolVersion = restResponse.ProtocolVersion
            };

            httpResponse.ErrorException = restResponse.ErrorException is not null
                ? _clientHttpRequestExceptionFactory.HttpRequestFailed(httpResponse)
                : null;

            return httpResponse;
        }
    }
}
