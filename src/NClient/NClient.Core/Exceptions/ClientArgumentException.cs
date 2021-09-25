using System;
using System.Reflection;

namespace NClient.Core.Exceptions
{
    public class ClientArgumentException : ClientException
    {
        public ClientArgumentException(string message) : base(message)
        {
        }

        public ClientArgumentException(string message, Type interfaceType, MethodInfo methodInfo) : base(message, interfaceType, methodInfo)
        {
        }

        public ClientArgumentException(string message, Type interfaceType, MethodInfo methodInfo, Exception innerException) : base(message, interfaceType, methodInfo, innerException)
        {
        }
    }
}
