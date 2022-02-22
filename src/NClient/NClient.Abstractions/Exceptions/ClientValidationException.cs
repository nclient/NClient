using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace NClient.Exceptions
{
    /// <summary>Represents exceptions to return information about an invalid client interface.</summary>
    public class ClientValidationException : ClientException
    {
        public ClientValidationException(string message) : base(message)
        {
        }

        public ClientValidationException(string message, Type interfaceType, MethodInfo methodInfo)
            : base(message, interfaceType, methodInfo)
        {
        }
    }
}
