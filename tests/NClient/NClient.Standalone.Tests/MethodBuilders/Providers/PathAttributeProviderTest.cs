using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NClient.Abstractions.Exceptions;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Core.Exceptions;
using NClient.Core.Mappers;
using NClient.Core.MethodBuilders.Providers;
using NUnit.Framework;

namespace NClient.Standalone.Tests.MethodBuilders.Providers
{
    [Parallelizable]
    public class PathAttributeProviderTest
    {
        private interface IClientWithout { }
        [Path("template")] private interface IClientWithPath { }

        [Api] private interface IClientWithApi { }
        [Api, Path("template")] private interface IClientWithPathAndApi { }
        [Other] private interface IClientWithOther { }
        [Other, Path("template")] private interface IClientWithPathAndOther { }

        public interface IClientInheritance : IControllerInheritance { }
        [Path("template")] public interface IControllerInheritance { }

        public interface IClientDeepInheritance : IClientDeepInheritanceBase { }
        public interface IClientDeepInheritanceBase : IControllerDeepInheritance { }
        [Path("template")] public interface IControllerDeepInheritance { }


        public static IEnumerable ValidTestCases = new[]
        {
            new TestCaseData(typeof(IClientWithout), null)
                .SetName("Without path"),
            new TestCaseData(typeof(IClientWithPath), new PathAttribute("template"))
                .SetName("With path"),

            new TestCaseData(typeof(IClientWithApi), null)
                .SetName("With api"),
            new TestCaseData(typeof(IClientWithPathAndApi), new PathAttribute("template"))
                .SetName("With path and api"),
            new TestCaseData(typeof(IClientWithOther), null)
                .SetName("With other attribute"),
            new TestCaseData(typeof(IClientWithPathAndOther), new PathAttribute("template"))
                .SetName("With path and other attribute"),

            new TestCaseData(typeof(IClientInheritance), new PathAttribute("template"))
                .SetName("With inheritance"),
            new TestCaseData(typeof(IClientDeepInheritance), new PathAttribute("template"))
                .SetName("With deep inheritance"),
        };

        [Path("template")] public interface IClientOverride : IControllerOverride { }
        [Path("templateBase")] public interface IControllerOverride { }

        public static IEnumerable InvalidTestCases = new[]
        {
            new TestCaseData(typeof(IClientOverride))
                .SetName("With deep inheritance"),
        };

        [TestCaseSource(nameof(ValidTestCases))]
        public void Find_ValidTestCase_PathAttribute(Type clientType, PathAttribute expectedAttribute)
        {
            var attributeMapper = new AttributeMapper();
            var attributeProvider = new PathAttributeProvider(attributeMapper);

            var actualAttribute = attributeProvider.Find(clientType);

            actualAttribute.Should().BeEquivalentTo(expectedAttribute);
        }

        [TestCaseSource(nameof(InvalidTestCases))]
        public void Find_InvalidTestCase_ThrowNClientException(Type clientType)
        {
            var attributeMapper = new AttributeMapper();
            var attributeProvider = new PathAttributeProvider(attributeMapper);

            attributeProvider
                .Invoking(x => x.Find(clientType))
                .Should()
                .Throw<NClientException>();
        }

        private class OtherAttribute : Attribute { }

        private class NotSupportedAttribute : PathAttribute
        {
            public NotSupportedAttribute(string template) : base(template)
            {
            }
        }
    }
}