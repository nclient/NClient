using System;
using System.Net;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;

namespace NClient.Core.HttpClients
{
    internal class StubHttpClient : IHttpClient
    {
        public Task<HttpResponse> ExecuteAsync(HttpRequest request, Type? bodyType = null, Type? errorType = null)
        {
            var response = new HttpResponse(request) { StatusCode = HttpStatusCode.OK };
            
            if (bodyType is null && errorType is not null)
            {
                var genericResponseType = typeof(HttpResponseWithError<>).MakeGenericType(errorType);
                return Task.FromResult((HttpResponse)Activator.CreateInstance(genericResponseType, response, request, null));
            }
            
            if (bodyType is not null && errorType is null)
            {
                var genericResponseType = typeof(HttpResponse<>).MakeGenericType(bodyType);
                return Task.FromResult((HttpResponse)Activator.CreateInstance(genericResponseType, response, request, null));
            }
            
            if (bodyType is not null && errorType is not null)
            {
                var genericResponseType = typeof(HttpResponseWithError<,>).MakeGenericType(bodyType, errorType);
                return Task.FromResult((HttpResponse)Activator.CreateInstance(genericResponseType, response, request, null, null));
            }
            
            return Task.FromResult(response);
        }
    }
}
