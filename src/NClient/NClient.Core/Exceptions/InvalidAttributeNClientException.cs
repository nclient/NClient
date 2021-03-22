using System;

namespace NClient.Core.Exceptions
{
    public class InvalidAttributeNClientException : NClientException
    {
        public InvalidAttributeNClientException()
        {
        }

        public InvalidAttributeNClientException(string message) : base(message)
        {
        }

        public InvalidAttributeNClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
