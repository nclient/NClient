using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NClient.Core.Helpers
{
    internal class MethodInfoEqualityComparer : IEqualityComparer<MethodInfo>
    {
        public bool Equals(MethodInfo left, MethodInfo right)
        {
            if (ReferenceEquals(left, right)) 
                return true;
            if (left.GetType() != right.GetType()) 
                return false;
            
            return left.MemberType == right.MemberType 
                   && left.Name == right.Name 
                   && left.GetParameters().Select(x => x.ParameterType)
                       .SequenceEqual(right.GetParameters().Select(x => x.ParameterType))
                   && left.ReturnType == right.ReturnType;
        }

        public int GetHashCode(MethodInfo obj)
        {
            unchecked
            {
                var hashCode = (int) obj.MemberType;
                hashCode = (hashCode * 397) ^ obj.Name.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.ReturnType.GetHashCode();
                return hashCode;
            }
        }
    }
}