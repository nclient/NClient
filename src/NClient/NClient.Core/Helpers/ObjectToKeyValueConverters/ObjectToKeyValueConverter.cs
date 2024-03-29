﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors;
using NClient.Core.Helpers.ObjectToKeyValueConverters.Factories;

namespace NClient.Core.Helpers.ObjectToKeyValueConverters
{
    internal interface IObjectToKeyValueConverter
    {
        PropertyKeyValue[] Convert(object? obj, string rootName, IMemberNameSelector memberNameSelector, bool useRootNameAsPrefix = true);
    }

    internal class ObjectToKeyValueConverter : IObjectToKeyValueConverter
    {
        private readonly IObjectMemberManager _objectMemberManager;
        private readonly IObjectToKeyValueConverterExceptionFactory _objectToKeyValueConverterExceptionFactory;

        public ObjectToKeyValueConverter(
            IObjectMemberManager objectMemberManager,
            IObjectToKeyValueConverterExceptionFactory objectToKeyValueConverterExceptionFactory)
        {
            _objectMemberManager = objectMemberManager;
            _objectToKeyValueConverterExceptionFactory = objectToKeyValueConverterExceptionFactory;
        }

        public PropertyKeyValue[] Convert(object? obj, string rootName, IMemberNameSelector memberNameSelector, bool useRootNameAsPrefix = true)
        {
            var stringValues = new List<PropertyKeyValue>();
            return ToKeyValue(stringValues, rootName, obj, memberNameSelector, useRootNameAsPrefix).ToArray();
        }

        private List<PropertyKeyValue> ToKeyValue(List<PropertyKeyValue> stringValues, string key, object? value, IMemberNameSelector memberNameSelector, bool useKeyAsPrefix)
        {
            if (TryAddAsPrimitive(stringValues, key, value))
                return stringValues;

            if (TryAddAsEnumerable(stringValues, key, value, useKeyAsPrefix))
                return stringValues;

            if (TryAddAsObject(stringValues, key, value, memberNameSelector, useKeyAsPrefix))
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

        private bool TryAddAsEnumerable(List<PropertyKeyValue> stringValues, string key, object? value, bool useKeyAsPrefix)
        {
            if (IsPrimitive(value) || value is not IEnumerable enumerable)
                return false;

            foreach (var item in enumerable)
            {
                if (enumerable is IDictionary)
                {
                    var itemKey = item.GetType().GetProperty("Key")!.GetValue(item, null);
                    if (!IsPrimitive(itemKey))
                        throw _objectToKeyValueConverterExceptionFactory.DictionaryWithComplexTypeOfKeyNotSupported();

                    var itemKeyWithPrefix = useKeyAsPrefix ? $"{key}[{itemKey}]" : itemKey.ToString();
                    var itemValue = item.GetType().GetProperty("Value")!.GetValue(item, null);
                    if (!TryAddAsPrimitive(stringValues, itemKeyWithPrefix, itemValue))
                        throw _objectToKeyValueConverterExceptionFactory.DictionaryWithComplexTypeOfValueNotSupported();
                }
                else
                {
                    if (!TryAddAsPrimitive(stringValues, key, item))
                        throw _objectToKeyValueConverterExceptionFactory.ArrayWithComplexTypeNotSupported();
                }
            }

            return true;
        }

        private bool TryAddAsObject(List<PropertyKeyValue> stringValues, string key, object? value, IMemberNameSelector memberNameSelector, bool useKeyAsPrefix)
        {
            if (IsPrimitive(value))
                return false;

            foreach (var prop in GetMembers(value, memberNameSelector))
            {
                var keyWithPrefix = useKeyAsPrefix ? $"{key}.{prop.Key}" : prop.Key;
                ToKeyValue(stringValues, keyWithPrefix, prop.Value, memberNameSelector, useKeyAsPrefix: true);
            }

            return true;
        }

        private IEnumerable<PropertyKeyValue> GetMembers(object? obj, IMemberNameSelector memberNameSelector)
        {
            if (obj is null)
                return Array.Empty<PropertyKeyValue>();

            return _objectMemberManager
                .GetPublic(obj)
                .Select(member => new PropertyKeyValue(
                    memberNameSelector.GetName(member),
                    _objectMemberManager.GetValue(member, obj)))
                .ToArray();
        }

        private static bool IsPrimitive(object? obj)
        {
            if (obj is null || obj is string)
                return true;

            if (obj is IEnumerable)
                return false;

            return obj.GetType().IsSerializable;
        }
    }
}
