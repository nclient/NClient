using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Core.Helpers.EqualityComparers;

namespace NClient.Core.Helpers
{
    internal static class TypeExtensions
    {
        private static readonly ReferenceAttributeEqualityComparer ReferenceAttributeEqualityComparer = new();
        
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
                throw new ArgumentException("Type is not interface.", nameof(type));
            if (inherit == false)
                return type.GetCustomAttributes().ToArray();

            var attributes = new HashSet<Attribute>(type.GetCustomAttributes(), ReferenceAttributeEqualityComparer);
            foreach (var interfaceType in type.GetDeclaredInterfaces())
            {
                foreach (var attribute in GetInterfaceCustomAttributes(interfaceType, inherit: true))
                {
                    attributes.Add(attribute);
                }
            }

            return attributes.ToArray();
        }

        public static MethodInfo[] GetInterfaceMethods(this Type type, bool inherit = false)
        {
            if (!type.IsInterface)
                throw new ArgumentException("Type is not interface.", nameof(type));
            if (inherit == false)
                return type.GetMethods().ToArray();

            var methodInfos = new HashSet<MethodInfo>(type.GetMethods());
            foreach (var interfaceType in type.GetDeclaredInterfaces())
            {
                foreach (var methodInfo in GetInterfaceMethods(interfaceType, inherit: true))
                {
                    methodInfos.Add(methodInfo);
                }
            }

            return methodInfos.ToArray();
        }

        public static bool HasMultipleInheritance(this Type type)
        {
            var interfaces = type.GetDeclaredInterfaces();
            if (interfaces.Length == 0)
                return false;
            if (interfaces.Length > 1)
                return true;

            return HasMultipleInheritance(interfaces.Single());
        }

        public static Type[] GetDeclaredInterfaces(this Type type)
        {
            var allInterfaces = type.GetInterfaces();
            var selection = allInterfaces
                .Where(x => !allInterfaces.Any(i => i.GetInterfaces().Contains(x)))
                .Except(type.BaseType?.GetInterfaces() ?? Array.Empty<Type>());
            return selection.ToArray();
        }
    }
}
