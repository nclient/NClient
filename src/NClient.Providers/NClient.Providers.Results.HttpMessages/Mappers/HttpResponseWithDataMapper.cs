using System;
using System.Linq;
using System.Threading.Tasks;
using NClient.Abstractions.Mapping;
using NClient.Abstractions.Serialization;

namespace NClient.Providers.Results.HttpMessages.Mappers
{
    public class HttpResponseWithDataMapper : IResponseMapper
    {
        public bool CanMapTo(Type resultType)
        {
            if (!resultType.IsGenericType)
                return false;
            
            return resultType.GetGenericTypeDefinition() == typeof(IHttpResponse<>).GetGenericTypeDefinition() 
                || resultType.GetGenericTypeDefinition() == typeof(HttpResponse<>).GetGenericTypeDefinition();
        }

        public async Task<object?> MapAsync(Type resultType, IHttpResponse httpResponse, ISerializer serializer)
        {
            var dataType = resultType.GetGenericArguments().Single();
            var data = TryGetDataObject(dataType, httpResponse, serializer);
            var genericResponseType = typeof(HttpResponse<>).MakeGenericType(dataType);
            return (IHttpResponse)Activator.CreateInstance(genericResponseType, httpResponse, httpResponse.Request, data);
        }
        
        private static object? TryGetDataObject(Type dataType, IHttpResponse response, ISerializer serializer)
        {
            return response.IsSuccessful
                ? serializer.Deserialize(response.Content.ToString(), dataType)
                : null;
        }
    }
}
