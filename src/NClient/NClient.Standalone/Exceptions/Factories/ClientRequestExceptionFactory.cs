using System;
using System.Reflection;
using NClient.Abstractions.Exceptions;
using NClient.Exceptions;

namespace NClient.Standalone.Exceptions.Factories
{
    public interface IClientRequestExceptionFactory<TResponse>
    {
        ClientRequestException<TResponse> WrapClientHttpRequestException(Type interfaceType, MethodInfo methodInfo, TResponse response, HttpClientException httpClientException);
        ClientRequestException<TResponse> WrapException(Type interfaceType, MethodInfo methodInfo, Exception exception);
        ClientRequestException<TResponse> WrapException(Type interfaceType, MethodInfo methodInfo, TResponse response, Exception exception);
    }

    public class ClientRequestExceptionFactory<TResponse> : IClientRequestExceptionFactory<TResponse>
    {
        public ClientRequestException<TResponse> WrapClientHttpRequestException(Type interfaceType, MethodInfo methodInfo, TResponse response, HttpClientException httpClientException) =>
            new(httpClientException.Message, interfaceType, methodInfo, response, httpClientException);

        public ClientRequestException<TResponse> WrapException(Type interfaceType, MethodInfo methodInfo, Exception exception) =>
            new(exception.Message, interfaceType, methodInfo, exception);

        public ClientRequestException<TResponse> WrapException(Type interfaceType, MethodInfo methodInfo, TResponse response, Exception exception) =>
            new(exception.Message, interfaceType, methodInfo, response, exception);
    }
}
