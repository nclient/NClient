using System;
using System.Linq;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Standalone.Interceptors.HttpResponsePopulation
{
    internal interface IHttpResponsePopulater
    {
        IHttpResponse Populate(IHttpResponse httpResponse, Type resultType);
    }

    internal class HttpResponsePopulater : IHttpResponsePopulater
    {
        private readonly ISerializer _serializer;

        public HttpResponsePopulater(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public IHttpResponse Populate(IHttpResponse httpResponse, Type resultType)
        {
            var (bodyType, errorType) = GetBodyAndErrorType(resultType);

            if (bodyType is null && errorType is not null)
            {
                var errorObject = TryGetErrorObject(errorType, httpResponse);
                var genericResponseType = typeof(IHttpResponseWithError<>).MakeGenericType(errorType);
                return (IHttpResponse)Activator.CreateInstance(genericResponseType, httpResponse, httpResponse.Request, errorObject);
            }

            if (bodyType is not null && errorType is null)
            {
                var bodyObject = TryGetBodyObject(bodyType, httpResponse);
                var genericResponseType = typeof(IHttpResponse<>).MakeGenericType(bodyType);
                return (IHttpResponse)Activator.CreateInstance(genericResponseType, httpResponse, httpResponse.Request, bodyObject);
            }

            if (bodyType is not null && errorType is not null)
            {
                var bodyObject = TryGetBodyObject(bodyType, httpResponse);
                var errorObject = TryGetErrorObject(errorType, httpResponse);
                var genericResponseType = typeof(IHttpResponseWithError<,>).MakeGenericType(bodyType, errorType);
                return (IHttpResponse)Activator.CreateInstance(genericResponseType, httpResponse, httpResponse.Request, bodyObject, errorObject);
            }

            return httpResponse;
        }

        private static (Type? BodyType, Type? ErrorType) GetBodyAndErrorType(Type resultType)
        {
            if (resultType == typeof(void) || resultType == typeof(IHttpResponse))
                return (null, null);

            if (IsAssignableFromGeneric(resultType, typeof(IHttpResponseWithError<>)))
                return (null, resultType.GetGenericArguments().Single());

            if (IsAssignableFromGeneric(resultType, typeof(IHttpResponse<>)))
                return (resultType.GetGenericArguments().Single(), null);

            if (IsAssignableFromGeneric(resultType, typeof(IHttpResponseWithError<,>)))
                return (resultType.GetGenericArguments()[0], resultType.GetGenericArguments()[1]);

            return (resultType, null);
        }

        private static bool IsAssignableFromGeneric(Type sourceType, Type destType)
        {
            return sourceType.IsGenericType && sourceType.GetGenericTypeDefinition().IsAssignableFrom(destType.GetGenericTypeDefinition());
        }

        private object? TryGetBodyObject(Type bodyType, IHttpResponse response)
        {
            return response.IsSuccessful
                ? _serializer.Deserialize(response.Content.ToString(), bodyType)
                : null;
        }

        private object? TryGetErrorObject(Type errorType, IHttpResponse response)
        {
            return !response.IsSuccessful
                ? _serializer.Deserialize(response.Content.ToString(), errorType)
                : null;
        }
    }
}
