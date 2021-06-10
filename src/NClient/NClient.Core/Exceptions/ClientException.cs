using System;
using NClient.Abstractions.Exceptions;
using NClient.Core.MethodBuilders.Models;

namespace NClient.Core.Exceptions
{
    public abstract class ClientException : NClientException
    {
        public Method Method { get; set; } = null!;

        public ClientException(string message)
            : base(message)
        {
        }

        public ClientException(string message, Method method)
            : base(message)
        {
            Method = method;
        }

        protected ClientException(string message, Method method, Exception innerException)
            : base(message, innerException)
        {
            Method = method;
        }
    }
}