using System;
using System.Reflection;
using NClient.Abstractions.Exceptions;

// ReSharper disable once CheckNamespace
namespace NClient.Exceptions
{
    /// <summary>
    /// Represents exceptions to return information about a failed client request.
    /// </summary>
    public class ClientRequestException<TResponse> : ClientException
    {
        /// <summary>
        /// The HTTP response.
        /// </summary>
        public TResponse? Response { get; }

        /// <summary>
        /// Shows HTTP error or not.
        /// </summary>
        public bool IsHttpError => InnerException is HttpClientException;

        public ClientRequestException(string message, Type interfaceType, MethodInfo methodInfo, Exception innerException)
            : base(message, interfaceType, methodInfo, innerException)
        {
        }

        public ClientRequestException(string message, Type interfaceType, MethodInfo methodInfo, TResponse response, Exception innerException)
            : base(message, interfaceType, methodInfo, innerException)
        {
            Response = response;
        }
    }
}
