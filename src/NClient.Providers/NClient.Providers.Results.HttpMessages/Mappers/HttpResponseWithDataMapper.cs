using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Mapping;
using NClient.Abstractions.Serialization;

namespace NClient.Providers.Results.HttpMessages.Mappers
{
    public class HttpResponseWithDataMapper : IResponseMapper<HttpResponseMessage>
    {
        private readonly HttpResponseMapper _httpResponseMapper;
        
        public HttpResponseWithDataMapper()
        {
            _httpResponseMapper = new HttpResponseMapper();
        }
        
        public bool CanMapTo(Type resultType)
        {
            if (!resultType.IsGenericType)
                return false;
            
            return resultType.GetGenericTypeDefinition() == typeof(IHttpResponse<>).GetGenericTypeDefinition() 
                || resultType.GetGenericTypeDefinition() == typeof(HttpResponse<>).GetGenericTypeDefinition();
        }

        public async Task<object?> MapAsync(Type resultType, IHttpRequest httpRequest, HttpResponseMessage response, ISerializer serializer)
        {
            var httpResponse = (HttpResponse)(await _httpResponseMapper
                .MapAsync(resultType, httpRequest, response, serializer)
                .ConfigureAwait(false))!;
            
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
