using System;

namespace NClient.Core.Attributes
{
    public interface IAttributeMapper
    {
        Attribute? TryMap(Attribute attribute);
    }
}
