using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers.ObjectMemberManagers.Factories;
using NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors;

namespace NClient.Core.Helpers.ObjectMemberManagers
{
    internal interface IObjectMemberManager
    {
        MemberInfo[] GetPublic(object obj);
        MemberInfo GetByName(object obj, string memberName, IMemberNameSelector memberNameSelector);
        object? GetValue(object obj, string memberPath, IMemberNameSelector memberNameSelector);
        object? GetValue(MemberInfo member, object obj);
        void SetValue(object obj, string? value, string memberPath, IMemberNameSelector memberNameSelector);
        void SetValue(object obj, MemberInfo member, string? value);
        void SetValue(MemberInfo member, object obj, object? value);
        bool IsMemberPath(string str);
        (string ObjectName, string? MemberPath) ParseNextPath(string memberPath);
        Type GetType(MemberInfo member);
    }

    internal class ObjectMemberManager : IObjectMemberManager
    {
        private readonly IObjectMemberManagerExceptionFactory _exceptionFactory;

        public ObjectMemberManager(IObjectMemberManagerExceptionFactory exceptionFactory)
        {
            _exceptionFactory = exceptionFactory;
        }
        
        public MemberInfo[] GetPublic(object obj)
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


        public MemberInfo GetByName(object obj, string memberName, IMemberNameSelector memberNameSelector)
        {
            var memberNamePairs = GetPublic(obj)
                .Select(member => (Name: memberNameSelector.GetName(member), Member: member))
                .Where(member => member.Name == memberName)
                .ToArray();
            if (memberNamePairs.Length > 1)
                throw _exceptionFactory.MemberNameConflict(memberName, obj.GetType().Name);

            var memberNamePair = memberNamePairs.SingleOrDefault();
            if (memberNamePair.Member is null)
                throw _exceptionFactory.MemberNotFound(memberName, obj.GetType().Name);
            return memberNamePair.Member;
        }

        public object? GetValue(object obj, string memberPath, IMemberNameSelector memberNameSelector)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));
            if (memberPath is null)
                throw new ArgumentNullException(nameof(memberPath));
            if (memberPath == "")
                throw new ArgumentException("Empty string.", nameof(memberPath));

            var iterationCount = 0;
            while (true)
            {
                AvoidInfiniteLoop(iterationCount, obj.GetType().Name);

                var (nextObjName, nextMemberPath) = ParseNextPath(memberPath);
                if (nextMemberPath is not null)
                {
                    var member = GetByName(obj, nextObjName, memberNameSelector);
                    obj = GetValue(member, obj)
                          ?? throw _exceptionFactory.MemberValueOfObjectInRouteIsNull(member.Name, obj.GetType().Name);
                    memberPath = nextMemberPath;
                }
                else
                {
                    var member = GetByName(obj, memberPath, memberNameSelector);
                    return GetValue(member, obj);
                }

                iterationCount++;
            }
        }

        public object? GetValue(MemberInfo member, object obj)
        {
            return (member as PropertyInfo)?.GetValue(obj) ?? (member as FieldInfo)?.GetValue(obj);
        }

        public void SetValue(object obj, string? value, string memberPath, IMemberNameSelector memberNameSelector)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));
            if (memberPath is null)
                throw new ArgumentNullException(nameof(memberPath));
            if (memberPath == "")
                throw new ArgumentException("Empty string.", nameof(memberPath));

            var iterationCount = 0;
            while (true)
            {
                AvoidInfiniteLoop(iterationCount, obj.GetType().Name);

                var (nextObjName, nextMemberPath) = ParseNextPath(memberPath);
                if (nextMemberPath is not null)
                {
                    var member = GetByName(obj, nextObjName, memberNameSelector);
                    obj = GetValue(member, obj)
                          ?? throw _exceptionFactory.MemberValueOfObjectInRouteIsNull(member.Name, obj.GetType().Name);
                    memberPath = nextMemberPath;
                }
                else
                {
                    var member = GetByName(obj, memberPath, memberNameSelector);
                    SetValue(obj, member, value);
                    return;
                }

                iterationCount++;
            }
        }

        public void SetValue(object obj, MemberInfo member, string? value)
        {
            var memberType = GetType(member);
            var converter = TypeDescriptor.GetConverter(memberType);
            if (!converter.IsValid(value))
                throw _exceptionFactory.RoutePropertyConvertError(member.Name, memberType.Name, value);
            SetValue(member, obj, converter.ConvertFrom(value));
        }

        public void SetValue(MemberInfo member, object obj, object? value)
        {
            (member as PropertyInfo)?.SetValue(obj, value);
            (member as FieldInfo)?.SetValue(obj, value);
        }

        public bool IsMemberPath(string str)
        {
            return str.Contains(".");
        }

        public (string ObjectName, string? MemberPath) ParseNextPath(string memberPath)
        {
            const int partsCount = 2;
            var memberPathParts = memberPath.Split(new[] { '.' }, partsCount);
            var objectName = memberPathParts[0];
            var nextMemberPath = memberPathParts.Length == partsCount ? memberPathParts[1] : null;
            return (objectName, nextMemberPath);
        }

        public Type GetType(MemberInfo member)
        {
            return (member as PropertyInfo)?.PropertyType ?? (member as FieldInfo)?.FieldType!;
        }

        private void AvoidInfiniteLoop(int iterationCount, string processingObjectName)
        {
            const int iterationLimit = 10;
            if (iterationCount > iterationLimit)
                throw _exceptionFactory.LimitNestingOfObjects(iterationLimit, processingObjectName);
        }
    }
}
