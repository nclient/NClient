using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.WebUtilities;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Providers.Results.HttpMessages;

namespace NClient.Providers.HttpClient.System
{
    internal class SystemHttpMessageBuilder : IHttpMessageBuilder<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly ISerializer _serializer;

        public SystemHttpMessageBuilder(ISerializer serializer)
        {
            _serializer = serializer;
        }
        
        public Task<HttpRequestMessage> BuildRequestAsync(IHttpRequest httpRequest)
        {
            var parameters = httpRequest.Parameters
                .ToDictionary(x => x.Name, x => x.Value!.ToString());
            var uri = new Uri(QueryHelpers.AddQueryString(httpRequest.Resource.ToString(), parameters));

            var httpRequestMessage = new HttpRequestMessage { Method = httpRequest.Method, RequestUri = uri };

            httpRequestMessage.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(_serializer.ContentType));

            foreach (var header in httpRequest.Headers)
            {
                httpRequestMessage.Headers.Add(header.Name, header.Value);
            }
            
            if (httpRequest.Data != null)
            {
                var body = _serializer.Serialize(httpRequest.Data);
                httpRequestMessage.Content = new StringContent(body, Encoding.UTF8, _serializer.ContentType);
            }

            return Task.FromResult(httpRequestMessage);
        }
        
        public async Task<IHttpResponse> BuildResponseAsync(IHttpRequest httpRequest, HttpResponseMessage response)
        {
            var exception = TryGetException(response);
            
            var finalHttpRequest = await BuildResponseAsync(httpRequest, response.RequestMessage)
                .ConfigureAwait(false);

            var content = response.Content is null 
                ? new HttpResponseContent() 
                : new HttpResponseContent(
                    await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false),
                    new HttpResponseContentHeaderContainer(response.Content.Headers));

            var httpResponse = new HttpResponse(finalHttpRequest)
            {
                Headers = new HttpResponseHeaderContainer(response.Headers),
                Content = content,
                StatusCode = response.StatusCode,
                StatusDescription = response.StatusCode.ToString(),
                ResponseUri = response.RequestMessage.RequestUri,
                ErrorMessage = exception?.Message,
                ErrorException = exception,
                ProtocolVersion = response.Version
            };

            return httpResponse;
        }
        
        private async Task<IHttpRequest> BuildResponseAsync(IHttpRequest httpRequest, HttpRequestMessage httpRequestMessage)
        {
            var resource = new Uri(httpRequestMessage.RequestUri.GetLeftPart(UriPartial.Path));
            var content = httpRequestMessage.Content is null ? null : await httpRequestMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            var finalRequest = new HttpRequest(httpRequest.Id, resource, httpRequestMessage.Method)
            {
                Content = content,
                Data = content is not null && httpRequest.Data is not null 
                    ? _serializer.Deserialize(content, httpRequest.Data.GetType()) 
                    : null
            };

            var headers = httpRequestMessage.Headers?
                .Select(x => new HttpHeader(x.Key!, x.Value?.FirstOrDefault() ?? ""))
                .ToArray() ?? Array.Empty<IHttpHeader>();
            var contentHeaders = httpRequestMessage.Content?.Headers
                .Select(x => new HttpHeader(x.Key!, x.Value?.FirstOrDefault() ?? ""))
                .ToArray() ?? Array.Empty<IHttpHeader>();
            foreach (var header in headers.Concat(contentHeaders))
            {
                finalRequest.AddHeader(header.Name, header.Value);
            }

            var queryParameterCollection = HttpUtility.ParseQueryString(httpRequestMessage.RequestUri.Query);
            foreach (var parameterName in queryParameterCollection.AllKeys)
            {
                var parameterValue = queryParameterCollection[parameterName] ?? "";
                finalRequest.AddParameter(parameterName, parameterValue);
            }

            return finalRequest;
        }
        
        private static HttpRequestException? TryGetException(HttpResponseMessage httpResponseMessage)
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
