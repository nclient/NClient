using System;
using System.Reflection;
using NClient.Abstractions.Exceptions;
using NClient.Abstractions.HttpClients;
using NClient.Exceptions;

namespace NClient.Core.Exceptions.Factories
{
    public interface IClientRequestExceptionFactory
    {
        ClientRequestException WrapClientHttpRequestException(Type interfaceType, MethodInfo methodInfo, ClientHttpRequestException httpRequestException);
        ClientRequestException WrapException(Type interfaceType, MethodInfo methodInfo, Exception exception);
        ClientRequestException WrapException(Type interfaceType, MethodInfo methodInfo, HttpResponse httpResponse, Exception exception);
    }

    public class ClientRequestExceptionFactory : IClientRequestExceptionFactory
    {
        public ClientRequestException WrapClientHttpRequestException(Type interfaceType, MethodInfo methodInfo, ClientHttpRequestException httpRequestException) =>
            new(httpRequestException.Message, interfaceType, methodInfo, httpRequestException);

        public ClientRequestException WrapException(Type interfaceType, MethodInfo methodInfo, Exception exception) =>
            new(exception.Message, interfaceType, methodInfo, exception);

        public ClientRequestException WrapException(Type interfaceType, MethodInfo methodInfo, HttpResponse httpResponse, Exception exception) =>
            new(exception.Message, interfaceType, methodInfo, httpResponse, exception);
    }
}