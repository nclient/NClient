using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NClient.Annotations;
using NClient.Core.Mappers;
using NClient.Exceptions;
using NClient.Standalone.ClientProxy.Interceptors.MethodBuilders.Providers;
using NClient.Standalone.Exceptions.Factories;
using NUnit.Framework;

namespace NClient.Standalone.Tests.MethodBuilders.Providers
{
    [Parallelizable]
    [SuppressMessage("ReSharper", "BadDeclarationBracesLineBreaks")]
    [SuppressMessage("ReSharper", "BadEmptyBracesLineBreaks")]
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
                .SetName("With deep inheritance")
        };

        [Path("template")] public interface IClientOverride : IControllerOverride { }
        [Path("templateBase")] public interface IControllerOverride { }

        public static IEnumerable InvalidTestCases = new[]
        {
            new TestCaseData(typeof(IClientOverride))
                .SetName("With override path")
        };

        private PathAttributeProvider _pathAttributeProvider = null!;

        [SetUp]
        public void SetUp()
        {
            var attributeMapper = new AttributeMapper();
            var clientRequestExceptionFactory = new ClientValidationExceptionFactory();
            _pathAttributeProvider = new PathAttributeProvider(attributeMapper, clientRequestExceptionFactory);
        }

        [TestCaseSource(nameof(ValidTestCases))]
        public void Find_ValidTestCase_PathAttribute(Type clientType, PathAttribute expectedAttribute)
        {
            var actualAttribute = _pathAttributeProvider.Find(clientType);

            actualAttribute.Should().BeEquivalentTo(expectedAttribute);
        }

        [TestCaseSource(nameof(InvalidTestCases))]
        public void Find_InvalidTestCase_ThrowNClientException(Type clientType)
        {
            _pathAttributeProvider
                .Invoking(x => x.Find(clientType))
                .Should()
                .Throw<NClientException>();
        }

        private class OtherAttribute : Attribute { }
    }
}
