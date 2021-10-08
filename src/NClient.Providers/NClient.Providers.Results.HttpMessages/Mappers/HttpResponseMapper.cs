using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Mapping;
using NClient.Abstractions.Serialization;

namespace NClient.Providers.Results.HttpMessages.Mappers
{
    public class HttpResponseMapper : IResponseMapper<HttpResponseMessage>
    {
        public bool CanMapTo(Type resultType)
        {
            return resultType == typeof(IHttpResponse)
                || resultType == typeof(HttpResponse);
        }
        
        public async Task<object?> MapAsync(Type resultType, IHttpRequest httpRequest, HttpResponseMessage response, ISerializer serializer)
        {
            var exception = TryGetException(response);
            
            var finalHttpRequest = await BuildResponseAsync(httpRequest, response.RequestMessage, serializer)
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
        
        private static async Task<IHttpRequest> BuildResponseAsync(IHttpRequest httpRequest, HttpRequestMessage httpRequestMessage, ISerializer serializer)
        {
            var resource = new Uri(httpRequestMessage.RequestUri.GetLeftPart(UriPartial.Path));
            var content = httpRequestMessage.Content is null ? null : await httpRequestMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            var finalRequest = new HttpRequest(httpRequest.Id, resource, httpRequestMessage.Method)
            {
                Content = content,
                Data = content is not null && httpRequest.Data is not null 
                    ? serializer.Deserialize(content, httpRequest.Data.GetType()) 
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
