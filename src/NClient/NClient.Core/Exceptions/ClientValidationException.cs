using System;
using NClient.Core.MethodBuilders.Models;

namespace NClient.Core.Exceptions
{
    /// <summary>
    /// Represents exceptions to return information about an invalid client interface.
    /// </summary>
    public class ClientValidationException : ClientException
    {
        public ClientValidationException(string message) : base(message)
        {
        }

        public ClientValidationException(string message, Type interfaceType, Method method)
            : base(message, interfaceType, method)
        {
        }
    }
}