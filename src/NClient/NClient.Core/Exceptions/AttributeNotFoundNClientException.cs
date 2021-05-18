using System;
using System.Reflection;

namespace NClient.Core.Exceptions
{
    public class AttributeNotFoundNClientException : RequestNClientException
    {
        public AttributeNotFoundNClientException(string message) : base(message)
        {
        }
    }
}
