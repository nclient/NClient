using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers.MemberNameSelectors;

namespace NClient.Core.Helpers
{
    internal interface IObjectToKeyValueConverter
    {
        PropertyKeyValue[] Convert(object? obj, string rootName, IMemberNameSelector memberNameSelector);
    }

    internal class ObjectToKeyValueConverter : IObjectToKeyValueConverter
    {
        public PropertyKeyValue[] Convert(object? obj, string rootName, IMemberNameSelector memberNameSelector)
        {
            var stringValues = new List<PropertyKeyValue>();
            return ToKeyValue(stringValues, rootName, obj, memberNameSelector).ToArray();
        }

        private static List<PropertyKeyValue> ToKeyValue(List<PropertyKeyValue> stringValues, string key, object? value, IMemberNameSelector memberNameSelector)
        {
            if (TryAddAsPrimitive(stringValues, key, value))
                return stringValues;

            if (TryAddAsEnumerable(stringValues, key, value))
                return stringValues;

            if (TryAddAsObject(stringValues, key, value, memberNameSelector))
                return stringValues;

            stringValues.Add(new PropertyKeyValue(key, value));
            return stringValues;
        }

        private static bool TryAddAsPrimitive(List<PropertyKeyValue> stringValues, string key, object? value)
        {
            if (!IsPrimitive(value))
                return false;

            stringValues.Add(new PropertyKeyValue(key, value));
            return true;
        }

        private static bool TryAddAsEnumerable(List<PropertyKeyValue> stringValues, string key, object? value)
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

        private static bool TryAddAsObject(List<PropertyKeyValue> stringValues, string key, object? value, IMemberNameSelector memberNameSelector)
        {
            if (IsPrimitive(value))
                return false;

            foreach (var prop in GetProperties(value, memberNameSelector))
            {
                ToKeyValue(stringValues, key: key + "." + prop.Key, prop.Value, memberNameSelector);
            }

            return true;
        }

        private static bool IsPrimitive(object? obj)
        {
            if (obj is null || obj is string)
                return true;

            if (obj is IEnumerable)
                return false;

            return obj.GetType().IsSerializable;
        }

        private static IEnumerable<PropertyKeyValue> GetProperties(object? obj, IMemberNameSelector memberNameSelector)
        {
            return obj?.GetType()
                .GetProperties()
                .Where(property => property.CanRead)
                .Select(property => new PropertyKeyValue(memberNameSelector.GetName(property), property.GetValue(obj)))
                .ToArray() ?? Array.Empty<PropertyKeyValue>();
        }
    }

    public class PropertyKeyValue
    {
        public string Key { get; }
        public object? Value { get; }

        public PropertyKeyValue(string key, object? value)
        {
            Key = key;
            Value = value;
        }
    }
}