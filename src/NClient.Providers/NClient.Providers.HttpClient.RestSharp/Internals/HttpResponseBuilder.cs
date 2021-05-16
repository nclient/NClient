using System;
using System.Linq;
using System.Text.Json;
using NClient.Abstractions.HttpClients;
using RestSharp;
using HttpHeader = NClient.Abstractions.HttpClients.HttpHeader;
using HttpResponse = NClient.Abstractions.HttpClients.HttpResponse;

namespace NClient.Providers.HttpClient.RestSharp.Internals
{
    public class HttpResponseBuilder
    {
        public HttpResponse Build(HttpRequest request, IRestResponse restResponse, Type? bodyType = null, Type? errorType = null)
        {
            var response = new HttpResponse(request)
            {
                ContentType = string.IsNullOrEmpty(restResponse.ContentType) ? null : restResponse.ContentType,
                ContentLength = restResponse.ContentLength,
                ContentEncoding = string.IsNullOrEmpty(restResponse.ContentEncoding) ? null : restResponse.ContentEncoding,
                Content = restResponse.Content,
                StatusCode = restResponse.StatusCode,
                ResponseUri = restResponse.ResponseUri,
                Server = string.IsNullOrEmpty(restResponse.Server) ? null : restResponse.Server,
                Headers = restResponse.Headers
                    .Where(x => x.Name != null)
                    .Select(x => new HttpHeader(x.Name!, x.Value?.ToString() ?? "")).ToArray(),
                ErrorMessage = restResponse.ErrorMessage,
                ErrorException = restResponse.ErrorException,
                ProtocolVersion = restResponse.ProtocolVersion
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