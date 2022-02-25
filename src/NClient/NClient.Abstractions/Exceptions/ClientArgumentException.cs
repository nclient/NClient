using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace NClient.Exceptions
{
    /// <summary>Represents exceptions thrown due to invalid arguments passed to a client methods.</summary>
    public class ClientArgumentException : ClientException
    {
        /// <summary>Initializes a new instance of the exception with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ClientArgumentException(string message) : base(message)
        {
        }

        /// <summary>Initializes a new instance of the exception with a specified error message and client info.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="interfaceType">The type of client interface.</param>
        /// <param name="methodInfo">The client method info.</param>
        public ClientArgumentException(string message, Type interfaceType, MethodInfo methodInfo) : base(message, interfaceType, methodInfo)
        {
        }

        /// <summary>Initializes a new instance of the exception with a specified error message, client info and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="interfaceType">The type of client interface.</param>
        /// <param name="methodInfo">The client method info.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ClientArgumentException(string message, Type interfaceType, MethodInfo methodInfo, Exception innerException) : base(message, interfaceType, methodInfo, innerException)
        {
        }
    }
}
