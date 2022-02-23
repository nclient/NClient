using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace NClient.Exceptions
{
    /// <summary>Represents exceptions to return information about an invalid client interface.</summary>
    public class ClientValidationException : ClientException
    {
        /// <summary>Initializes a new instance of the exception with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ClientValidationException(string message) : base(message)
        {
        }

        /// <summary>Initializes a new instance of the exception with a specified error message and client info.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="interfaceType">The type of client interface.</param>
        /// <param name="methodInfo">The client method info.</param>
        public ClientValidationException(string message, Type interfaceType, MethodInfo methodInfo)
            : base(message, interfaceType, methodInfo)
        {
        }
    }
}
