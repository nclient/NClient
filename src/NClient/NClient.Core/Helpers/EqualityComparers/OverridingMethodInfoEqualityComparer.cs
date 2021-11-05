using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NClient.Core.Helpers.EqualityComparers
{
    internal class OverridingMethodInfoEqualityComparer : IEqualityComparer<MethodInfo>
    {
        public virtual bool Equals(MethodInfo left, MethodInfo right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (left.GetType() != right.GetType())
                return false;

            return left.MemberType == right.MemberType
                && left.Name == right.Name
                && left.GetParameters().Select(x => x.ParameterType)
                    .SequenceEqual(right.GetParameters().Select(x => x.ParameterType));
        }

        public virtual int GetHashCode(MethodInfo obj)
        {
            unchecked
            {
                var hashCode = (int) obj.MemberType;
                hashCode = (hashCode * 397) ^ obj.Name.GetHashCode();
                return hashCode;
            }
        }
    }
}
