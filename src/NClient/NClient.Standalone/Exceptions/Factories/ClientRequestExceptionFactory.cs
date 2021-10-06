using System;
using System.Reflection;
using NClient.Abstractions.Exceptions;
using NClient.Abstractions.HttpClients;
using NClient.Exceptions;

namespace NClient.Standalone.Exceptions.Factories
{
    public interface IClientRequestExceptionFactory
    {
        ClientRequestException WrapClientHttpRequestException(Type interfaceType, MethodInfo methodInfo, HttpResponse httpResponse, HttpClientException httpClientException);
        ClientRequestException WrapException(Type interfaceType, MethodInfo methodInfo, Exception exception);
        ClientRequestException WrapException(Type interfaceType, MethodInfo methodInfo, HttpResponse httpResponse, Exception exception);
    }

    public class ClientRequestExceptionFactory : IClientRequestExceptionFactory
    {
        public ClientRequestException WrapClientHttpRequestException(Type interfaceType, MethodInfo methodInfo, HttpResponse httpResponse, HttpClientException httpClientException) =>
            new(httpClientException.Message, interfaceType, methodInfo, httpResponse, httpClientException);

        public ClientRequestException WrapException(Type interfaceType, MethodInfo methodInfo, Exception exception) =>
            new(exception.Message, interfaceType, methodInfo, exception);

        public ClientRequestException WrapException(Type interfaceType, MethodInfo methodInfo, HttpResponse httpResponse, Exception exception) =>
            new(exception.Message, interfaceType, methodInfo, httpResponse, exception);
    }
}
