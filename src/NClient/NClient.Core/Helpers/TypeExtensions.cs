using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Core.Exceptions.Factories;

namespace NClient.Core.Helpers
{
    internal static class TypeExtensions
    {
        public static bool IsPrimitive(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
                return IsPrimitive(typeInfo.GetGenericArguments()[0]);

            return typeInfo.IsPrimitive
                   || typeInfo.IsEnum
                   || type == typeof(string)
                   || type == typeof(decimal)
                   || type == typeof(Guid);
        }

        public static Attribute[] GetInterfaceCustomAttributes(this Type type, bool inherit = false)
        {
            if (!type.IsInterface)
                throw InnerExceptionFactory.ArgumentException(nameof(type), "Type is not interface.");
            if (inherit == false)
                return type.GetCustomAttributes().ToArray();

            var attributes = new HashSet<Attribute>(type.GetCustomAttributes());
            foreach (var interfaceType in type.GetInterfaces())
            {
                foreach (var attribute in GetInterfaceCustomAttributes(interfaceType, inherit: true))
                {
                    attributes.Add(attribute);
                }
            }

            return attributes.ToArray();
        }
    }
}
