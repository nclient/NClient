using System;
using System.Linq;
using System.Threading.Tasks;
using NClient.Abstractions.Mapping;
using NClient.Abstractions.Serialization;

namespace NClient.Providers.Results.HttpMessages.Mappers
{
    public class HttpResponseWithErrorMapper : IResponseMapper
    {
        public bool CanMapTo(Type resultType)
        {
            if (!resultType.IsGenericType)
                return false;
            
            return resultType.GetGenericTypeDefinition() == typeof(IHttpResponseWithError<>).GetGenericTypeDefinition() 
                || resultType.GetGenericTypeDefinition() == typeof(HttpResponseWithError<>).GetGenericTypeDefinition();
        }

        public async Task<object?> MapAsync(Type resultType, IHttpResponse httpResponse, ISerializer serializer)
        {
            var errorType = resultType.GetGenericArguments().Single();
            var error = TryGetErrorObject(errorType, httpResponse, serializer);
            var genericResponseType = typeof(HttpResponseWithError<>).MakeGenericType(errorType);
            return (IHttpResponse)Activator.CreateInstance(genericResponseType, httpResponse, httpResponse.Request, error);
        }
        
        private static object? TryGetErrorObject(Type errorType, IHttpResponse response, ISerializer serializer)
        {
            return !response.IsSuccessful
                ? serializer.Deserialize(response.Content.ToString(), errorType)
                : null;
        }
    }
}
