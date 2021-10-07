using System;
using System.Reflection;
using NClient.Abstractions.Exceptions;

// ReSharper disable once CheckNamespace
namespace NClient.Exceptions
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
        public MethodInfo MethodInfo { get; set; } = null!;

        protected ClientException(string message)
            : base(message)
        {
        }

        protected ClientException(string message, Type interfaceType, MethodInfo methodInfo)
            : base(message)
        {
            InterfaceType = interfaceType;
            MethodInfo = methodInfo;
        }

        protected ClientException(string message, Type interfaceType, MethodInfo methodInfo, Exception innerException)
            : base(message, innerException)
        {
            InterfaceType = interfaceType;
            MethodInfo = methodInfo;
        }
    }
}
