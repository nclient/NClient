using System;
using System.Linq;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Standalone.Interceptors.HttpResponsePopulation
{
    internal interface IHttpResponsePopulator
    {
        Task<IHttpResponse> PopulateAsync(IHttpResponse httpResponse, Type resultType);
    }

    internal class HttpResponsePopulator<TRequest, TResponse> : IHttpResponsePopulator
    {
        private readonly ISerializer _serializer;
        private readonly IHttpMessageBuilder<TRequest, TResponse> _httpMessageBuilder;

        public HttpResponsePopulator(
            ISerializer serializer,
            IHttpMessageBuilder<TRequest, TResponse> httpMessageBuilder)
        {
            _serializer = serializer;
            _httpMessageBuilder = httpMessageBuilder;
        }

        public async Task<IHttpResponse> PopulateAsync(IHttpResponse httpResponse, Type resultType)
        {
            var (dataType, errorType) = GetDataAndErrorType(resultType);

            if (dataType is null && errorType is not null)
            {
                var errorObject = TryGetErrorObject(errorType, httpResponse);
                return await _httpMessageBuilder
                    .BuildResponseWithErrorAsync(errorObject, errorType, httpResponse)
                    .ConfigureAwait(false);
            }

            if (dataType is not null && errorType is null)
            {
                var dataObject = TryGetDataObject(dataType, httpResponse);
                return await _httpMessageBuilder
                    .BuildResponseWithDataAsync(dataObject, dataType, httpResponse)
                    .ConfigureAwait(false);
            }

            if (dataType is not null && errorType is not null)
            {
                var dataObject = TryGetDataObject(dataType, httpResponse);
                var errorObject = TryGetErrorObject(errorType, httpResponse);
                return await _httpMessageBuilder
                    .BuildResponseWithDataAndErrorAsync(dataObject, dataType, errorObject, errorType, httpResponse)
                    .ConfigureAwait(false);
            }

            return httpResponse;
        }

        private static (Type? DataType, Type? ErrorType) GetDataAndErrorType(Type resultType)
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

        private object? TryGetDataObject(Type dataType, IHttpResponse response)
        {
            return response.IsSuccessful
                ? _serializer.Deserialize(response.Content.ToString(), dataType)
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
