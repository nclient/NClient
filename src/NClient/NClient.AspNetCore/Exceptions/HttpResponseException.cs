using System;
using System.Net;

namespace NClient.AspNetCore.Exceptions
{
    public class HttpResponseException : Exception
    {
        public HttpStatusCode Status { get; }
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