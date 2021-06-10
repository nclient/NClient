using System;
using NClient.Abstractions.Exceptions;
using NClient.Core.MethodBuilders.Models;

namespace NClient.Core.Exceptions
{
    /// <summary>
    /// Represents exceptions to return information about an client-side errors.
    /// </summary>
    public abstract class ClientException : NClientException
    {
        /// <summary>
        /// The type of client interface.
        /// </summary>
        public Type InterfaceType { get; set; } = null!;

        /// <summary>
        /// The method of client.
        /// </summary>
        public Method Method { get; set; } = null!;

        public ClientException(string message)
            : base(message)
        {
        }

        public ClientException(string message, Type interfaceType, Method method)
            : base(message)
        {
            InterfaceType = interfaceType;
            Method = method;
        }

        protected ClientException(string message, Type interfaceType, Method method, Exception innerException)
            : base(message, innerException)
        {
            InterfaceType = interfaceType;
            Method = method;
        }
    }
}