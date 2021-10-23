using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace NClient.Exceptions
{
    /// <summary>
    /// Represents exceptions to return information about a failed client request.
    /// </summary>
    public class ClientRequestException : ClientException
    {
        /// <summary>
        /// Shows HTTP error or not.
        /// </summary>
        public bool IsHttpError => InnerException is HttpClientException;

        public ClientRequestException(string message, Type interfaceType, MethodInfo methodInfo, Exception innerException)
            : base(message, interfaceType, methodInfo, innerException)
        {
        }
    }
}
