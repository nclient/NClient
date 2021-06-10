using System;
using NClient.Abstractions.Exceptions;
using NClient.Core.MethodBuilders.Models;

namespace NClient.Core.Exceptions
{
    public class ClientRequestException : ClientException
    {
        public ClientHttpRequestException? HttpRequestException { get; }

        public bool IsHttpError => HttpRequestException is not null;

        public ClientRequestException(string message, ClientHttpRequestException? httpRequestException = null)
            : base(message)
        {
            HttpRequestException = httpRequestException;
        }

        public ClientRequestException(string message, Method method, ClientHttpRequestException? httpRequestException = null)
            : base(message, method, httpRequestException!)
        {
            HttpRequestException = httpRequestException;
        }

        public ClientRequestException(string message, Method method, Exception innerException)
            : base(message, method, innerException)
        {
        }
    }
}