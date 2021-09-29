using System;

namespace NClient.Abstractions.HttpClients
{
    public interface IHttpClientExceptionFactory<TRequest, TResponse, TException> : IHttpClientExceptionFactory<TRequest, TResponse> 
        where TException : Exception
    {
        HttpClientException<TRequest, TResponse> Create(TRequest request, TResponse response, TException innerException);
    }
    
    public interface IHttpClientExceptionFactory<TRequest, TResponse>
    {
        HttpClientException<TRequest, TResponse>? TryCreate(TRequest? request, TResponse? response, Exception? innerException);
    }
}
