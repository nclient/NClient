using System;
using NClient.Abstractions.Exceptions;

namespace NClient.Abstractions.HttpClients
{
    /// <summary>
    /// Represents exceptions thrown by NClient client during the processing of the HTTP request.
    /// </summary>
    public class HttpClientException : NClientException
    {
        public HttpClientException(string message) : base(message)
        {
        }
        public HttpClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    
    /// <summary>
    /// Represents exceptions thrown by NClient client during the processing of the HTTP request.
    /// </summary>
    public class HttpClientException<TRequest, TResponse> : HttpClientException
    {
        /// <summary>
        /// The HTTP request that the response belongs to.
        /// </summary>
        public TRequest Request { get; }

        /// <summary>
        /// The HTTP response.
        /// </summary>
        public TResponse Response { get; }

        public HttpClientException(TRequest request, TResponse response, string errorMessage) : base(errorMessage)
        {
            Request = request;
            Response = response;
        }
        
        public HttpClientException(TRequest request, TResponse response, string errorMessage, Exception innerException) : base(errorMessage, innerException)
        {
            Request = request;
            Response = response;
        }
    }
}
