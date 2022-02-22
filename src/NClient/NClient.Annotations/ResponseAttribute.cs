using System;
using System.Net;
using NClient.Common.Helpers;

namespace NClient.Annotations
{
    /// <summary>A filter that specifies the type of the value and status code returned by the action.</summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ResponseAttribute : Attribute, IResponseAttribute
    {
        /// <summary>Gets or sets the type of the value returned by an action.</summary>
        public Type Type { get; }

        /// <summary>Gets or sets the HTTP status code of the response.</summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>Initializes an instance of <see cref="ResponseAttribute"/>.</summary>       
        /// <param name="statusCode">The HTTP response status code.</param>
        public ResponseAttribute(HttpStatusCode statusCode) : this(typeof(void), statusCode)
        {
        }

        /// <summary>Initializes an instance of <see cref="ResponseAttribute"/>.</summary>
        /// <param name="type">The <see cref="Type"/> of object that is going to be written in the response.</param>
        /// <param name="statusCode">The HTTP response status code.</param>
        public ResponseAttribute(Type type, HttpStatusCode statusCode)
        {
            Ensure.IsNotNull(type, nameof(type));

            Type = type;
            StatusCode = statusCode;
        }
    }
}
