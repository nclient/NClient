using System;
using NClient.Abstractions.Exceptions;
using NClient.Core.MethodBuilders.Models;

namespace NClient.Core.Exceptions.Factories
{
    public static class ClientRequestExceptionFactory
    {
        public static ClientRequestException WrapClientHttpRequestException(Method method, ClientHttpRequestException httpRequestException) =>
            new(httpRequestException.Message, method, httpRequestException);
        
        public static ClientRequestException WrapException(Method method, Exception exception) =>
            new(exception.Message, method, exception);
    }
}