using System;

// ReSharper disable once CheckNamespace
namespace NClient.Exceptions
{
    /// <summary>Represents exceptions thrown by NClient client during the processing of the transport request.</summary>
    public class TransportException : NClientException
    {
        /// <summary>Initializes a new instance of the exception with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public TransportException(string message) : base(message)
        {
        }
        
        /// <summary>Initializes a new instance of the exception with a specified error message and a reference to the inner exception that is the cause of this exception..</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public TransportException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    
    /// <summary>Represents exceptions thrown by NClient client during the processing of the transport request.</summary>
    public class TransportException<TRequest, TResponse> : TransportException
    {
        /// <summary>Gets the transport request that the response belongs to.</summary>
        public TRequest Request { get; }

        /// <summary>Gets the transport response.</summary>
        public TResponse Response { get; }

        /// <summary>Initializes a new instance of the exception with a specified error message, request and response.</summary>
        /// <param name="request">The transport request.</param>
        /// <param name="response">The transport response.</param>
        /// <param name="message">The message that describes the error.</param>
        public TransportException(TRequest request, TResponse response, string message) 
            : base(message)
        {
            Request = request;
            Response = response;
        }
        
        /// <summary>Initializes a new instance of the exception with a specified error message, request, response and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="request">The transport request.</param>
        /// <param name="response">The transport response.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public TransportException(TRequest request, TResponse response, string message, Exception innerException) 
            : base(message, innerException)
        {
            Request = request;
            Response = response;
        }
    }
}
