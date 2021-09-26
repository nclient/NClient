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

            var content = httpResponseMessage.Content is null 
                ? new HttpResponseContent() 
                : new HttpResponseContent(
                    await httpResponseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false),
                    new HttpResponseContentHeaderContainer(httpResponseMessage.Content.Headers));

            var httpResponse = new HttpResponse(finalRequest)
            {
                Headers = new HttpResponseHeaderContainer(httpResponseMessage.Headers),
                ContentType = httpResponseMessage.Content?.Headers?.ContentType?.MediaType,
                ContentLength = httpResponseMessage.Content?.Headers?.ContentLength,
                ContentEncoding = httpResponseMessage.Content?.Headers?.ContentEncoding.FirstOrDefault(),
                Content = content,
                StatusCode = httpResponseMessage.StatusCode,
                StatusDescription = httpResponseMessage.StatusCode.ToString(),
                ResponseUri = httpResponseMessage.RequestMessage.RequestUri,
                Server = httpResponseMessage.Headers?.Server?.ToString(),
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
