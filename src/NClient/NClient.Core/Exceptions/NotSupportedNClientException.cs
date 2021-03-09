using System;

namespace NClient.Core.Exceptions
{
    public class NotSupportedNClientException : NClientException
    {
        public NotSupportedNClientException()
        {
        }

        public NotSupportedNClientException(string message) : base(message)
        {
        }

        public NotSupportedNClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
