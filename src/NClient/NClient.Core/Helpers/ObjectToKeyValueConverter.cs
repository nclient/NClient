using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NClient.Core.Exceptions.Factories;

namespace NClient.Core.Helpers
{
    public record PropertyKeyValue(string Key, object? Value);

    internal interface IObjectToKeyValueConverter
    {
        PropertyKeyValue[] Convert(object? obj, string rootName);
    }

    internal class ObjectToKeyValueConverter : IObjectToKeyValueConverter
    {
        public PropertyKeyValue[] Convert(object? obj, string rootName)
        {
            var stringValues = new List<PropertyKeyValue>();
            return ToKeyValue(stringValues, rootName, obj).ToArray();
        }

        internal static List<PropertyKeyValue> ToKeyValue(List<PropertyKeyValue> stringValues, string key, object? value)
        {
            if (TryAddAsPrimitive(stringValues, key, value))
                return stringValues;

            if (TryAddAsEnumerable(stringValues, key, value))
                return stringValues;

            if (TryAddAsObject(stringValues, key, value))
                return stringValues;

            stringValues.Add(new PropertyKeyValue(key, value));
            return stringValues;
        }

        internal static bool TryAddAsPrimitive(List<PropertyKeyValue> stringValues, string key, object? value)
        {
            if (!IsPrimitive(value))
                return false;

            stringValues.Add(new PropertyKeyValue(key, value));
            return true;
        }

        internal static bool TryAddAsEnumerable(List<PropertyKeyValue> stringValues, string key, object? value)
        {
            if (IsPrimitive(value) || value is not IEnumerable enumerable)
                return false;

            foreach (var item in enumerable)
            {
                if (enumerable is IDictionary)
                {
                    var itemKey = item.GetType().GetProperty("Key")!.GetValue(item, null);
                    if (!IsPrimitive(itemKey))
                        throw OuterExceptionFactory.DictionaryWithComplexTypeOfKeyNotSupported();

                    var itemValue = item.GetType().GetProperty("Value")!.GetValue(item, null);
                    if (!TryAddAsPrimitive(stringValues, key: $"{key}[{itemKey}]", itemValue))
                        throw OuterExceptionFactory.DictionaryWithComplexTypeOfValueNotSupported();
                }
                else
                {
                    if (!TryAddAsPrimitive(stringValues, key, item))
                        throw OuterExceptionFactory.ArrayWithComplexTypeNotSupported();
                }
            }

            return true;
        }

        internal static bool TryAddAsObject(List<PropertyKeyValue> stringValues, string key, object? value)
        {
            if (IsPrimitive(value))
                return false;

            foreach (var (propName, propValue) in GetProperties(value))
            {
                ToKeyValue(stringValues, key: key + "." + propName, propValue);
            }

            return true;
        }

        private static bool IsPrimitive(object? obj)
        {
            if (obj is null || obj is string)
                return true;

            if (obj.GetType().IsAssignableTo(typeof(IEnumerable)))
                return false;

            return obj.GetType().IsSerializable;
        }

        private static IEnumerable<PropertyKeyValue> GetProperties(object? obj)
        {
            return obj?.GetType()
                .GetProperties()
                .Where(property => property.CanRead)
                .Select(property => new PropertyKeyValue(property.Name, property.GetValue(obj)))
                .ToArray() ?? Array.Empty<PropertyKeyValue>();
        }
    }
}