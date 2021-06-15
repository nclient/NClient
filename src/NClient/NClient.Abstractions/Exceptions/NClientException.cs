using System;

namespace NClient.Abstractions.Exceptions
{
    /// <summary>
    /// Represents exceptions thrown by the NClient library classes.
    /// </summary>
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
