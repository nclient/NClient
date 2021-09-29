using System;

namespace NClient.Abstractions.HttpClients
{
    internal interface IHttpClientExceptionFactory<TRequest, TResponse>
    {
        HttpClientException<TRequest, TResponse> HttpRequestFailed(TRequest request, TResponse response, Exception innerException);
    }
}
