using System;
using System.Net.Http;
using NClient.Abstractions.HttpClients;

namespace NClient.Providers.HttpClient.System
{
    public interface ISystemHttpClientExceptionFactory : IHttpClientExceptionFactory<HttpRequestMessage, HttpResponseMessage, HttpRequestException>
    {
    }
    
    public class SystemHttpClientExceptionFactory : ISystemHttpClientExceptionFactory
    {
        public HttpClientException<HttpRequestMessage, HttpResponseMessage> Create(
            HttpRequestMessage request, HttpResponseMessage response, HttpRequestException innerException)
        {
            return new HttpClientException<HttpRequestMessage, HttpResponseMessage>(request, response, innerException.Message, innerException);
        }
        
        public HttpClientException<HttpRequestMessage, HttpResponseMessage>? TryCreate(HttpRequestMessage? request, HttpResponseMessage? response, Exception? innerException)
        {
            if (request is null || response is null || innerException is null)
                return null;
            if (innerException is not HttpRequestException httpRequestException)
                return null;

            return Create(request, response, httpRequestException);
        }
    }
}
