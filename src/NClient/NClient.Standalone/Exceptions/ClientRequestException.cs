using System;
using System.Reflection;
using NClient.Abstractions.Exceptions;
using NClient.Abstractions.HttpClients;

// ReSharper disable once CheckNamespace
namespace NClient.Exceptions
{
    /// <summary>
    /// Represents exceptions to return information about a failed client request.
    /// </summary>
    public class ClientRequestException : ClientException
    {
        /// <summary>
        /// The HTTP request that the response belongs to.
        /// </summary>
        public IHttpRequest? HttpRequest { get; }

        /// <summary>
        /// The HTTP response.
        /// </summary>
        public IHttpResponse? HttpResponse { get; }

        /// <summary>
        /// Shows HTTP error or not.
        /// </summary>
        public bool IsHttpError => InnerException is HttpClientException;

        public ClientRequestException(string message, Type interfaceType, MethodInfo methodInfo, Exception innerException)
            : base(message, interfaceType, methodInfo, innerException)
        {
        }

        public ClientRequestException(string message, Type interfaceType, MethodInfo methodInfo, IHttpResponse httpResponse, Exception innerException)
            : base(message, interfaceType, methodInfo, innerException)
        {
            HttpRequest = httpResponse.Request;
            HttpResponse = httpResponse;
        }
    }
}
