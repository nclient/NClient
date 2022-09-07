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

        public static MethodInfo[] GetAllInterfaceMethods(this Type type, bool inherit = false)
        {
            if (!type.IsInterface)
                throw new ArgumentException("Type is not interface.", nameof(type));
            if (inherit == false)
                return type.GetMethods().ToArray();

            var methodInfos = new HashSet<MethodInfo>(type.GetMethods());
            foreach (var interfaceType in type.GetDeclaredInterfaces())
            {
                foreach (var methodInfo in GetAllInterfaceMethods(interfaceType, inherit: true))
                {
                    methodInfos.Add(methodInfo);
                }
            }

            return methodInfos.ToArray();
        }

        public static MethodInfo[] GetUnhiddenInterfaceMethods(this Type type, bool inherit = false)
        {
            var methods = GetAllInterfaceMethods(type, inherit);
            var filterSet = new HashSet<MethodInfo>();

            foreach (var method in methods)
            {
                if (method.IsHideBySig) //shadows by signature
                {

                    if (!SetContainsMethod(filterSet, method))
                        filterSet.Add(method);
                }
            }
            return filterSet.ToArray(); 
        }

        private static bool SetContainsMethod(HashSet<MethodInfo> set, MethodInfo method)
        {
            if (set.Count == 0)
                return false;

            foreach (var element in set)
            {
                if (method.Name == element.Name)
                {
                    var methodParams = method.GetParameters();
                    var elementParams = element.GetParameters();

                    if (methodParams.Length == 0)
                        return true;

                    if (methodParams.Length == elementParams.Length)
                    {

                        for (int i = 0; i < methodParams.Length; i++)
                        {
                            var methodParam = methodParams[i];
                            var elementParam = elementParams[i];
                            if (methodParam.ParameterType == elementParam.ParameterType)
                            {
                                if (methodParam.Name == elementParam.Name)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    
                }
            }
            return false;
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
