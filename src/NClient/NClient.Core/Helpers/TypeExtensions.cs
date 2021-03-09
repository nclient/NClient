using System;
using System.Reflection;

namespace NClient.Core.Helpers
{
    internal static class TypeExtensions
    {
        public static bool IsSimple(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
                return IsSimple(typeInfo.GetGenericArguments()[0]);

            return typeInfo.IsPrimitive
                   || typeInfo.IsEnum
                   || type == typeof(string)
                   || type == typeof(decimal);
        }
    }
}
