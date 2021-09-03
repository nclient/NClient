using System;
using System.Net;

namespace NClient.AspNetCore.Exceptions
{
    /// <summary>
    /// Represents exceptions to return the HTTP status code that are processed in the action filter.
    /// </summary>
    public class HttpResponseException : Exception
    {
        /// <summary>
        /// The HTTP status code.
        /// </summary>
        public HttpStatusCode Status { get; }

        /// <summary>
        /// The response content.
        /// </summary>
        public object? Value { get; }

        public HttpResponseException(HttpStatusCode status)
        {
            Status = status;
        }

        public HttpResponseException(HttpStatusCode status, object value) : this(status)
        {
            Value = value;
        }
    }
}
