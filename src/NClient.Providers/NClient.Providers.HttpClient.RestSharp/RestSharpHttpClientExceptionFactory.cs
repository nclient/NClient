using System;
using NClient.Abstractions.HttpClients;
using RestSharp;

namespace NClient.Providers.HttpClient.RestSharp
{
    public interface IRestSharpHttpClientExceptionFactory : IHttpClientExceptionFactory<IRestRequest, IRestResponse, Exception>
    {
    }
    
    public class RestSharpHttpClientExceptionFactory : IRestSharpHttpClientExceptionFactory
    {
        public HttpClientException<IRestRequest, IRestResponse> Create(
            IRestRequest request, IRestResponse response, Exception innerException)
        {
            return new HttpClientException<IRestRequest, IRestResponse>(request, response, innerException.Message, innerException);
        }

        public HttpClientException<IRestRequest, IRestResponse>? TryCreate(IRestRequest? request, IRestResponse? response, Exception? innerException)
        {
            if (request is null || response is null || innerException is null)
                return null;

            return Create(request, response, innerException);
        }
    }
}
