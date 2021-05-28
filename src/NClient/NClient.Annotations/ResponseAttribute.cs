using System;
using System.Net;
using NClient.Common.Helpers;

namespace NClient.Annotations
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ResponseAttribute : Attribute
    {
        public Type Type { get; }
        public HttpStatusCode StatusCode { get; }

        public ResponseAttribute(HttpStatusCode statusCode) : this(typeof(void), statusCode)
        {
        }

        public ResponseAttribute(Type type, HttpStatusCode statusCode)
        {
            Ensure.IsNotNull(type, nameof(type));

            Type = type;
            StatusCode = statusCode;
        }
    }
}