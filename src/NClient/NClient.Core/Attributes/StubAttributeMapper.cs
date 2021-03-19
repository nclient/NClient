using System;

namespace NClient.Core.Attributes
{
    public class StubAttributeMapper : IAttributeMapper
    {
        public Attribute? TryMap(Attribute attribute)
        {
            return attribute;
        }
    }
}
