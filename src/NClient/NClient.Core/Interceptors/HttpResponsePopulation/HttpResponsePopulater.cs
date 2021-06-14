using System;
using System.Linq;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Core.Interceptors.HttpResponsePopulation
{
    internal interface IHttpResponsePopulater
    {
        HttpResponse Populate<TResult>(HttpResponse httpResponse);
    }

    internal class HttpResponsePopulater : IHttpResponsePopulater
    {
        private readonly ISerializer _serializer;

        public HttpResponsePopulater(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public HttpResponse Populate<TResult>(HttpResponse httpResponse)
        {
            var (bodyType, errorType) = GetBodyAndErrorType<TResult>();
            
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
        
        private static (Type? BodyType, Type? ErrorType) GetBodyAndErrorType<TResult>()
        {
            var resultType = typeof(TResult);

            if (resultType == typeof(HttpResponse))
                return (null, null);

            if (IsAssignableFromGeneric<TResult>(typeof(HttpResponseWithError<>)))
                return (null, resultType.GetGenericArguments().Single());

            if (IsAssignableFromGeneric<TResult>(typeof(HttpResponse<>)))
                return (resultType.GetGenericArguments().Single(), null);

            if (IsAssignableFromGeneric<TResult>(typeof(HttpResponseWithError<,>)))
                return (resultType.GetGenericArguments()[0], resultType.GetGenericArguments()[1]);

            return (resultType, null);
        }
        
        private static bool IsAssignableFromGeneric<TSource>(Type destType)
        {
            return typeof(TSource).IsGenericType && typeof(TSource).GetGenericTypeDefinition().IsAssignableFrom(destType.GetGenericTypeDefinition());
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