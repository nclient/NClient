using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NClient.Annotations.Http;
using NClient.Common.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors;
using NClient.Providers.Api.Rest.Exceptions.Factories;
using NUnit.Framework;

namespace NClient.Core.Tests
{
    [SuppressMessage("ReSharper", "BadDeclarationBracesLineBreaks")]
    [SuppressMessage("ReSharper", "BadEmptyBracesLineBreaks")]
    [SuppressMessage("ReSharper", "MultipleTypeMembersOnOneLine")]
    internal class ObjectMemberManagerTest
    {
        private static readonly ObjectMemberManagerExceptionFactory ExceptionFactory = new();
        private ObjectMemberManager _objectMemberManager = null!;

        public class TestObjWithCustomQueryPropName { [QueryParam(Name = "MyProp")] public int Prop { get; set; } = 1; }
        public class TestObjWithCustomFromQueryName { [FromQuery(Name = "MyProp")] public int Prop { get; set; } = 1; }
        public class TestObjWithCustomJsonPropertyName { [JsonPropertyName("MyProp")] public int Prop { get; set; } = 1; }
        public class TestObjWithMemberNameConflict { [QueryParam(Name = "MyProp")] public int Prop { get; set; } = 1; public int MyProp { get; set; } = 2; }

        public class TestObjWithIntField { public int Field = 1; }
        public class TestObjWithNestedField : TestObjWithIntField { }
        public class TestObjWithIntProp { public int Prop { get; set; } = 1; }
        public class TestObjWithNestedProp : TestObjWithIntProp { }
        public class TestObjWithStringProp { public string Prop { get; set; } = "test"; }
        public class TestObjWithObjectProp { public TestObjWithIntProp Prop { get; set; } = new() { Prop = 1 }; }
        public class TestObjWithDeepObjectProp { public TestObjWithObjectProp Prop { get; set; } = new() { Prop = new TestObjWithIntProp { Prop = 1 } }; }

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
            new TestCaseData(new TestObjWithCustomQueryPropName { Prop = 1 }, "Prop",
                    ExceptionFactory.MemberNotFound("Prop", nameof(TestObjWithCustomQueryPropName)), new QueryMemberNameSelector())
                .SetName("Object with custom QueryProp name but used real prop name"),
            new TestCaseData(new TestObjWithCustomFromQueryName { Prop = 1 }, "Prop",
                    ExceptionFactory.MemberNotFound("Prop", nameof(TestObjWithCustomFromQueryName)), new QueryMemberNameSelector())
                .SetName("Object with custom FromQuery name but used real prop name"),
            new TestCaseData(new TestObjWithCustomJsonPropertyName { Prop = 1 }, "Prop",
                    ExceptionFactory.MemberNotFound("Prop", nameof(TestObjWithCustomJsonPropertyName)), new BodyMemberNameSelector())
                .SetName("Object with custom JsonPropertyName but used real prop name"),
            new TestCaseData(new TestObjWithMemberNameConflict { Prop = 1 }, "MyProp",
                    ExceptionFactory.MemberNameConflict("MyProp", nameof(TestObjWithMemberNameConflict)), new QueryMemberNameSelector())
                .SetName("Object with member name conflict"),

            new TestCaseData(null, null,
                    EnsureExceptionFactory.CreateArgumentNullException("obj"), null)
                .SetName("Null object and null path"),
            new TestCaseData(null, "",
                    EnsureExceptionFactory.CreateArgumentNullException("obj"), null)
                .SetName("Null object and empty path"),
            new TestCaseData(new { }, null,
                    EnsureExceptionFactory.CreateArgumentNullException("memberPath"), null)
                .SetName("Empty object and null path"),
            new TestCaseData(null, "Prop",
                    EnsureExceptionFactory.CreateArgumentNullException("obj"), null)
                .SetName("Null object and null path"),
            new TestCaseData(new { }, "",
                    EnsureExceptionFactory.CreateEmptyArgumentException("memberPath"), null)
                .SetName("Empty object and empty path"),
            new TestCaseData(new { }, "Prop",
                    ExceptionFactory.MemberNotFound("Prop", "<>f__AnonymousType1"), null)
                .SetName("Empty object and path"),
            new TestCaseData(new { Prop = 1 }, "",
                    EnsureExceptionFactory.CreateEmptyArgumentException("memberPath"), null)
                .SetName("Object and empty path"),
            new TestCaseData(new { Prop = 1 }, "NonexistentProp",
                    ExceptionFactory.MemberNotFound("NonexistentProp", "<>f__AnonymousType0`1"), null)
                .SetName("Object and nonexistent path"),
            new TestCaseData(new { Prop = new { Prop = 1 } }, "Prop.NonexistentProp",
                    ExceptionFactory.MemberNotFound("NonexistentProp", "<>f__AnonymousType0`1"), null)
                .SetName("Object and inner nonexistent path"),
            new TestCaseData(new { Prop = 1 }, "prop",
                    ExceptionFactory.MemberNotFound("prop", "<>f__AnonymousType0`1"), null)
                .SetName("Top level property with invalid case"),
            new TestCaseData(new { Prop = new { Prop = 1 } }, "prop.prop",
                    ExceptionFactory.MemberNotFound("Prop", "<>f__AnonymousType0`1"), null)
                .SetName("Inner property with invalid case")
        };

        public static IEnumerable ValidTestCasesForSetMemberValue = new[]
        {
            new TestCaseData(new TestObjWithCustomQueryPropName { Prop = 1 }, "2", "MyProp",
                    new TestObjWithCustomQueryPropName { Prop = 2 }, new QueryMemberNameSelector())
                .SetName("Object with custom QueryProp name"),
            new TestCaseData(new TestObjWithCustomFromQueryName { Prop = 1 }, "2", "MyProp",
                    new TestObjWithCustomFromQueryName { Prop = 2 }, new QueryMemberNameSelector())
                .SetName("Object with custom FromQuery name"),
            new TestCaseData(new TestObjWithCustomJsonPropertyName { Prop = 1 }, "2", "MyProp",
                    new TestObjWithCustomJsonPropertyName { Prop = 2 }, new BodyMemberNameSelector())
                .SetName("Object with custom JsonPropertyName"),

            new TestCaseData(new TestObjWithIntField { Field = 1 }, "2", "Field",
                    new TestObjWithIntField { Field = 2 }, null)
                .SetName("Top level primitive field"),
            new TestCaseData(new TestObjWithNestedField { Field = 1 }, "2", "Field",
                    new TestObjWithNestedField { Field = 2 }, null)
                .SetName("Nested field"),
            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "2", "Prop",
                    new TestObjWithIntProp { Prop = 2 }, null)
                .SetName("Top level primitive property"),
            new TestCaseData(new TestObjWithNestedProp { Prop = 1 }, "2", "Prop",
                    new TestObjWithNestedProp { Prop = 2 }, null)
                .SetName("Nested property"),

            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "2", "Prop",
                    new TestObjWithIntProp { Prop = 2 }, null)
                .SetName("Top level primitive property"),
            new TestCaseData(new TestObjWithStringProp { Prop = "test" }, "test2", "Prop",
                    new TestObjWithStringProp { Prop = "test2" }, null)
                .SetName("Top level string property"),
            new TestCaseData(new TestObjWithObjectProp { Prop = new TestObjWithIntProp { Prop = 1 } }, "2", "Prop.Prop",
                    new TestObjWithObjectProp { Prop = new TestObjWithIntProp { Prop = 2 } }, null)
                .SetName("Inner property"),
            new TestCaseData(new TestObjWithDeepObjectProp { Prop = new TestObjWithObjectProp { Prop = new TestObjWithIntProp { Prop = 1 } } }, "2", "Prop.Prop.Prop",
                    new TestObjWithDeepObjectProp { Prop = new TestObjWithObjectProp { Prop = new TestObjWithIntProp { Prop = 2 } } }, null)
                .SetName("Deep property")
        };

        public static IEnumerable InvalidTestCasesForSetMemberValue = new[]
        {
            new TestCaseData(new TestObjWithCustomQueryPropName { Prop = 1 }, "2", "Prop",
                    ExceptionFactory.MemberNotFound("Prop", nameof(TestObjWithCustomQueryPropName)), new QueryMemberNameSelector())
                .SetName("Object with custom QueryProp name but used real prop name"),
            new TestCaseData(new TestObjWithCustomFromQueryName { Prop = 1 }, "2", "Prop",
                    ExceptionFactory.MemberNotFound("Prop", nameof(TestObjWithCustomFromQueryName)), new QueryMemberNameSelector())
                .SetName("Object with custom FromQuery name but used real prop name"),
            new TestCaseData(new TestObjWithCustomJsonPropertyName { Prop = 1 }, "2", "Prop",
                    ExceptionFactory.MemberNotFound("Prop", nameof(TestObjWithCustomJsonPropertyName)), new BodyMemberNameSelector())
                .SetName("Object with custom JsonPropertyName but used real prop name"),
            new TestCaseData(new TestObjWithMemberNameConflict { Prop = 1 }, "2", "MyProp",
                    ExceptionFactory.MemberNameConflict("MyProp", nameof(TestObjWithMemberNameConflict)), new QueryMemberNameSelector())
                .SetName("Object with member name conflict"),

            new TestCaseData(null, "2", null,
                    EnsureExceptionFactory.CreateArgumentNullException("obj"), null)
                .SetName("Null object and null path"),
            new TestCaseData(null, "2", "",
                    EnsureExceptionFactory.CreateArgumentNullException("obj"), null)
                .SetName("Null object and empty path"),
            new TestCaseData(new { }, "2", null,
                    EnsureExceptionFactory.CreateArgumentNullException("memberPath"), null)
                .SetName("Empty object and null path"),
            new TestCaseData(null, "2", "Prop",
                    EnsureExceptionFactory.CreateArgumentNullException("obj"), null)
                .SetName("Null object and null path"),
            new TestCaseData(new { }, "2", "",
                    EnsureExceptionFactory.CreateEmptyArgumentException("memberPath"), null)
                .SetName("Empty object and empty path"),
            new TestCaseData(new { }, "2", "Prop",
                    ExceptionFactory.MemberNotFound("Prop", "<>f__AnonymousType1"), null)
                .SetName("Empty object and path"),
            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "2", "",
                    EnsureExceptionFactory.CreateEmptyArgumentException("memberPath"), null)
                .SetName("Object and empty path"),
            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "2", "NonexistentProp",
                    ExceptionFactory.MemberNotFound("NonexistentProp", nameof(TestObjWithIntProp)), null)
                .SetName("Object and nonexistent path"),
            new TestCaseData(new TestObjWithObjectProp { Prop = new TestObjWithIntProp { Prop = 1 } }, "2", "Prop.NonexistentProp",
                    ExceptionFactory.MemberNotFound("NonexistentProp", nameof(TestObjWithIntProp)), null)
                .SetName("Object and inner nonexistent path"),
            new TestCaseData(new TestObjWithIntProp { Prop = 1 }, "2", "prop",
                    ExceptionFactory.MemberNotFound("prop", nameof(TestObjWithIntProp)), null)
                .SetName("Top level property with invalid case"),
            new TestCaseData(new TestObjWithObjectProp { Prop = new TestObjWithIntProp { Prop = 1 } }, "2", "prop.prop",
                    ExceptionFactory.MemberNotFound("prop", nameof(TestObjWithObjectProp)), null)
                .SetName("Inner property with invalid case")
        };

        [SetUp]
        public void SetUp()
        {
            _objectMemberManager = new ObjectMemberManager(ExceptionFactory);
        }

        [TestCaseSource(nameof(ValidTestCasesForGetMemberValue))]
        public void GetMemberValue_ValidTestCases(object obj, string memberPath, object expectedResult, IMemberNameSelector? memberNameSelector = null)
        {
            memberNameSelector ??= new DefaultMemberNameSelector();

            var actualResult = _objectMemberManager.GetValue(obj, memberPath, memberNameSelector);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [TestCaseSource(nameof(InvalidTestCasesForGetMemberValue))]
        public void GetMemberValue_InvalidTestCases(object obj, string memberPath, Exception exception, IMemberNameSelector? memberNameSelector = null)
        {
            memberNameSelector ??= new DefaultMemberNameSelector();

            Func<object?> func = () => _objectMemberManager.GetValue(obj, memberPath, memberNameSelector);

            func.Should().Throw<Exception>()
                .Where(x => x.GetType() == exception.GetType())
                .WithMessage(exception.Message);
        }

        [TestCaseSource(nameof(ValidTestCasesForSetMemberValue))]
        public void SetMemberValue_ValidTestCases(object obj, string value, string memberPath, object expectedResult, IMemberNameSelector? memberNameSelector = null)
        {
            memberNameSelector ??= new DefaultMemberNameSelector();

            _objectMemberManager.SetValue(obj, value, memberPath, memberNameSelector);

            obj.Should().BeEquivalentTo(expectedResult);
        }

        [TestCaseSource(nameof(InvalidTestCasesForSetMemberValue))]
        public void SetMemberValue_InvalidTestCases(object obj, string value, string memberPath, Exception exception, IMemberNameSelector? memberNameSelector = null)
        {
            memberNameSelector ??= new DefaultMemberNameSelector();

            Action func = () => _objectMemberManager.SetValue(obj, value, memberPath, memberNameSelector);

            func.Should().Throw<Exception>()
                .Where(x => x.GetType() == exception.GetType())
                .WithMessage(exception.Message);
        }
    }
}
