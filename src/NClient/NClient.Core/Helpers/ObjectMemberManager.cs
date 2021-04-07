using System;
using System.ComponentModel;
using System.Reflection;
using NClient.Core.Exceptions.Factories;

namespace NClient.Core.Helpers
{
    internal static class ObjectMemberManager
    {
        public static bool IsMemberPath(string str)
        {
            return str.Contains(".");
        }

        public static (string ObjectName, string? MemberPath) ParseNextPath(string memberPath)
        {
            const int partsCount = 2;
            var memberPathParts = memberPath.Split(new[] { '.' }, partsCount);
            var objectName = memberPathParts[0];
            var nextMemberPath = memberPathParts.Length == partsCount ? memberPathParts[1] : null;
            return (objectName, nextMemberPath);
        }

        public static object? GetMemberValue(object obj, string memberPath)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));
            if (memberPath is null)
                throw new ArgumentNullException(nameof(memberPath));
            if (memberPath == "")
                throw new ArgumentException("Empty string", nameof(memberPath));

            var iterationCount = 0;
            while (true)
            {
                AvoidInfiniteLoop(iterationCount, obj.GetType().Name);

                var (nextObjName, nextMemberPath) = ParseNextPath(memberPath);
                if (nextMemberPath is not null)
                {
                    var member = GetMemberByName(obj, nextObjName);
                    obj = GetMemberValue(member, obj)
                          ?? throw OuterExceptionFactory.MemberValueOfObjectInRouteIsNull(member.Name, obj.GetType().Name);
                    memberPath = nextMemberPath;
                }
                else
                {
                    var member = GetMemberByName(obj, memberPath);
                    return GetMemberValue(member, obj);
                }

                iterationCount++;
            }
        }

        public static void SetMemberValue(object obj, string? value, string memberPath)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));
            if (memberPath is null)
                throw new ArgumentNullException(nameof(memberPath));
            if (memberPath == "")
                throw new ArgumentException("Empty string", nameof(memberPath));

            var iterationCount = 0;
            while (true)
            {
                AvoidInfiniteLoop(iterationCount, obj.GetType().Name);

                var (nextObjName, nextMemberPath) = ParseNextPath(memberPath);
                if (nextMemberPath is not null)
                {
                    var member = GetMemberByName(obj, nextObjName);
                    obj = GetMemberValue(member, obj)
                          ?? throw OuterExceptionFactory.MemberValueOfObjectInRouteIsNull(member.Name, obj.GetType().Name);
                    memberPath = nextMemberPath;
                }
                else
                {
                    var member = GetMemberByName(obj, memberPath);
                    SetMemberValue(obj, member, value);
                    return;
                }

                iterationCount++;
            }
        }

        private static void AvoidInfiniteLoop(int iterationCount, string processingObjectName)
        {
            const int iterationLimit = 10;
            if (iterationCount > iterationLimit)
                throw OuterExceptionFactory.LimitNestingOfObjects(iterationLimit, processingObjectName);
        }

        private static MemberInfo GetMemberByName(object obj, string memberName)
        {
            MemberInfo? property = obj.GetType().GetProperty(memberName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            MemberInfo? field = obj.GetType().GetField(memberName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property is null && field is null)
                throw OuterExceptionFactory.MemberNotFound(memberName, obj.GetType().Name);
            return property ?? field!;
        }

        private static void SetMemberValue(object obj, MemberInfo member, string? value)
        {
            var memberType = GetMemberType(member);
            var converter = TypeDescriptor.GetConverter(memberType);
            if (!converter.IsValid(value))
                throw OuterExceptionFactory.RoutePropertyConvertError(member.Name, memberType.Name, value);
            SetMemberValue(member, obj, converter.ConvertFrom(value));
        }

        private static Type GetMemberType(MemberInfo member)
        {
            return (member as PropertyInfo)?.PropertyType ?? (member as FieldInfo)?.FieldType!;
        }

        private static object? GetMemberValue(MemberInfo member, object obj)
        {
            return (member as PropertyInfo)?.GetValue(obj) ?? (member as FieldInfo)?.GetValue(obj);
        }

        private static void SetMemberValue(MemberInfo member, object obj, object? value)
        {
            (member as PropertyInfo)?.SetValue(obj, value);
            (member as FieldInfo)?.SetValue(obj, value);
        }
    }
}
