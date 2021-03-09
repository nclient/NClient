using System;

namespace NClient.Core.Exceptions
{
    public class NClientException : Exception
    {
        public NClientException()
        {
        }

        public NClientException(string message) : base(message)
        {
        }

        public NClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
