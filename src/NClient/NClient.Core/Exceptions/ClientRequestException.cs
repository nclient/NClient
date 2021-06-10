using System;
using NClient.Abstractions.Exceptions;
using NClient.Core.MethodBuilders.Models;

namespace NClient.Core.Exceptions
{
    /// <summary>
    /// Represents exceptions to return information about a failed client request.
    /// </summary>
    public class ClientRequestException : ClientException
    {
        /// <summary>
        /// The HTTP request error.
        /// </summary>
        public ClientHttpRequestException? HttpRequestException { get; }

        /// <summary>
        /// Shows HTTP error or not.
        /// </summary>
        public bool IsHttpError => HttpRequestException is not null;

        public ClientRequestException(string message, ClientHttpRequestException? httpRequestException = null)
            : base(message)
        {
            HttpRequestException = httpRequestException;
        }

        public ClientRequestException(string message, Type interfaceType, Method method, ClientHttpRequestException? httpRequestException = null)
            : base(message, interfaceType, method, httpRequestException!)
        {
            HttpRequestException = httpRequestException;
        }

        public ClientRequestException(string message, Type interfaceType, Method method, Exception innerException)
            : base(message, interfaceType, method, innerException)
        {
        }
    }
}