using System;
using NClient.Abstractions.Exceptions;
using NClient.Core.MethodBuilders.Models;
using NClient.Exceptions;

namespace NClient.Core.Exceptions.Factories
{
    public interface IClientRequestExceptionFactory
    {
        ClientRequestException WrapClientHttpRequestException(Type interfaceType, Method method, ClientHttpRequestException httpRequestException);
        ClientRequestException WrapException(Type interfaceType, Method method, Exception exception);
    }

    public class ClientRequestExceptionFactory : IClientRequestExceptionFactory
    {
        public ClientRequestException WrapClientHttpRequestException(Type interfaceType, Method method, ClientHttpRequestException httpRequestException) =>
            new(httpRequestException.Message, interfaceType, method, httpRequestException);

        public ClientRequestException WrapException(Type interfaceType, Method method, Exception exception) =>
            new(exception.Message, interfaceType, method, exception);
    }
}