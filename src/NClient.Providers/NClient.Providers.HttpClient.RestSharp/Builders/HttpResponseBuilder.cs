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
        private readonly IClientHttpRequestExceptionFactory _clientHttpRequestExceptionFactory;

        public HttpResponseBuilder(IClientHttpRequestExceptionFactory clientHttpRequestExceptionFactory)
        {
            _clientHttpRequestExceptionFactory = clientHttpRequestExceptionFactory;
        }

        public HttpResponse Build(HttpRequest request, IRestResponse restResponse)
        {
            var httpResponse = new HttpResponse(request)
            {
                ContentType = string.IsNullOrEmpty(restResponse.ContentType) ? null : restResponse.ContentType,
                ContentLength = restResponse.ContentLength,
                ContentEncoding = string.IsNullOrEmpty(restResponse.ContentEncoding) ? null : restResponse.ContentEncoding,
                RawBytes = restResponse.RawBytes,
                StatusCode = restResponse.StatusCode,
                ResponseUri = restResponse.ResponseUri,
                Server = string.IsNullOrEmpty(restResponse.Server) ? null : restResponse.Server,
                Headers = restResponse.Headers
                    .Where(x => x.Name != null)
                    .Select(x => new HttpHeader(x.Name!, x.Value?.ToString() ?? "")).ToArray(),
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