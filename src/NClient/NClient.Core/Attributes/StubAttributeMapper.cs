using System;
using NClient.Core.Exceptions.Factories;

namespace NClient.Core.Attributes
{
    public class StubAttributeMapper : IAttributeMapper
    {
        public Attribute? TryMap(Attribute attribute)
        {
            if (attribute is null)
                throw InnerExceptionFactory.NullArgument(nameof(attribute));

            return attribute;
        }
    }
}
