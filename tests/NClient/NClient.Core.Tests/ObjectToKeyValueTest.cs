using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NClient.Core.Helpers;
using NClient.Core.Helpers.MemberNameSelectors;
using NUnit.Framework;
using NotSupportedNClientException = NClient.Core.Exceptions.NotSupportedNClientException;

namespace NClient.Core.Tests
{
    [Parallelizable]
    public class ObjectToKeyValueTest
    {
        public static IEnumerable ConvertibleObjectSource = new[]
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
            new TestCaseData((int?)1, new PropertyKeyValue[]
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
                new("obj", 3),
            }).SetName("Array of int32: multiple items"),
            new TestCaseData(new[] { "str1", "str2", "str3" }, new PropertyKeyValue[]
            {
                new("obj", "str1"),
                new("obj", "str2"),
                new("obj", "str3"),
            }).SetName("Array of strings: multiple items"),
            new TestCaseData(new Dictionary<string, int> { ["str1"] = 1 }, new PropertyKeyValue[]
            {
                new("obj[str1]", 1)
            }).SetName("Dictionary of strings/int32: single item"),
            new TestCaseData(new Dictionary<string, int> { ["str1"] = 1, ["str2"] = 2, ["str3"] = 3 }, new PropertyKeyValue[]
            {
                new("obj[str1]", 1),
                new("obj[str2]", 2),
                new("obj[str3]", 3),
            }).SetName("Dictionary of strings/int32: multiple items"),
            new TestCaseData(new Dictionary<int, string> { [1] = "str1", [2] = "str2", [3] = "str3" }, new PropertyKeyValue[]
            {
                new("obj[1]", "str1"),
                new("obj[2]", "str2"),
                new("obj[3]", "str3"),
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
                new("obj.Prop1", 3),
            }).SetName("Custom object: array of int32 property"),
            new TestCaseData(new { Prop1 = new[] { "str1", "str2", "str3" } }, new PropertyKeyValue[]
            {
                new("obj.Prop1", "str1"),
                new("obj.Prop1", "str2"),
                new("obj.Prop1", "str3"),
            }).SetName("Custom object: array of strings property"),
            new TestCaseData(new { Prop1 = new Dictionary<string, int> { ["str1"] = 1, ["str2"] = 2, ["str3"] = 3 } }, new PropertyKeyValue[]
            {
                new("obj.Prop1[str1]", 1),
                new("obj.Prop1[str2]", 2),
                new("obj.Prop1[str3]", 3),
            }).SetName("Custom object: dictionary of int32 property"),
            new TestCaseData(new { Prop1 = new Dictionary<int, string> { [1] = "str1", [2] = "str2", [3] = "str3" } }, new PropertyKeyValue[]
            {
                new("obj.Prop1[1]", "str1"),
                new("obj.Prop1[2]", "str2"),
                new("obj.Prop1[3]", "str3"),
            }).SetName("Custom object: dictionary of strings property"),
        };

        public static IEnumerable NotSupportedObjectSource = new[]
        {
            new TestCaseData(new[] { new { Prop1 = 1 } }, "")
                .SetName("Array of custom objects: single item"),
            new TestCaseData(new[] { new { Prop1 = 1 }, new { Prop1 = 1 } }, "")
                .SetName("Array of custom objects: multiple items"),
            new TestCaseData(new { Prop1 = new[] { new { Prop2 = 1 }, new { Prop2 = 2 } } }, "")
                .SetName("Custom object: array of custom objects property"),
            new TestCaseData(new Dictionary<int, object> { [1] = new { Prop1 = 1 } }, "")
                .SetName("Dictionary of int32/custom objects: single item"),
            new TestCaseData(new Dictionary<int, object> { [1] = new { Prop1 = 1 }, [2] = new { Prop1 = 2 } }, "")
                .SetName("Dictionary of int32/custom objects: multiple items"),
            new TestCaseData(new { Prop1 = new Dictionary<int, object> { [1] = new { Prop1 = 1 }, [2] = new { Prop1 = 2 } } }, "")
                .SetName("Custom object: dictionary of int32/custom objects property"),
            new TestCaseData(new Dictionary<object, int> { [new { Prop1 = 1 }] = 1 }, "")
                .SetName("Dictionary of custom objects/int32: single item"),
            new TestCaseData(new Dictionary<object, int> { [new { Prop1 = 1 }] = 1, [new { Prop1 = 2 }] = 2 }, "")
                .SetName("Dictionary of custom objects/int32: multiple items"),
            new TestCaseData(new { Prop1 = new Dictionary<object, int> { [new { Prop1 = 1 }] = 1, [new { Prop1 = 2 }] = 2 } }, "")
                .SetName("Custom object: dictionary of custom objects/int32 property")
        };

        [TestCaseSource(nameof(ConvertibleObjectSource))]
        public void Convert_ConvertibleObject_ArrayOfKeyValue(object obj, PropertyKeyValue[] expectedResult)
        {
            var actualResult = new ObjectToKeyValueConverter().Convert(obj, "obj", new DefaultMemberNameSelector());

            actualResult
                .Should()
                .BeEquivalentTo(expectedResult, config => config.WithoutStrictOrdering());
        }

        [TestCaseSource(nameof(NotSupportedObjectSource))]
        public void Convert_NotSupportedObject_ThrowNotSupportedNClientException(object obj, string _)
        {
            new ObjectToKeyValueConverter()
                .Invoking(x => x.Convert(obj, "obj", new DefaultMemberNameSelector()))
                .Should()
                .Throw<NotSupportedNClientException>();
        }
    }
}