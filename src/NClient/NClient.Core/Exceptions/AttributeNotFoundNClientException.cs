using System;
using System.Reflection;

namespace NClient.Core.Exceptions
{
    /// <summary>
    /// Represents exceptions thrown by NClient client if a required attribute is not found.
    /// </summary>
    public class AttributeNotFoundNClientException : RequestNClientException
    {
        public AttributeNotFoundNClientException(string message) : base(message)
        {
        }
    }
}
