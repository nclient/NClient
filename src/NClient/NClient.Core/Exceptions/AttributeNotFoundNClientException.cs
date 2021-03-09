using System;
using System.Reflection;

namespace NClient.Core.Exceptions
{
    public class AttributeNotFoundNClientException : NClientException
    {
        public AttributeNotFoundNClientException(Type attributeType, Type client) 
            : base(message: $"Attribute '{attributeType.Name}' not found for target type '{client.Name}'.")
        {
        }

        public AttributeNotFoundNClientException(Type attributeType, MethodInfo method)
            : base(message: $"Attribute '{attributeType.Name}' not found for method '{method.Name}'. Client name: {method.DeclaringType.Name}.")
        {
        }
    }
}
