using System;

// ReSharper disable once CheckNamespace
namespace NClient.Exceptions
{
    /// <summary>
    /// Represents exceptions thrown by NClient client during the processing of the HTTP request.
    /// </summary>
    public class TransportException : NClientException
    {
        public TransportException(string message) : base(message)
        {
        }
        public TransportException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    
    /// <summary>
    /// Represents exceptions thrown by NClient client during the processing of the HTTP request.
    /// </summary>
    public class TransportException<TRequest, TResponse> : TransportException
    {
        /// <summary>
        /// The HTTP request that the response belongs to.
        /// </summary>
        public TRequest Request { get; }

        /// <summary>
        /// The HTTP response.
        /// </summary>
        public TResponse Response { get; }

        public TransportException(TRequest request, TResponse response, string errorMessage) : base(errorMessage)
        {
            Request = request;
            Response = response;
        }
        
        public TransportException(TRequest request, TResponse response, string errorMessage, Exception innerException) : base(errorMessage, innerException)
        {
            Request = request;
            Response = response;
        }
    }
}
