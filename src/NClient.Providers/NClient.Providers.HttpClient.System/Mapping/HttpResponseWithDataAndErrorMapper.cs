using System;
using System.Net.Http;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Mapping;
using NClient.Abstractions.Serialization;

namespace NClient.Providers.HttpClient.System.Mapping
{
    public class HttpResponseWithDataAndErrorMapper : IResponseMapper<HttpResponseMessage>
    {
        private readonly HttpResponseMapper _httpResponseMapper;
        
        public HttpResponseWithDataAndErrorMapper()
        {
            _httpResponseMapper = new HttpResponseMapper();
        }
        
        public bool CanMapTo(Type resultType)
        {
            if (!resultType.IsGenericType)
                return false;
            
            return resultType.GetGenericTypeDefinition() == typeof(IHttpResponseWithError<,>).GetGenericTypeDefinition()
                || resultType.GetGenericTypeDefinition() == typeof(HttpResponseWithError<,>).GetGenericTypeDefinition();
        }

        public async Task<object?> MapAsync(Type resultType, IHttpRequest httpRequest, HttpResponseMessage response, ISerializer serializer)
        {
            var httpResponse = (HttpResponse)(await _httpResponseMapper
                .MapAsync(resultType, httpRequest, response, serializer)
                .ConfigureAwait(false))!;
            
            var dataType = resultType.GetGenericArguments()[0];
            var data = TryGetDataObject(dataType, httpResponse, serializer);
            var errorType = resultType.GetGenericArguments()[1];
            var error = TryGetErrorObject(errorType, httpResponse, serializer);
            var genericResponseType = typeof(HttpResponseWithError<,>).MakeGenericType(dataType, errorType);
            return (IHttpResponse)Activator.CreateInstance(genericResponseType, httpResponse, httpResponse.Request, data, error);
        }
        
        private static object? TryGetDataObject(Type dataType, IHttpResponse response, ISerializer serializer)
        {
            return response.IsSuccessful
                ? serializer.Deserialize(response.Content.ToString(), dataType)
                : null;
        }
        
        private static object? TryGetErrorObject(Type errorType, IHttpResponse response, ISerializer serializer)
        {
            return !response.IsSuccessful
                ? serializer.Deserialize(response.Content.ToString(), errorType)
                : null;
        }
    }
}
