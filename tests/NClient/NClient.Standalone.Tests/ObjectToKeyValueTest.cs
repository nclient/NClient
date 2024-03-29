﻿using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors;
using NClient.Core.Helpers.ObjectToKeyValueConverters;
using NClient.Exceptions;
using NClient.Providers.Api.Rest.Exceptions.Factories;
using NUnit.Framework;

namespace NClient.Standalone.Tests
{
    [Parallelizable]
    internal class ObjectToKeyValueTest
    {
        private static readonly ObjectMemberManagerExceptionFactory ClientObjectMemberManagerExceptionFactory = new();
        private static readonly ObjectToKeyValueConverterExceptionFactory ObjectToKeyValueConverterExceptionFactory = new();
        private ObjectToKeyValueConverter _objectToKeyValue = null!;

        [SetUp]
        public void SetUp()
        {
            var objectMemberManager = new ObjectMemberManager(ClientObjectMemberManagerExceptionFactory);
            _objectToKeyValue = new ObjectToKeyValueConverter(objectMemberManager, ObjectToKeyValueConverterExceptionFactory);
        }

        public static IEnumerable ConvertibleObjectSourceWithPrefix = new[]
        {
            new TestCaseData(null, new PropertyKeyValue[]
            {
                new("obj", null)
            }).SetName("Null"),
            new TestCaseData(true, new PropertyKeyValue[]
            {
                new("obj", true)
            }).SetName("Bool"),
            new TestCaseData(1, new PropertyKeyValue[]
            {
                new("obj", 1)
            }).SetName("Int32"),
            new TestCaseData(1.2f, new PropertyKeyValue[]
            {
                new("obj", 1.2f)
            }).SetName("Float"),
            new TestCaseData(1.2d, new PropertyKeyValue[]
            {
                new("obj", 1.2d)
            }).SetName("Double"),
            new TestCaseData(1.2m, new PropertyKeyValue[]
            {
                new("obj", 1.2m)
            }).SetName("Decimal"),
            new TestCaseData(new int?(), new PropertyKeyValue[]
            {
                new("obj", null)
            }).SetName("Nullable int32: null"),
            new TestCaseData((int?) 1, new PropertyKeyValue[]
            {
                new("obj", 1)
            }).SetName("Nullable int32: has value"),
            new TestCaseData("str", new PropertyKeyValue[]
            {
                new("obj", "str")
            }).SetName("String"),
            new TestCaseData(new DateTime(2020, 1, 2, 3, 4, 5), new PropertyKeyValue[]
            {
                new("obj", new DateTime(2020, 1, 2, 3, 4, 5))
            }).SetName("DateTime"),
            new TestCaseData(new DateTimeOffset(new DateTime(2020, 1, 2, 3, 4, 5)), new PropertyKeyValue[]
            {
                new("obj", new DateTimeOffset(new DateTime(2020, 1, 2, 3, 4, 5)))
            }).SetName("BoDateTimeOffset"),
            new TestCaseData(UriKind.Absolute, new PropertyKeyValue[]
            {
                new("obj", UriKind.Absolute)
            }).SetName("Enum"),
            new TestCaseData(Guid.Parse("fa467be0-fcae-4935-8ecb-fa1cf720104b"), new PropertyKeyValue[]
            {
                new("obj", Guid.Parse("fa467be0-fcae-4935-8ecb-fa1cf720104b"))
            }).SetName("Guid"),
            new TestCaseData(new[] { 1 }, new PropertyKeyValue[]
            {
                new("obj", 1)
            }).SetName("Array of int32: single item"),
            new TestCaseData(new[] { 1, 2, 3 }, new PropertyKeyValue[]
            {
                new("obj", 1),
                new("obj", 2),
                new("obj", 3)
            }).SetName("Array of int32: multiple items"),
            new TestCaseData(new[] { "str1", "str2", "str3" }, new PropertyKeyValue[]
            {
                new("obj", "str1"),
                new("obj", "str2"),
                new("obj", "str3")
            }).SetName("Array of strings: multiple items"),
            new TestCaseData(new Dictionary<string, int> { ["str1"] = 1 }, new PropertyKeyValue[]
            {
                new("obj[str1]", 1)
            }).SetName("Dictionary of strings/int32: single item"),
            new TestCaseData(new Dictionary<string, int> { ["str1"] = 1, ["str2"] = 2, ["str3"] = 3 }, new PropertyKeyValue[]
            {
                new("obj[str1]", 1),
                new("obj[str2]", 2),
                new("obj[str3]", 3)
            }).SetName("Dictionary of strings/int32: multiple items"),
            new TestCaseData(new Dictionary<int, string> { [1] = "str1", [2] = "str2", [3] = "str3" }, new PropertyKeyValue[]
            {
                new("obj[1]", "str1"),
                new("obj[2]", "str2"),
                new("obj[3]", "str3")
            }).SetName("Dictionary of int32/strings: multiple items"),
            new TestCaseData(new { Prop1 = 1 }, new PropertyKeyValue[]
            {
                new("obj.Prop1", 1)
            }).SetName("Custom object: single property"),
            new TestCaseData(new { Prop1 = 1, Prop2 = "str" }, new PropertyKeyValue[]
            {
                new("obj.Prop1", 1),
                new("obj.Prop2", "str")
            }).SetName("Custom object: multiple properties"),
            new TestCaseData(new { Prop1 = new { Prop2 = 1, Prop3 = 2 } }, new PropertyKeyValue[]
            {
                new("obj.Prop1.Prop2", 1),
                new("obj.Prop1.Prop3", 2)
            }).SetName("Custom object: nested custom object"),
            new TestCaseData(new { Prop1 = new { Prop2 = new { Prop3 = 1, Prop4 = 2 } } }, new PropertyKeyValue[]
            {
                new("obj.Prop1.Prop2.Prop3", 1),
                new("obj.Prop1.Prop2.Prop4", 2)
            }).SetName("Custom object: deep nested custom objects"),
            new TestCaseData(new { Prop1 = new[] { 1, 2, 3 } }, new PropertyKeyValue[]
            {
                new("obj.Prop1", 1),
                new("obj.Prop1", 2),
                new("obj.Prop1", 3)
            }).SetName("Custom object: array of int32 property"),
            new TestCaseData(new { Prop1 = new[] { "str1", "str2", "str3" } }, new PropertyKeyValue[]
            {
                new("obj.Prop1", "str1"),
                new("obj.Prop1", "str2"),
                new("obj.Prop1", "str3")
            }).SetName("Custom object: array of strings property"),
            new TestCaseData(new { Prop1 = new Dictionary<string, int> { ["str1"] = 1, ["str2"] = 2, ["str3"] = 3 } }, new PropertyKeyValue[]
            {
                new("obj.Prop1[str1]", 1),
                new("obj.Prop1[str2]", 2),
                new("obj.Prop1[str3]", 3)
            }).SetName("Custom object: dictionary of int32 property"),
            new TestCaseData(new { Prop1 = new Dictionary<int, string> { [1] = "str1", [2] = "str2", [3] = "str3" } }, new PropertyKeyValue[]
            {
                new("obj.Prop1[1]", "str1"),
                new("obj.Prop1[2]", "str2"),
                new("obj.Prop1[3]", "str3")
            }).SetName("Custom object: dictionary of strings property")
        };
        
        public static IEnumerable ConvertibleObjectSourceWithoutRootPrefix = new[]
        {
            new TestCaseData(null, new PropertyKeyValue[]
            {
                new("obj", null)
            }).SetName("Null"),
            new TestCaseData(true, new PropertyKeyValue[]
            {
                new("obj", true)
            }).SetName("Bool"),
            new TestCaseData(1, new PropertyKeyValue[]
            {
                new("obj", 1)
            }).SetName("Int32"),
            new TestCaseData(1.2f, new PropertyKeyValue[]
            {
                new("obj", 1.2f)
            }).SetName("Float"),
            new TestCaseData(1.2d, new PropertyKeyValue[]
            {
                new("obj", 1.2d)
            }).SetName("Double"),
            new TestCaseData(1.2m, new PropertyKeyValue[]
            {
                new("obj", 1.2m)
            }).SetName("Decimal"),
            new TestCaseData(new int?(), new PropertyKeyValue[]
            {
                new("obj", null)
            }).SetName("Nullable int32: null"),
            new TestCaseData((int?) 1, new PropertyKeyValue[]
            {
                new("obj", 1)
            }).SetName("Nullable int32: has value"),
            new TestCaseData("str", new PropertyKeyValue[]
            {
                new("obj", "str")
            }).SetName("String"),
            new TestCaseData(new DateTime(2020, 1, 2, 3, 4, 5), new PropertyKeyValue[]
            {
                new("obj", new DateTime(2020, 1, 2, 3, 4, 5))
            }).SetName("DateTime"),
            new TestCaseData(new DateTimeOffset(new DateTime(2020, 1, 2, 3, 4, 5)), new PropertyKeyValue[]
            {
                new("obj", new DateTimeOffset(new DateTime(2020, 1, 2, 3, 4, 5)))
            }).SetName("BoDateTimeOffset"),
            new TestCaseData(UriKind.Absolute, new PropertyKeyValue[]
            {
                new("obj", UriKind.Absolute)
            }).SetName("Enum"),
            new TestCaseData(Guid.Parse("fa467be0-fcae-4935-8ecb-fa1cf720104b"), new PropertyKeyValue[]
            {
                new("obj", Guid.Parse("fa467be0-fcae-4935-8ecb-fa1cf720104b"))
            }).SetName("Guid"),
            new TestCaseData(new[] { 1 }, new PropertyKeyValue[]
            {
                new("obj", 1)
            }).SetName("Array of int32: single item"),
            new TestCaseData(new[] { 1, 2, 3 }, new PropertyKeyValue[]
            {
                new("obj", 1),
                new("obj", 2),
                new("obj", 3)
            }).SetName("Array of int32: multiple items"),
            new TestCaseData(new[] { "str1", "str2", "str3" }, new PropertyKeyValue[]
            {
                new("obj", "str1"),
                new("obj", "str2"),
                new("obj", "str3")
            }).SetName("Array of strings: multiple items"),
            new TestCaseData(new Dictionary<string, int> { ["str1"] = 1 }, new PropertyKeyValue[]
            {
                new("str1", 1)
            }).SetName("Dictionary of strings/int32: single item"),
            new TestCaseData(new Dictionary<string, int> { ["str1"] = 1, ["str2"] = 2, ["str3"] = 3 }, new PropertyKeyValue[]
            {
                new("str1", 1),
                new("str2", 2),
                new("str3", 3)
            }).SetName("Dictionary of strings/int32: multiple items"),
            new TestCaseData(new Dictionary<int, string> { [1] = "str1", [2] = "str2", [3] = "str3" }, new PropertyKeyValue[]
            {
                new("1", "str1"),
                new("2", "str2"),
                new("3", "str3")
            }).SetName("Dictionary of int32/strings: multiple items"),
            new TestCaseData(new { Prop1 = 1 }, new PropertyKeyValue[]
            {
                new("Prop1", 1)
            }).SetName("Custom object: single property"),
            new TestCaseData(new { Prop1 = 1, Prop2 = "str" }, new PropertyKeyValue[]
            {
                new("Prop1", 1),
                new("Prop2", "str")
            }).SetName("Custom object: multiple properties"),
            new TestCaseData(new { Prop1 = new { Prop2 = 1, Prop3 = 2 } }, new PropertyKeyValue[]
            {
                new("Prop1.Prop2", 1),
                new("Prop1.Prop3", 2)
            }).SetName("Custom object: nested custom object"),
            new TestCaseData(new { Prop1 = new { Prop2 = new { Prop3 = 1, Prop4 = 2 } } }, new PropertyKeyValue[]
            {
                new("Prop1.Prop2.Prop3", 1),
                new("Prop1.Prop2.Prop4", 2)
            }).SetName("Custom object: deep nested custom objects"),
            new TestCaseData(new { Prop1 = new[] { 1, 2, 3 } }, new PropertyKeyValue[]
            {
                new("Prop1", 1),
                new("Prop1", 2),
                new("Prop1", 3)
            }).SetName("Custom object: array of int32 property"),
            new TestCaseData(new { Prop1 = new[] { "str1", "str2", "str3" } }, new PropertyKeyValue[]
            {
                new("Prop1", "str1"),
                new("Prop1", "str2"),
                new("Prop1", "str3")
            }).SetName("Custom object: array of strings property"),
            new TestCaseData(new { Prop1 = new Dictionary<string, int> { ["str1"] = 1, ["str2"] = 2, ["str3"] = 3 } }, new PropertyKeyValue[]
            {
                new("Prop1[str1]", 1),
                new("Prop1[str2]", 2),
                new("Prop1[str3]", 3)
            }).SetName("Custom object: dictionary of int32 property"),
            new TestCaseData(new { Prop1 = new Dictionary<int, string> { [1] = "str1", [2] = "str2", [3] = "str3" } }, new PropertyKeyValue[]
            {
                new("Prop1[1]", "str1"),
                new("Prop1[2]", "str2"),
                new("Prop1[3]", "str3")
            }).SetName("Custom object: dictionary of strings property")
        };

        public static IEnumerable NotSupportedObjectSource = new[]
        {
            new TestCaseData(new[] { new { Prop1 = 1 } },
                    ObjectToKeyValueConverterExceptionFactory.ArrayWithComplexTypeNotSupported())
                .SetName("Array of custom objects: single item"),
            new TestCaseData(new[] { new { Prop1 = 1 }, new { Prop1 = 1 } },
                    ObjectToKeyValueConverterExceptionFactory.ArrayWithComplexTypeNotSupported())
                .SetName("Array of custom objects: multiple items"),
            new TestCaseData(new { Prop1 = new[] { new { Prop2 = 1 }, new { Prop2 = 2 } } },
                    ObjectToKeyValueConverterExceptionFactory.ArrayWithComplexTypeNotSupported())
                .SetName("Custom object: array of custom objects property"),
            new TestCaseData(new Dictionary<int, object> { [1] = new { Prop1 = 1 } },
                    ObjectToKeyValueConverterExceptionFactory.DictionaryWithComplexTypeOfValueNotSupported())
                .SetName("Dictionary of int32/custom objects: single item"),
            new TestCaseData(new Dictionary<int, object> { [1] = new { Prop1 = 1 }, [2] = new { Prop1 = 2 } },
                    ObjectToKeyValueConverterExceptionFactory.DictionaryWithComplexTypeOfValueNotSupported())
                .SetName("Dictionary of int32/custom objects: multiple items"),
            new TestCaseData(new { Prop1 = new Dictionary<int, object> { [1] = new { Prop1 = 1 }, [2] = new { Prop1 = 2 } } },
                    ObjectToKeyValueConverterExceptionFactory.DictionaryWithComplexTypeOfValueNotSupported())
                .SetName("Custom object: dictionary of int32/custom objects property"),
            new TestCaseData(new Dictionary<object, int> { [new { Prop1 = 1 }] = 1 },
                    ObjectToKeyValueConverterExceptionFactory.DictionaryWithComplexTypeOfKeyNotSupported())
                .SetName("Dictionary of custom objects/int32: single item"),
            new TestCaseData(new Dictionary<object, int> { [new { Prop1 = 1 }] = 1, [new { Prop1 = 2 }] = 2 },
                    ObjectToKeyValueConverterExceptionFactory.DictionaryWithComplexTypeOfKeyNotSupported())
                .SetName("Dictionary of custom objects/int32: multiple items"),
            new TestCaseData(new { Prop1 = new Dictionary<object, int> { [new { Prop1 = 1 }] = 1, [new { Prop1 = 2 }] = 2 } },
                    ObjectToKeyValueConverterExceptionFactory.DictionaryWithComplexTypeOfKeyNotSupported())
                .SetName("Custom object: dictionary of custom objects/int32 property")
        };

        [TestCaseSource(nameof(ConvertibleObjectSourceWithPrefix))]
        public void Convert_ConvertibleObjectWithRootPrefix_ArrayOfKeyValue(object obj, PropertyKeyValue[] expectedResult)
        {
            var actualResult = _objectToKeyValue.Convert(obj, rootName: "obj", new DefaultMemberNameSelector());

            actualResult
                .Should()
                .BeEquivalentTo(expectedResult, config => config.WithoutStrictOrdering());
        }
        
        [TestCaseSource(nameof(ConvertibleObjectSourceWithoutRootPrefix))]
        public void Convert_ConvertibleObjectWithoutRootPrefix_ArrayOfKeyValue(object obj, PropertyKeyValue[] expectedResult)
        {
            var actualResult = _objectToKeyValue.Convert(obj, rootName: "obj", new DefaultMemberNameSelector(), useRootNameAsPrefix: false);

            actualResult
                .Should()
                .BeEquivalentTo(expectedResult, config => config.WithoutStrictOrdering());
        }
        
        [TestCaseSource(nameof(NotSupportedObjectSource))]
        public void Convert_NotSupportedObjectWithRootPrefix_ThrowClientValidationException(object obj, Exception exception)
        {
            _objectToKeyValue
                .Invoking(x => x.Convert(obj, rootName: "obj", new DefaultMemberNameSelector()))
                .Should()
                .Throw<ClientValidationException>()
                .Where(x => x.GetType() == exception.GetType())
                .WithMessage(exception.Message);
        }

        [TestCaseSource(nameof(NotSupportedObjectSource))]
        public void Convert_NotSupportedObjectWithoutRootPrefix_ThrowClientValidationException(object obj, Exception exception)
        {
            _objectToKeyValue
                .Invoking(x => x.Convert(obj, rootName: "obj", new DefaultMemberNameSelector(), useRootNameAsPrefix: false))
                .Should()
                .Throw<ClientValidationException>()
                .Where(x => x.GetType() == exception.GetType())
                .WithMessage(exception.Message);
        }
    }
}
