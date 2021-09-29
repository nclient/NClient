using System;
using System.Net.Http;
using NClient.Abstractions.HttpClients;

namespace NClient.Providers.HttpClient.System
{
    public class SystemHttpClientExceptionFactory : IHttpClientExceptionFactory<HttpRequestMessage, HttpResponseMessage>
    {
        public HttpClientException<HttpRequestMessage, HttpResponseMessage> HttpRequestFailed(
            HttpRequestMessage request, HttpResponseMessage response, Exception innerException)
        {
            return new HttpClientException<HttpRequestMessage, HttpResponseMessage>(request, response, innerException.Message, innerException);
        }
    }
}
