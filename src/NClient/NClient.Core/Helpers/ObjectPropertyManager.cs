using System.ComponentModel;
using System.Reflection;
using NClient.Core.Exceptions.Factories;

namespace NClient.Core.Helpers
{
    internal static class ObjectPropertyManager
    {
        public static bool IsPropertyPath(string str)
        {
            return str.Contains(".");
        }

        public static (string ObjectName, string? PropertyPath) ParseNextPath(string propertyPath)
        {
            const int partsCount = 2;
            var propertyPathParts = propertyPath.Split(new[] { '.' }, partsCount);
            var objectName = propertyPathParts[0];
            var nextPropertyPath = propertyPathParts.Length == partsCount ? propertyPathParts[1] : null;
            return (objectName, nextPropertyPath);
        }

        public static object? GetPropertyValue(object obj, string propertyPath)
        {
            var iterationCount = 0;
            while (true)
            {
                AvoidInfiniteLoop(iterationCount, obj.GetType().Name);

                var (nextObjName, nextPropertyPath) = ParseNextPath(propertyPath);
                if (nextPropertyPath is not null)
                {
                    var property = GetPropertyByName(obj, nextObjName);
                    obj = property!.GetValue(obj) 
                          ?? throw OuterExceptionFactory.PropertyValueOfObjectInRouteIsNull(property.Name, obj.GetType().Name);
                    propertyPath = nextPropertyPath;
                }
                else
                {
                    var property = GetPropertyByName(obj, propertyPath);
                    return property.GetValue(obj);
                }

                iterationCount++;
            }
        }

        public static void SetPropertyValue(object obj, object? value, string propertyPath)
        {
            var iterationCount = 0;
            while (true)
            {
                AvoidInfiniteLoop(iterationCount, obj.GetType().Name);

                var (nextObjName, nextPropertyPath) = ParseNextPath(propertyPath);
                if (nextPropertyPath is not null)
                {
                    var property = GetPropertyByName(obj, nextObjName);
                    obj = property!.GetValue(obj)
                          ?? throw OuterExceptionFactory.PropertyValueOfObjectInRouteIsNull(property.Name, obj.GetType().Name);
                    propertyPath = nextPropertyPath;
                }
                else
                {
                    var property = GetPropertyByName(obj, propertyPath);
                    SetPropertyValue(obj, property, value);
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

        private static PropertyInfo GetPropertyByName(object obj, string propertyName)
        {
            var property = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property is null)
                throw OuterExceptionFactory.PropertyNotFound(propertyName, obj.GetType().Name);
            return property;
        }

        private static void SetPropertyValue(object obj, PropertyInfo property, object? value)
        {
            var converter = TypeDescriptor.GetConverter(property.PropertyType);
            if (!converter.IsValid(value))
                throw OuterExceptionFactory.RoutePropertyConvertError(property.Name, property.PropertyType.Name, value?.ToString());
            property.SetValue(obj, converter.ConvertFrom(value));
        }
    }
}
