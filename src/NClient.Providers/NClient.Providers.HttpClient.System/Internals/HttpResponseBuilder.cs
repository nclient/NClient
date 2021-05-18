using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Providers.HttpClient.System.Internals
{
    public class HttpResponseBuilder
    {
        private readonly ISerializer _serializer;

        public HttpResponseBuilder(ISerializer serializer)
        {
            _serializer = serializer;
        }
        
        public async Task<HttpResponse> BuildAsync(
            HttpRequest request, HttpResponseMessage httpResponseMessage,
            Type? bodyType = null, Type? errorType = null, Exception? exception = null)
        {
            var headers = httpResponseMessage.Headers
                .Select(x => new HttpHeader(x.Key!, x.Value?.FirstOrDefault() ?? ""))
                .ToArray();
            var contentHeaders = httpResponseMessage.Content.Headers
                .Select(x => new HttpHeader(x.Key!, x.Value?.FirstOrDefault() ?? ""))
                .ToArray();
            var content = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            var response = new HttpResponse(request)
            {
                ContentType = httpResponseMessage.Content.Headers.ContentType?.MediaType,
                ContentLength = httpResponseMessage.Content.Headers.ContentLength,
                ContentEncoding = httpResponseMessage.Content.Headers.ContentEncoding.FirstOrDefault(),
                Content = content,
                StatusCode = httpResponseMessage.StatusCode,
                StatusDescription = httpResponseMessage.StatusCode.ToString(),
                ResponseUri = httpResponseMessage.RequestMessage.RequestUri,
                Server = httpResponseMessage.Headers.Server?.ToString(),
                Headers = headers.Concat(contentHeaders).ToArray(),
                ErrorMessage = exception?.Message,
                ErrorException = exception,
                ProtocolVersion = httpResponseMessage.Version
            };

            if (bodyType is null && errorType is not null)
            {
                var errorObject = TryGetErrorObject(errorType, response);
                var genericResponseType = typeof(HttpResponseWithError<>).MakeGenericType(errorType);
                return (HttpResponse)Activator.CreateInstance(genericResponseType, response, request, errorObject);
            }

            if (bodyType is not null && errorType is null)
            {
                var bodyObject = TryGetBodyObject(bodyType, response);
                var genericResponseType = typeof(HttpResponse<>).MakeGenericType(bodyType);
                return (HttpResponse)Activator.CreateInstance(genericResponseType, response, request, bodyObject);
            }

            if (bodyType is not null && errorType is not null)
            {
                var bodyObject = TryGetBodyObject(bodyType, response);
                var errorObject = TryGetErrorObject(errorType, response);
                var genericResponseType = typeof(HttpResponseWithError<,>).MakeGenericType(bodyType, errorType);
                return (HttpResponse)Activator.CreateInstance(genericResponseType, response, request, bodyObject, errorObject);
            }

            return response;
        }

        private object? TryGetBodyObject(Type bodyType, HttpResponse response)
        {
            return response.IsSuccessful
                ? _serializer.Deserialize(response.Content!, bodyType)
                : null;
        }

        private object? TryGetErrorObject(Type errorType, HttpResponse response)
        {
            return !response.IsSuccessful
                ? _serializer.Deserialize(response.Content!, errorType)
                : null;
        }
    }
}