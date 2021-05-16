using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using NClient.Abstractions.HttpClients;
using HttpHeader = NClient.Abstractions.HttpClients.HttpHeader;
using HttpResponse = NClient.Abstractions.HttpClients.HttpResponse;

namespace NClient.Providers.HttpClient.System
{
    public class SystemHttpClient : IHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _httpClientName;

        public SystemHttpClient(IHttpClientFactory httpClientFactory, string? httpClientName = null)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientName = httpClientName ?? Options.DefaultName;
        }

        public async Task<HttpResponse> ExecuteAsync(HttpRequest request, Type? bodyType = null, Type? errorType = null)
        {
            var httpRequestMessage = BuildRequestMessage(request);
            var (httpResponseMessage, exception) = await TrySendAsync(httpRequestMessage).ConfigureAwait(false);
            return await BuildResponseAsync(request, httpResponseMessage, bodyType, errorType, exception).ConfigureAwait(false);
        }

        private static HttpRequestMessage BuildRequestMessage(HttpRequest request)
        {
            var parameters = request.Parameters
                .ToDictionary(x => x.Name, x => x.Value!.ToString());
            var uri = new Uri(QueryHelpers.AddQueryString(request.Uri.ToString(), parameters));

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = request.Method,
                RequestUri = uri
            };

            httpRequestMessage.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            if (request.Body != null)
            {
                var body = JsonSerializer.Serialize(request.Body);
                httpRequestMessage.Content = new StringContent(body, Encoding.UTF8, mediaType: "application/json");
            }

            foreach (var header in request.Headers)
            {
                httpRequestMessage.Headers.Add(header.Name, header.Value);
            }

            return httpRequestMessage;
        }
        
        private async Task<(HttpResponseMessage HttpResponseMessage, Exception? Exception)> TrySendAsync(HttpRequestMessage httpRequestMessage)
        {
            var httpClient = _httpClientFactory.CreateClient(_httpClientName);
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false); ;

            try
            {
                httpResponseMessage.EnsureSuccessStatusCode();
                return (httpResponseMessage, null);
            }
            catch (HttpRequestException e)
            {
                return (httpResponseMessage, e);
            }
        }

        private static async Task<HttpResponse> BuildResponseAsync(
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
                var genericResponseType = typeof(HttpResponse<>).MakeGenericType(errorType);
                return (HttpResponse)Activator.CreateInstance(genericResponseType, response, request, errorObject);
            }
            
            if (bodyType is not null && errorType is null)
            {
                var bodyObject = TryGetBodyObject(bodyType, response);
                var genericResponseType = typeof(HttpValueResponse<>).MakeGenericType(bodyType);
                return (HttpResponse)Activator.CreateInstance(genericResponseType, response, request, bodyObject);
            }
            
            if (bodyType is not null && errorType is not null)
            {
                var bodyObject = TryGetBodyObject(bodyType, response);
                var errorObject = TryGetErrorObject(errorType, response);
                var genericResponseType = typeof(HttpValueResponse<,>).MakeGenericType(bodyType, errorType);
                return (HttpResponse)Activator.CreateInstance(genericResponseType, response, request, bodyObject, errorObject);
            }
            
            return response;
        }

        private static object? TryGetBodyObject(Type bodyType, HttpResponse response)
        {
            return response.IsSuccessful && !string.IsNullOrEmpty(response.Content)
                ? JsonSerializer.Deserialize(response.Content!, bodyType)
                : null;
        }
        
        private static object? TryGetErrorObject(Type errorType, HttpResponse response)
        {
            return !response.IsSuccessful && !string.IsNullOrEmpty(response.Content)
                ? JsonSerializer.Deserialize(response.Content!, errorType)
                : null;
        }
    }
}
