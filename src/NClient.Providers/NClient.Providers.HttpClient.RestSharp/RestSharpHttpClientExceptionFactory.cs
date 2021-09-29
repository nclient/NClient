using System;
using NClient.Abstractions.HttpClients;
using RestSharp;

namespace NClient.Providers.HttpClient.RestSharp
{
    public class RestSharpHttpClientExceptionFactory : IHttpClientExceptionFactory<IRestRequest, IRestResponse>
    {
        public HttpClientException<IRestRequest, IRestResponse> HttpRequestFailed(
            IRestRequest request, IRestResponse response, Exception innerException)
        {
            return new HttpClientException<IRestRequest, IRestResponse>(request, response, innerException.Message, innerException);
        }
    }
}
