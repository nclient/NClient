using System;
using System.Net;

namespace NClient.Annotations
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ResponseAttribute : Attribute
    {
        public Type Type { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ResponseAttribute(HttpStatusCode statusCode) : this(typeof(void), statusCode)
        {
        }

        public ResponseAttribute(Type type, HttpStatusCode statusCode)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            Type = type;
            StatusCode = statusCode;
        }
    }
}