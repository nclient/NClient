using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace NClient.Exceptions
{
    /// <summary>Represents exceptions to return information about a failed client request.</summary>
    public class ClientRequestException : ClientException
    {
        /// <summary>Shows HTTP error or not.</summary>
        public bool IsHttpError => InnerException is TransportException;

        /// <summary>Initializes a new instance of the exception with a specified error message and client info.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="interfaceType">The type of client interface.</param>
        /// <param name="methodInfo">The invoked method info of client.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ClientRequestException(string message, Type interfaceType, MethodInfo methodInfo, Exception innerException)
            : base(message, interfaceType, methodInfo, innerException)
        {
        }
    }
}
