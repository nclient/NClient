using System;
using System.Net;

namespace NClient.AspNetCore.Exceptions
{
    /// <summary>Represents exceptions to return the HTTP status code that are processed in the action filter.</summary>
    public class HttpResponseException : Exception
    {
        /// <summary>The HTTP status code.</summary>
        public HttpStatusCode Status { get; }

        /// <summary>The response content.</summary>
        public object? Value { get; }

        /// <summary>Initializes a new instance of the exception with a specified HTTP status code.</summary>
        /// <param name="status">The HTTP status code.</param>
        public HttpResponseException(HttpStatusCode status)
        {
            Status = status;
        }

        /// <summary>Initializes a new instance of the exception with a specified HTTP status code and response content.</summary>
        /// <param name="status">The HTTP status code.</param>
        /// <param name="value">The response content.</param>
        public HttpResponseException(HttpStatusCode status, object value) : this(status)
        {
            Value = value;
        }
    }
}
