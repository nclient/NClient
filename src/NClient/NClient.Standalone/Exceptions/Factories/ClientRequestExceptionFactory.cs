using System;
using System.Reflection;
using NClient.Exceptions;

namespace NClient.Standalone.Exceptions.Factories
{
    public interface IClientRequestExceptionFactory
    {
        ClientRequestException WrapException(Type interfaceType, MethodInfo methodInfo, Exception exception);
    }

    public class ClientRequestExceptionFactory : IClientRequestExceptionFactory
    {
        public ClientRequestException WrapException(Type interfaceType, MethodInfo methodInfo, Exception exception) =>
            new(exception.Message, interfaceType, methodInfo, exception);
    }
}
