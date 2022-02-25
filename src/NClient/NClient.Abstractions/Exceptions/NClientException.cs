using System;

// ReSharper disable once CheckNamespace
namespace NClient.Exceptions
{
    /// <summary>Represents exceptions thrown by the NClient library classes.</summary>
    public class NClientException : Exception
    {
        /// <summary>Initializes a new instance of the NClient exception.</summary>
        public NClientException()
        {
        }

        /// <summary>Initializes a new instance of the NClient exception with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public NClientException(string message) : base(message)
        {
        }

        /// <summary>Initializes a new instance of the NClient exception  with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public NClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
