using System;
using NClient.Abstractions.Exceptions;

namespace NClient.Exceptions
{
    public class ClientBuildException : NClientException
    {
        public ClientBuildException(string message) : base(message)
        {
        }
        
        public ClientBuildException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
