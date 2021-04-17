using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers.MemberNameSelectors;

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

        public static object? GetMemberValue(object obj, string memberPath, IMemberNameSelector memberNameSelector)
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
                    var member = GetMemberByName(obj, nextObjName, memberNameSelector);
                    obj = GetMemberValue(member, obj)
                          ?? throw OuterExceptionFactory.MemberValueOfObjectInRouteIsNull(member.Name, obj.GetType().Name);
                    memberPath = nextMemberPath;
                }
                else
                {
                    var member = GetMemberByName(obj, memberPath, memberNameSelector);
                    return GetMemberValue(member, obj);
                }

                iterationCount++;
            }
        }

        public static void SetMemberValue(object obj, string? value, string memberPath, IMemberNameSelector memberNameSelector)
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
                    var member = GetMemberByName(obj, nextObjName, memberNameSelector);
                    obj = GetMemberValue(member, obj)
                          ?? throw OuterExceptionFactory.MemberValueOfObjectInRouteIsNull(member.Name, obj.GetType().Name);
                    memberPath = nextMemberPath;
                }
                else
                {
                    var member = GetMemberByName(obj, memberPath, memberNameSelector);
                    SetMemberValue(obj, member, value);
                    return;
                }

                iterationCount++;
            }
        }

        public static MemberInfo[] GetPublicMembers(object obj)
        {
            var properties = obj
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Cast<MemberInfo>();

            var fields = obj
                .GetType()
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Cast<MemberInfo>();

            return properties.Concat(fields).ToArray();
        }

        public static MemberInfo GetMemberByName(object obj, string memberName, IMemberNameSelector memberNameSelector)
        {
            var memberNamePairs = GetPublicMembers(obj)
                .Select(member => (Name: memberNameSelector.GetName(member), Member: member))
                .Where(member => member.Name == memberName)
                .ToArray();
            if (memberNamePairs.Length > 1)
                throw OuterExceptionFactory.MemberNameConflict(memberName, obj.GetType().Name);

            var memberNamePair = memberNamePairs.SingleOrDefault();
            if (memberNamePair.Member is null)
                throw OuterExceptionFactory.MemberNotFound(memberName, obj.GetType().Name);
            return memberNamePair.Member;
        }

        public static void SetMemberValue(object obj, MemberInfo member, string? value)
        {
            var memberType = GetMemberType(member);
            var converter = TypeDescriptor.GetConverter(memberType);
            if (!converter.IsValid(value))
                throw OuterExceptionFactory.RoutePropertyConvertError(member.Name, memberType.Name, value);
            SetMemberValue(member, obj, converter.ConvertFrom(value));
        }

        public static Type GetMemberType(MemberInfo member)
        {
            return (member as PropertyInfo)?.PropertyType ?? (member as FieldInfo)?.FieldType!;
        }

        public static object? GetMemberValue(MemberInfo member, object obj)
        {
            return (member as PropertyInfo)?.GetValue(obj) ?? (member as FieldInfo)?.GetValue(obj);
        }

        public static void SetMemberValue(MemberInfo member, object obj, object? value)
        {
            (member as PropertyInfo)?.SetValue(obj, value);
            (member as FieldInfo)?.SetValue(obj, value);
        }
        
        private static void AvoidInfiniteLoop(int iterationCount, string processingObjectName)
        {
            const int iterationLimit = 10;
            if (iterationCount > iterationLimit)
                throw OuterExceptionFactory.LimitNestingOfObjects(iterationLimit, processingObjectName);
        }
    }
}
