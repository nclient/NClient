using System;

namespace NClient.Core.Mappers
{
    internal interface IAttributeMapper
    {
        Attribute? TryMap(Attribute attribute);
    }

    internal class AttributeMapper : IAttributeMapper
    {
        public Attribute TryMap(Attribute attribute)
        {
            return attribute switch
            {
                { } => attribute,
                _ => throw new ArgumentNullException(nameof(attribute))
            };
        }
    }
}
