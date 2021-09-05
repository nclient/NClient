using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NClient.Abstractions.Exceptions.Factories;
using NClient.Abstractions.HttpClients;

namespace NClient.Providers.HttpClient.System.Builders
{
    internal interface IHttpResponseBuilder
    {
        Task<HttpResponse> BuildAsync(
            HttpRequest request, HttpResponseMessage httpResponseMessage, Exception? exception = null);
    }

    internal class HttpResponseBuilder : IHttpResponseBuilder
    {
        private readonly IFinalHttpRequestBuilder _finalHttpRequestBuilder;
        private readonly IClientHttpRequestExceptionFactory _clientHttpRequestExceptionFactory;

        public HttpResponseBuilder(
            IFinalHttpRequestBuilder finalHttpRequestBuilder,
            IClientHttpRequestExceptionFactory clientHttpRequestExceptionFactory)
        {
            _finalHttpRequestBuilder = finalHttpRequestBuilder;
            _clientHttpRequestExceptionFactory = clientHttpRequestExceptionFactory;
        }

        public async Task<HttpResponse> BuildAsync(
            HttpRequest request, HttpResponseMessage httpResponseMessage, Exception? exception = null)
        {
            var finalRequest = await _finalHttpRequestBuilder
                .BuildAsync(request, httpResponseMessage.RequestMessage)
                .ConfigureAwait(false);
            
            var headers = httpResponseMessage.Headers
                .Select(x => new HttpHeader(x.Key!, x.Value?.FirstOrDefault() ?? ""))
                .ToArray();
            var contentHeaders = httpResponseMessage.Content.Headers
                .Select(x => new HttpHeader(x.Key!, x.Value?.FirstOrDefault() ?? ""))
                .ToArray();
            var rawBytes = await httpResponseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

            var httpResponse = new HttpResponse(finalRequest)
            {
                ContentType = httpResponseMessage.Content.Headers.ContentType?.MediaType,
                ContentLength = httpResponseMessage.Content.Headers.ContentLength,
                ContentEncoding = httpResponseMessage.Content.Headers.ContentEncoding.FirstOrDefault(),
                RawBytes = rawBytes,
                StatusCode = httpResponseMessage.StatusCode,
                StatusDescription = httpResponseMessage.StatusCode.ToString(),
                ResponseUri = httpResponseMessage.RequestMessage.RequestUri,
                Server = httpResponseMessage.Headers.Server?.ToString(),
                Headers = headers.Concat(contentHeaders).ToArray(),
                ErrorMessage = exception?.Message,
                ProtocolVersion = httpResponseMessage.Version
            };

            httpResponse.ErrorException = exception is not null
                ? _clientHttpRequestExceptionFactory.HttpRequestFailed(httpResponse)
                : null;

            return httpResponse;
        }
    }
}
