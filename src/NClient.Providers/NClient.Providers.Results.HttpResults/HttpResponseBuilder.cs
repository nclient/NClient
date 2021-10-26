using System;
using System.Net.Http;
using System.Threading.Tasks;
using NClient.Providers.Serialization;

namespace NClient.Providers.Results.HttpResults
{
    public class HttpResponseBuilder : IResultBuilder<HttpResponseMessage>
    {
        public bool CanBuild(Type resultType, HttpResponseMessage httpResponseMessage)
        {
            if (!resultType.IsGenericType)
                return false;

            return resultType.GetGenericTypeDefinition() == typeof(HttpResponse)
                || resultType.GetGenericTypeDefinition() == typeof(IHttpResponse)
                || resultType.GetGenericTypeDefinition() == typeof(HttpResponse<>)
                || resultType.GetGenericTypeDefinition() == typeof(IHttpResponse<>)
                || resultType.GetGenericTypeDefinition() == typeof(HttpResponseWithError<>)
                || resultType.GetGenericTypeDefinition() == typeof(IHttpResponseWithError<>)
                || resultType.GetGenericTypeDefinition() == typeof(HttpResponseWithError<,>)
                || resultType.GetGenericTypeDefinition() == typeof(IHttpResponseWithError<,>);
        }
        
        public async Task<object?> BuildAsync(Type resultType, HttpResponseMessage httpResponseMessage, ISerializer serializer)
        {
            var httpRequest = new HttpRequest(httpResponseMessage.RequestMessage);
            var httpResponse = new HttpResponse(httpRequest, httpResponseMessage);
            var content = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (resultType == typeof(HttpResponse) || resultType == typeof(IHttpResponse))
                return httpResponse;
            
            if (resultType.GetGenericTypeDefinition() == typeof(HttpResponse<>) || resultType.GetGenericTypeDefinition() == typeof(IHttpResponse<>))
            { 
                var data = httpResponse.IsSuccessful
                    ? serializer.Deserialize(content, resultType.GetGenericArguments()[0])
                    : null;
                var httpResponseType = typeof(HttpResponse<>).MakeGenericType(resultType.GetGenericArguments()[0]);
                return Activator.CreateInstance(httpResponseType, httpResponse, data);
            }
            
            if (resultType.GetGenericTypeDefinition() == typeof(HttpResponseWithError<>) || resultType.GetGenericTypeDefinition() == typeof(IHttpResponseWithError<>))
            {
                var error = httpResponse.IsSuccessful 
                    ? null
                    : serializer.Deserialize(content, resultType.GetGenericArguments()[0]);
                var httpResponseType = typeof(HttpResponseWithError<>).MakeGenericType(resultType.GetGenericArguments()[0]);
                return Activator.CreateInstance(httpResponseType, httpResponse, error);
            }
            
            if (resultType.GetGenericTypeDefinition() == typeof(HttpResponseWithError<,>) || resultType.GetGenericTypeDefinition() == typeof(IHttpResponseWithError<,>))
            {
                var data = httpResponse.IsSuccessful
                    ? serializer.Deserialize(content, resultType.GetGenericArguments()[0])
                    : null;
                var error = httpResponse.IsSuccessful 
                    ? null
                    : serializer.Deserialize(content, resultType.GetGenericArguments()[1]);
                var httpResponseType = typeof(HttpResponseWithError<,>).MakeGenericType(resultType.GetGenericArguments()[0], resultType.GetGenericArguments()[1]);
                return Activator.CreateInstance(httpResponseType, httpResponse, data, error);
            }
            
            // TODO throw exception
            return httpResponse;
        }
    }
}
