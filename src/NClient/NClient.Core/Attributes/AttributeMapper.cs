using System;
using NClient.Core.Exceptions.Factories;

namespace NClient.Core.Attributes
{
    public interface IAttributeMapper
    {
        Attribute? TryMap(Attribute attribute);
    }

    public class AttributeMapper : IAttributeMapper
    {
        public Attribute TryMap(Attribute attribute)
        {
            return attribute switch
            {
                { } => attribute,
                _ => throw InnerExceptionFactory.NullArgument(nameof(attribute))
            };
        }
    }
}
