using System;
using System.Collections;
using System.Text.Json.Serialization;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions;
using NClient.Core.Helpers;
using NClient.Core.Helpers.MemberNameSelectors;
using NUnit.Framework;

namespace NClient.Core.Tests
{
    public class ObjectMemberManagerTest
    {
        public class TestObjWithCustomQueryPropName {[QueryParam(Name = "MyProp")] public int Prop { get; set; } = 1; }
        public class TestObjWithCustomFromQueryName {[FromQuery(Name = "MyProp")] public int Prop { get; set; } = 1; }
        public class TestObjWithCustomJsonPropertyName {[JsonPropertyName("MyProp")] public int Prop { get; set; } = 1; }
        public class TestObjWithMemberNameConflict {[QueryParam(Name = "MyProp")] public int Prop { get; set; } = 1; public int MyProp { get; set; } = 2; }

        public class TestObjWithIntField { public int Field = 1; }
        public class TestObjWithNestedField : TestObjWithIntField { }
        public class TestObjWithIntProp { public int Prop { get; set; } = 1; }
        public class TestObjWithNestedProp : TestObjWithIntProp { }
        public class TestObjWithStringProp { public string Prop { get; set; } = "test"; }
        public class TestObjWithObjectProp { public TestObjWithIntProp Prop { get; set; } = new() { Prop = 1 }; }
        public class TestObjWithDeepObjectProp { public TestObjWithObjectProp Prop { get; set; } = new() { Prop = new() { Prop = 1 } }; }

        public static IEnumerable ValidTestCasesForGetMemberValue = new[]
        {
            new TestCaseData(new TestObjWithCustomQueryPropName { Prop = 1 }, "MyProp", 1, new QueryMemberNameSelector())
                .SetName("Object with custom QueryProp name"),
            new TestCaseData(new TestObjWithCustomFromQueryName { Prop = 1 }, "MyProp", 1, new QueryMemberNameSelector())
                .SetName("Object with custom FromQuery name"),
            new TestCaseData(new TestObjWithCustomJsonPropertyName { Prop = 1 }, "MyProp", 1, new BodyMemberNameSelector())
                .SetName("Object with custom JsonPropertyName"),

            new TestCaseData(new TestObjWithIntField { Field = 1 }, "Field", 1, null)
                .SetName("Top level primitive field"),
            new TestCaseData(new TestObjWithNestedField { Field = 1 }, "Field", 1, null)
                .SetName("Nested field"),
            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "Prop", 1, null)
                .SetName("Top level primitive property"),
            new TestCaseData(new TestObjWithNestedProp { Prop = 1 }, "Prop", 1, null)
                .SetName("Nested property"),

            new TestCaseData(new { Prop = 1 }, "Prop", 1, null)
                .SetName("Top level primitive property"),
            new TestCaseData(new { Prop = "test" }, "Prop", "test", null)
                .SetName("Top level string property"),
            new TestCaseData(new { Prop = new[] { 1, 2 } }, "Prop", new[] { 1, 2 }, null)
                .SetName("Top level array property"),
            new TestCaseData(new { Prop = new { Prop = 1 } }, "Prop", new { Prop = 1 }, null)
                .SetName("Top level custom object property"),
            new TestCaseData(new { Prop = new { Prop = 1 } }, "Prop.Prop", 1, null)
                .SetName("Inner property"),
            new TestCaseData(new { Prop = new { Prop = new { Prop = 1 } } }, "Prop.Prop.Prop", 1, null)
                .SetName("Deep property")

        };

        public static IEnumerable InvalidTestCasesForGetMemberValue = new[]
        {
            new TestCaseData(new TestObjWithCustomQueryPropName { Prop = 1 }, "Prop", typeof(NClientException), new QueryMemberNameSelector())
                .SetName("Object with custom QueryProp name but used real prop name"),
            new TestCaseData(new TestObjWithCustomFromQueryName { Prop = 1 }, "Prop", typeof(NClientException), new QueryMemberNameSelector())
                .SetName("Object with custom FromQuery name but used real prop name"),
            new TestCaseData(new TestObjWithCustomJsonPropertyName { Prop = 1 }, "Prop", typeof(NClientException), new BodyMemberNameSelector())
                .SetName("Object with custom JsonPropertyName but used real prop name"),
            new TestCaseData(new TestObjWithMemberNameConflict { Prop = 1 }, "MyProp", typeof(NClientException), new QueryMemberNameSelector())
                .SetName("Object with member name conflict"),

            new TestCaseData(null, null, typeof(ArgumentNullException), null)
                .SetName("Null object and null path"),
            new TestCaseData(null, "", typeof(ArgumentNullException), null)
                .SetName("Null object and empty path"),
            new TestCaseData(new {}, null, typeof(ArgumentNullException), null)
                .SetName("Empty object and null path"),
            new TestCaseData(null, "Prop", typeof(ArgumentNullException), null)
                .SetName("Null object and null path"),
            new TestCaseData(new {}, "", typeof(ArgumentException), null)
                .SetName("Empty object and empty path"),
            new TestCaseData(new {}, "Prop", typeof(NClientException), null)
                .SetName("Empty object and path"),
            new TestCaseData(new { Prop = 1 }, "", typeof(ArgumentException), null)
                .SetName("Object and empty path"),
            new TestCaseData(new { Prop = 1 }, "NonexistentProp", typeof(NClientException), null)
                .SetName("Object and nonexistent path"),
            new TestCaseData(new { Prop = new { Prop = 1 } }, "Prop.NonexistentProp", typeof(NClientException), null)
                .SetName("Object and inner nonexistent path"),
            new TestCaseData(new { Prop = 1 }, "prop", typeof(NClientException), null)
                .SetName("Top level property with invalid case"),
            new TestCaseData(new { Prop = new { Prop = 1 } }, "prop.prop", typeof(NClientException), null)
                .SetName("Inner property with invalid case"),
        };

        public static IEnumerable ValidTestCasesForSetMemberValue = new[]
        {
            new TestCaseData(new TestObjWithCustomQueryPropName { Prop = 1 }, "2", "MyProp", new TestObjWithCustomQueryPropName { Prop = 2 }, new QueryMemberNameSelector())
                .SetName("Object with custom QueryProp name"),
            new TestCaseData(new TestObjWithCustomFromQueryName { Prop = 1 }, "2", "MyProp", new TestObjWithCustomFromQueryName { Prop = 2 }, new QueryMemberNameSelector())
                .SetName("Object with custom FromQuery name"),
            new TestCaseData(new TestObjWithCustomJsonPropertyName { Prop = 1 }, "2", "MyProp", new TestObjWithCustomJsonPropertyName { Prop = 2 }, new BodyMemberNameSelector())
                .SetName("Object with custom JsonPropertyName"),

            new TestCaseData(new TestObjWithIntField { Field = 1 }, "2", "Field", new TestObjWithIntField { Field = 2 }, null)
                .SetName("Top level primitive field"),
            new TestCaseData(new TestObjWithNestedField { Field = 1 },"2", "Field", new TestObjWithNestedField { Field = 2 }, null)
                .SetName("Nested field"),
            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "2", "Prop", new TestObjWithIntProp { Prop = 2 }, null)
                .SetName("Top level primitive property"),
            new TestCaseData(new TestObjWithNestedProp { Prop = 1 }, "2", "Prop", new TestObjWithNestedProp { Prop = 2 }, null)
                .SetName("Nested property"),

            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "2", "Prop",
                    new TestObjWithIntProp { Prop = 2 }, null)
                .SetName("Top level primitive property"),
            new TestCaseData(new TestObjWithStringProp { Prop = "test" }, "test2", "Prop",
                    new TestObjWithStringProp { Prop = "test2" }, null)
                .SetName("Top level string property"),
            new TestCaseData(new TestObjWithObjectProp { Prop = new() { Prop = 1 } }, "2", "Prop.Prop",
                    new TestObjWithObjectProp { Prop = new() { Prop = 2 } }, null)
                .SetName("Inner property"),
            new TestCaseData(new TestObjWithDeepObjectProp { Prop = new() { Prop = new() { Prop = 1 } } }, "2", "Prop.Prop.Prop",
                    new TestObjWithDeepObjectProp { Prop = new() { Prop = new() { Prop = 2 } } }, null)
                .SetName("Deep property")
        };

        public static IEnumerable InvalidTestCasesForSetMemberValue = new[]
        {
            new TestCaseData(new TestObjWithCustomQueryPropName { Prop = 1 }, "2", "Prop", typeof(NClientException), new QueryMemberNameSelector())
                .SetName("Object with custom QueryProp name but used real prop name"),
            new TestCaseData(new TestObjWithCustomFromQueryName { Prop = 1 }, "2", "Prop", typeof(NClientException), new QueryMemberNameSelector())
                .SetName("Object with custom FromQuery name but used real prop name"),
            new TestCaseData(new TestObjWithCustomJsonPropertyName { Prop = 1 }, "2", "Prop", typeof(NClientException), new BodyMemberNameSelector())
                .SetName("Object with custom JsonPropertyName but used real prop name"),
            new TestCaseData(new TestObjWithMemberNameConflict { Prop = 1 }, "2", "MyProp", typeof(NClientException), new QueryMemberNameSelector())
                .SetName("Object with member name conflict"),

            new TestCaseData(null, "2", null, typeof(ArgumentNullException), null)
                .SetName("Null object and null path"),
            new TestCaseData(null, "2", "", typeof(ArgumentNullException), null)
                .SetName("Null object and empty path"),
            new TestCaseData(new {}, "2", null, typeof(ArgumentNullException), null)
                .SetName("Empty object and null path"),
            new TestCaseData(null, "2", "Prop", typeof(ArgumentNullException), null)
                .SetName("Null object and null path"),
            new TestCaseData(new {}, "2", "", typeof(ArgumentException), null)
                .SetName("Empty object and empty path"),
            new TestCaseData(new {}, "2", "Prop", typeof(NClientException), null)
                .SetName("Empty object and path"),
            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "2", "", typeof(ArgumentException), null)
                .SetName("Object and empty path"),
            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "2", "NonexistentProp", typeof(NClientException), null)
                .SetName("Object and nonexistent path"),
            new TestCaseData(new TestObjWithObjectProp { Prop = new TestObjWithIntProp { Prop = 1 } }, "2", "Prop.NonexistentProp", typeof(NClientException), null)
                .SetName("Object and inner nonexistent path"),
            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "2", "prop", typeof(NClientException), null)
                .SetName("Top level property with invalid case"),
            new TestCaseData(new TestObjWithObjectProp { Prop = new () { Prop = 1 } }, "2", "prop.prop", typeof(NClientException), null)
                .SetName("Inner property with invalid case"),
        };

        [TestCaseSource(nameof(ValidTestCasesForGetMemberValue))]
        public void GetMemberValue_ValidTestCases(object obj, string memberPath, object expectedResult, IMemberNameSelector? memberNameSelector = null)
        {
            memberNameSelector ??= new DefaultMemberNameSelector();

            var actualResult = ObjectMemberManager.GetMemberValue(obj, memberPath, memberNameSelector);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [TestCaseSource(nameof(InvalidTestCasesForGetMemberValue))]
        public void GetMemberValue_InvalidTestCases(object obj, string memberPath, Type exceptionType, IMemberNameSelector? memberNameSelector = null)
        {
            memberNameSelector ??= new DefaultMemberNameSelector();

            Func<object?> func = () => ObjectMemberManager.GetMemberValue(obj, memberPath, memberNameSelector);

            func.Should().Throw<Exception>().Where(x => x.GetType() == exceptionType);
        }

        [TestCaseSource(nameof(ValidTestCasesForSetMemberValue))]
        public void SetMemberValue_ValidTestCases(object obj, string value, string memberPath, object expectedResult, IMemberNameSelector? memberNameSelector = null)
        {
            memberNameSelector ??= new DefaultMemberNameSelector();

            ObjectMemberManager.SetMemberValue(obj, value, memberPath, memberNameSelector);

            obj.Should().BeEquivalentTo(expectedResult);
        }

        [TestCaseSource(nameof(InvalidTestCasesForSetMemberValue))]
        public void SetMemberValue_InvalidTestCases(object obj, string value, string memberPath, Type exceptionType, IMemberNameSelector? memberNameSelector = null)
        {
            memberNameSelector ??= new DefaultMemberNameSelector();

            Action func = () => ObjectMemberManager.SetMemberValue(obj, value, memberPath, memberNameSelector);

            func.Should().Throw<Exception>().Where(x => x.GetType() == exceptionType);
        }
    }
}
