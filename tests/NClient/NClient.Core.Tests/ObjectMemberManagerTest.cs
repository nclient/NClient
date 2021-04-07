using System;
using System.Collections;
using FluentAssertions;
using NClient.Core.Exceptions;
using NClient.Core.Helpers;
using NUnit.Framework;

namespace NClient.Core.Tests
{
    public class ObjectMemberManagerTest
    {
        public class TestObjWithIntField { public int Field = 1; }
        public class TestObjWithNestedField : TestObjWithIntField { }
        public class TestObjWithIntProp { public int Prop { get; set; } = 1; }
        public class TestObjWithNestedProp : TestObjWithIntProp { }
        public class TestObjWithStringProp { public string Prop { get; set; } = "test"; }
        public class TestObjWithObjectProp { public TestObjWithIntProp Prop { get; set; } = new() { Prop = 1 }; }
        public class TestObjWithDeepObjectProp { public TestObjWithObjectProp Prop { get; set; } = new() { Prop = new() { Prop = 1 } }; }

        public static IEnumerable ValidTestCasesForGetMemberValue = new[]
        {
            new TestCaseData(new TestObjWithIntField { Field = 1 }, "Field", 1)
                .SetName("Top level primitive field"),
            new TestCaseData(new TestObjWithNestedField { Field = 1 }, "Field", 1)
                .SetName("Nested field"),
            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "Prop", 1)
                .SetName("Top level primitive property"),
            new TestCaseData(new TestObjWithNestedProp { Prop = 1 }, "Prop", 1)
                .SetName("Nested property"),

            new TestCaseData(new { Prop = 1 }, "Prop", 1)
                .SetName("Top level primitive property"),
            new TestCaseData(new { Prop = "test" }, "Prop", "test")
                .SetName("Top level string property"),
            new TestCaseData(new { Prop = new[] { 1, 2 } }, "Prop", new[] { 1, 2 })
                .SetName("Top level array property"),
            new TestCaseData(new { Prop = new { Prop = 1 } }, "Prop", new { Prop = 1 })
                .SetName("Top level custom object property"),
            new TestCaseData(new { Prop = new { Prop = 1 } }, "Prop.Prop", 1)
                .SetName("Inner property"),
            new TestCaseData(new { Prop = new { Prop = new { Prop = 1 } } }, "Prop.Prop.Prop", 1)
                .SetName("Deep property")

        };

        public static IEnumerable InvalidTestCasesForGetMemberValue = new[]
        {
            new TestCaseData(null, null, typeof(ArgumentNullException))
                .SetName("Null object and null path"),
            new TestCaseData(null, "", typeof(ArgumentNullException))
                .SetName("Null object and empty path"),
            new TestCaseData(new {}, null, typeof(ArgumentNullException))
                .SetName("Empty object and null path"),
            new TestCaseData(null, "Prop", typeof(ArgumentNullException))
                .SetName("Null object and null path"),
            new TestCaseData(new {}, "", typeof(ArgumentException))
                .SetName("Empty object and empty path"),
            new TestCaseData(new {}, "Prop", typeof(NClientException))
                .SetName("Empty object and path"),
            new TestCaseData(new { Prop = 1 }, "", typeof(ArgumentException))
                .SetName("Object and empty path"),
            new TestCaseData(new { Prop = 1 }, "NonexistentProp", typeof(NClientException))
                .SetName("Object and nonexistent path"),
            new TestCaseData(new { Prop = new { Prop = 1 } }, "Prop.NonexistentProp", typeof(NClientException))
                .SetName("Object and inner nonexistent path"),
            new TestCaseData(new { Prop = 1 }, "prop", typeof(NClientException))
                .SetName("Top level property with invalid case"),
            new TestCaseData(new { Prop = new { Prop = 1 } }, "prop.prop", typeof(NClientException))
                .SetName("Inner property with invalid case"),
        };

        public static IEnumerable ValidTestCasesForSetMemberValue = new[]
        {
            new TestCaseData(new TestObjWithIntField { Field = 1 }, "2", "Field", new TestObjWithIntField { Field = 2 })
                .SetName("Top level primitive field"),
            new TestCaseData(new TestObjWithNestedField { Field = 1 },"2", "Field", new TestObjWithNestedField { Field = 2 })
                .SetName("Nested field"),
            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "2", "Prop", new TestObjWithIntProp { Prop = 2 })
                .SetName("Top level primitive property"),
            new TestCaseData(new TestObjWithNestedProp { Prop = 1 }, "2", "Prop", new TestObjWithNestedProp { Prop = 2 })
                .SetName("Nested property"),

            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "2", "Prop",
                    new TestObjWithIntProp { Prop = 2 })
                .SetName("Top level primitive property"),
            new TestCaseData(new TestObjWithStringProp { Prop = "test" }, "test2", "Prop",
                    new TestObjWithStringProp { Prop = "test2" })
                .SetName("Top level string property"),
            new TestCaseData(new TestObjWithObjectProp { Prop = new() { Prop = 1 } }, "2", "Prop.Prop",
                    new TestObjWithObjectProp { Prop = new() { Prop = 2 } })
                .SetName("Inner property"),
            new TestCaseData(new TestObjWithDeepObjectProp { Prop = new() { Prop = new() { Prop = 1 } } }, "2", "Prop.Prop.Prop",
                    new TestObjWithDeepObjectProp { Prop = new() { Prop = new() { Prop = 2 } } })
                .SetName("Deep property")
        };

        public static IEnumerable InvalidTestCasesForSetMemberValue = new[]
        {
            new TestCaseData(null, "2", null, typeof(ArgumentNullException))
                .SetName("Null object and null path"),
            new TestCaseData(null, "2", "", typeof(ArgumentNullException))
                .SetName("Null object and empty path"),
            new TestCaseData(new {}, "2", null, typeof(ArgumentNullException))
                .SetName("Empty object and null path"),
            new TestCaseData(null, "2", "Prop", typeof(ArgumentNullException))
                .SetName("Null object and null path"),
            new TestCaseData(new {}, "2", "", typeof(ArgumentException))
                .SetName("Empty object and empty path"),
            new TestCaseData(new {}, "2", "Prop", typeof(NClientException))
                .SetName("Empty object and path"),
            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "2", "", typeof(ArgumentException))
                .SetName("Object and empty path"),
            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "2", "NonexistentProp", typeof(NClientException))
                .SetName("Object and nonexistent path"),
            new TestCaseData(new TestObjWithObjectProp { Prop = new TestObjWithIntProp { Prop = 1 } }, "2", "Prop.NonexistentProp", typeof(NClientException))
                .SetName("Object and inner nonexistent path"),
            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "2", "prop", typeof(NClientException))
                .SetName("Top level property with invalid case"),
            new TestCaseData(new TestObjWithObjectProp { Prop = new () { Prop = 1 } }, "2", "prop.prop", typeof(NClientException))
                .SetName("Inner property with invalid case"),
        };

        [TestCaseSource(nameof(ValidTestCasesForGetMemberValue))]
        public void GetMemberValue_ValidTestCases(object obj, string memberPath, object expectedResult)
        {
            var actualResult = ObjectMemberManager.GetMemberValue(obj, memberPath);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [TestCaseSource(nameof(InvalidTestCasesForGetMemberValue))]
        public void GetMemberValue_InvalidTestCases(object obj, string memberPath, Type exceptionType)
        {
            Func<object?> func = () => ObjectMemberManager.GetMemberValue(obj, memberPath);

            func.Should().Throw<Exception>().Where(x => x.GetType() == exceptionType);
        }

        [TestCaseSource(nameof(ValidTestCasesForSetMemberValue))]
        public void SetMemberValue_ValidTestCases(object obj, string value, string memberPath, object expectedResult)
        {
            ObjectMemberManager.SetMemberValue(obj, value, memberPath);

            obj.Should().BeEquivalentTo(expectedResult);
        }

        [TestCaseSource(nameof(InvalidTestCasesForSetMemberValue))]
        public void SetMemberValue_InvalidTestCases(object obj, string value, string memberPath, Type exceptionType)
        {
            Action func = () => ObjectMemberManager.SetMemberValue(obj, value, memberPath); ;

            func.Should().Throw<Exception>().Where(x => x.GetType() == exceptionType);
        }
    }
}
