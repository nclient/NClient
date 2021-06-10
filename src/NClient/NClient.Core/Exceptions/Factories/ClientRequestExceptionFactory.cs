using System;
using NClient.Abstractions.Exceptions;
using NClient.Core.MethodBuilders.Models;

namespace NClient.Core.Exceptions.Factories
{
    public interface IClientRequestExceptionFactory
    {
        ClientRequestException WrapClientHttpRequestException(Method method, ClientHttpRequestException httpRequestException);
        ClientRequestException WrapException(Method method, Exception exception);
    }

    public class ClientRequestExceptionFactory : IClientRequestExceptionFactory
    {
        public ClientRequestException WrapClientHttpRequestException(Method method, ClientHttpRequestException httpRequestException) =>
            new(httpRequestException.Message, method, httpRequestException);

        public ClientRequestException WrapException(Method method, Exception exception) =>
            new(exception.Message, method, exception);
    }
}