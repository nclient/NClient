﻿using System.Reflection;

namespace NClient.Core.Helpers.EqualityComparers
{
    internal class MethodInfoEqualityComparer : OverridingMethodInfoEqualityComparer
    {
        public override bool Equals(MethodInfo left, MethodInfo right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (left.GetType() != right.GetType())
                return false;

            return base.Equals(left, right) && left.ReturnType == right.ReturnType;
        }

        public override int GetHashCode(MethodInfo obj)
        {
            unchecked
            {
                var hashCode = base.GetHashCode(obj);
                hashCode = (hashCode * 397) ^ obj.ReturnType.GetHashCode();
                return hashCode;
            }
        }
    }
}
