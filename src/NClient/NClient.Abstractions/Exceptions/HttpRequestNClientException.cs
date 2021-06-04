using System;
using System.Net;
using NClient.Abstractions.Exceptions.Providers;

namespace NClient.Abstractions.Exceptions
{
    /// <summary>
    /// Represents exceptions thrown by NClient client during the processing of the HTTP request.
    /// </summary>
    public class HttpRequestNClientException : NClientException, IClientInfoProviderException
    {
        private readonly string _formattedMessage;

        public HttpStatusCode HttpStatusCode { get; }
        public override string Message => $"{_formattedMessage} Client name: {ClientName}. Method name: {MethodName}.";

        public string ClientName { get; private set; } = "";
        public string MethodName { get; private set; } = "";

        public HttpRequestNClientException(HttpStatusCode httpStatusCode, string? message, string? content, Exception innerException)
            : base(message: "", innerException)
        {
            HttpStatusCode = httpStatusCode;
            _formattedMessage = $"The request completed with status {httpStatusCode} (http code: {(int)httpStatusCode}). Error message: '{message ?? ""}'. Content: '{content}'.";
        }

        public Exception WithRequestInfo(string clientName, string methodName)
        {
            ClientName = clientName;
            MethodName = methodName;
            return this;
        }
    }
}
