using System;
using NClient.Abstractions.HttpClients;

namespace NClient.Core.HttpClients
{
    public class StubHttpClientExceptionFactory : IHttpClientExceptionFactory<HttpRequest, HttpResponse, Exception>
    {
        public HttpClientException<HttpRequest, HttpResponse>? TryCreate(HttpRequest? request, HttpResponse? response, Exception? innerException)
        {
            if (request is null || response is null || innerException is null)
                return null;
            
            return Create(request, response, innerException);
        }
        
        public HttpClientException<HttpRequest, HttpResponse> Create(HttpRequest request, HttpResponse response, Exception innerException)
        {
            return new HttpClientException<HttpRequest, HttpResponse>(request, response, errorMessage: "Error in stub", innerException);
        }
    }
}
