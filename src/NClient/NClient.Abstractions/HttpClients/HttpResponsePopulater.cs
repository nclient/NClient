using System;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions.HttpClients
{
    internal interface IHttpResponsePopulater
    {
        HttpResponse Populate(HttpResponse httpResponse, Type? bodyType, Type? errorType);
    }

    internal class HttpResponsePopulater : IHttpResponsePopulater
    {
        private readonly ISerializer _serializer;

        public HttpResponsePopulater(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public HttpResponse Populate(HttpResponse httpResponse, Type? bodyType, Type? errorType)
        {
            if (bodyType is null && errorType is not null)
            {
                var errorObject = TryGetErrorObject(errorType, httpResponse);
                var genericResponseType = typeof(HttpResponseWithError<>).MakeGenericType(errorType);
                return (HttpResponse)Activator.CreateInstance(genericResponseType, httpResponse, httpResponse.Request, errorObject);
            }

            if (bodyType is not null && errorType is null)
            {
                var bodyObject = TryGetBodyObject(bodyType, httpResponse);
                var genericResponseType = typeof(HttpResponse<>).MakeGenericType(bodyType);
                return (HttpResponse)Activator.CreateInstance(genericResponseType, httpResponse, httpResponse.Request, bodyObject);
            }

            if (bodyType is not null && errorType is not null)
            {
                var bodyObject = TryGetBodyObject(bodyType, httpResponse);
                var errorObject = TryGetErrorObject(errorType, httpResponse);
                var genericResponseType = typeof(HttpResponseWithError<,>).MakeGenericType(bodyType, errorType);
                return (HttpResponse)Activator.CreateInstance(genericResponseType, httpResponse, httpResponse.Request, bodyObject, errorObject);
            }

            return httpResponse;
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