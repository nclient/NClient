using System;

// ReSharper disable once CheckNamespace
namespace NClient.Exceptions
{
    /// <summary>Represents exceptions thrown during client creation.</summary>
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
