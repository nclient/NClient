using System;
using System.Collections.Generic;

namespace NClient.Core.Helpers
{
    internal class ReferenceAttributeEqualityComparer : IEqualityComparer<Attribute>
    {
        public bool Equals(Attribute left, Attribute right)
        {
            return ReferenceEquals(left, right);
        }
        public int GetHashCode(Attribute attribute)
        {
            return attribute.GetHashCode();
        }
    }
}
