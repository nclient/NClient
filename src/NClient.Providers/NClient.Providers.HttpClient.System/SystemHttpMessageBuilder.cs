using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using NClient.Abstractions.Exceptions.Factories;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Providers.HttpClient.System.Builders;

namespace NClient.Providers.HttpClient.System
{
    internal class SystemHttpMessageBuilder : IHttpMessageBuilder<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly ISerializer _serializer;
        private readonly IFinalHttpRequestBuilder _finalHttpRequestBuilder;
        private readonly IClientHttpRequestExceptionFactory _clientHttpRequestExceptionFactory;

        public SystemHttpMessageBuilder(
            ISerializer serializer,
            IFinalHttpRequestBuilder finalHttpRequestBuilder,
            IClientHttpRequestExceptionFactory clientHttpRequestExceptionFactory)
        {
            _serializer = serializer;
            _finalHttpRequestBuilder = finalHttpRequestBuilder;
            _clientHttpRequestExceptionFactory = clientHttpRequestExceptionFactory;
        }
        
        public Task<HttpRequestMessage> BuildRequestAsync(HttpRequest request)
        {
            var parameters = request.Parameters
                .ToDictionary(x => x.Name, x => x.Value!.ToString());
            var uri = new Uri(QueryHelpers.AddQueryString(request.Resource.ToString(), parameters));

            var httpRequestMessage = new HttpRequestMessage { Method = request.Method, RequestUri = uri };

            httpRequestMessage.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(_serializer.ContentType));

            if (request.Body != null)
            {
                var body = _serializer.Serialize(request.Body);
                httpRequestMessage.Content = new StringContent(body, Encoding.UTF8, _serializer.ContentType);
            }

            foreach (var header in request.Headers)
            {
                httpRequestMessage.Headers.Add(header.Name, header.Value);
            }

            return Task.FromResult(httpRequestMessage);
        }

        public async Task<HttpResponse> BuildResponseAsync(HttpRequest request, HttpResponseMessage httpResponseMessage)
        {
            var exception = TryGetException(httpResponseMessage);
            
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
                Content = content,
                StatusCode = httpResponseMessage.StatusCode,
                StatusDescription = httpResponseMessage.StatusCode.ToString(),
                ResponseUri = httpResponseMessage.RequestMessage.RequestUri,
                ErrorMessage = exception?.Message,
                ProtocolVersion = httpResponseMessage.Version
            };

            httpResponse.ErrorException = exception is not null
                ? _clientHttpRequestExceptionFactory.HttpRequestFailed(httpResponse)
                : null;

            return httpResponse;
        }
        
        private static Exception? TryGetException(HttpResponseMessage httpResponseMessage)
        {
            try
            {
                httpResponseMessage.EnsureSuccessStatusCode();
                return null;
            }
            catch (HttpRequestException e)
            {
                return e;
            }
        }
    }
}
