using System;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;

namespace NClient.Standalone.Client.HttpClients
{
    internal class StubHttpMessageBuilder : IHttpMessageBuilder<IHttpRequest, IHttpResponse>
    {
        public Task<IHttpRequest> BuildRequestAsync(IHttpRequest httpRequest)
        {
            return Task.FromResult(httpRequest);
        }
        public Task<IHttpResponse> BuildResponseAsync(IHttpRequest httpRequest, IHttpResponse response)
        {
            return Task.FromResult(response);
        }
        
        public Task<IHttpResponse> BuildResponseWithDataAsync(object? data, Type dataType, IHttpResponse httpResponse)
        {
            var genericResponseType = typeof(HttpResponse<>).MakeGenericType(dataType);
            return Task.FromResult((IHttpResponse)Activator.CreateInstance(genericResponseType, httpResponse, httpResponse.Request, data));
        }
        
        public Task<IHttpResponse> BuildResponseWithErrorAsync(object? error, Type errorType, IHttpResponse httpResponse)
        {
            var genericResponseType = typeof(HttpResponseWithError<>).MakeGenericType(errorType);
            return Task.FromResult((IHttpResponse)Activator.CreateInstance(genericResponseType, httpResponse, httpResponse.Request, error));
        }
        
        public Task<IHttpResponse> BuildResponseWithDataAndErrorAsync(object? data, Type dataType, object? error, Type errorType, IHttpResponse httpResponse)
        {
            var genericResponseType = typeof(HttpResponseWithError<,>).MakeGenericType(dataType, errorType);
            return Task.FromResult((IHttpResponse)Activator.CreateInstance(genericResponseType, httpResponse, httpResponse.Request, data, error));
        }
    }
}
