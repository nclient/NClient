using System;

namespace NClient.Core.Exceptions
{
    public class RequestNClientException : NClientException
    {
        private readonly string _originMessage;

        public override string Message => $"{_originMessage} Client name: {ClientName}. Method name: {MethodName}.";

        public string ClientName { get; private set; } = "";
        public string MethodName { get; private set; } = "";

        public RequestNClientException(string message)
        {
            _originMessage = message;
        }

        public RequestNClientException(string message, Exception innerException) : base(message, innerException)
        {
            _originMessage = message;
        }

        public Exception WithRequestInfo(string clientName, string methodName)
        {
            ClientName = clientName;
            MethodName = methodName;
            return this;
        }
    }
}