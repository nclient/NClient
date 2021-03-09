using System;

namespace NClient.Core.Exceptions
{
    public class InvalidRouteNClientException : NClientException
    {
        public InvalidRouteNClientException()
        {
        }

        public InvalidRouteNClientException(string message) : base(message)
        {
        }

        public InvalidRouteNClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
