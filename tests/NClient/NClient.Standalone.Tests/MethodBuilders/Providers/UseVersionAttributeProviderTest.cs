using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NClient.Abstractions.Exceptions;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Versioning;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Interceptors.MethodBuilders.Providers;
using NClient.Core.Mappers;
using NUnit.Framework;

namespace NClient.Standalone.Tests.MethodBuilders.Providers
{
    [Parallelizable]
    public class UseVersionAttributeProviderTest
    {
        private interface IClientWithout { void Method(); }
        [UseVersion("1.0")] private interface IClientWithVersion { void Method(); }

        [Api] private interface IClientWithApi { void Method(); }
        [Api, UseVersion("1.0")] private interface IClientWithVersionAndApi { void Method(); }
        [Other] private interface IClientWithOther { void Method(); }
        [Other, UseVersion("1.0")] private interface IClientWithVersionAndOther { void Method(); }

        public interface IClientInheritance : IControllerInheritance { void Method(); }
        [UseVersion("1.0")] public interface IControllerInheritance { }

        public interface IClientDeepInheritance : IClientDeepInheritanceBase { void Method(); }
        public interface IClientDeepInheritanceBase : IControllerDeepInheritance { }
        [UseVersion("1.0")] public interface IControllerDeepInheritance { }


        private interface IClientWithMethodVersion {[UseVersion("1.0")] void Method(); }

        private interface IClientWithMethod {[PostMethod] void Method(); }
        private interface IClientWithMethodVersionAndMethod {[PostMethod, UseVersion("1.0")] void Method(); }
        private interface IClientWithMethodOther {[Other] void Method(); }


        private interface IClientWithMethodVersionAndOther {[Other, UseVersion("1.0")] void Method(); }

        public interface IClientMethodOverride : IControllerMethodOverride {[UseVersion("2.0")] new void Method(); }
        public interface IControllerMethodOverride {[UseVersion("1.0")] void Method(); }

        [UseVersion("1.0")] private interface IClientWithInterfaceAndMethodVersion {[UseVersion("2.0")] void Method(); }

        public static IEnumerable ValidTestCases = new[]
        {
            new TestCaseData(typeof(IClientWithout), GetMethodInfo<IClientWithout>(), null)
                .SetName("Without version"),
            new TestCaseData(typeof(IClientWithVersion), GetMethodInfo<IClientWithVersion>(), new UseVersionAttribute("1.0"))
                .SetName("With version"),

            new TestCaseData(typeof(IClientWithApi), GetMethodInfo<IClientWithApi>(), null)
                .SetName("With api"),
            new TestCaseData(typeof(IClientWithVersionAndApi), GetMethodInfo<IClientWithVersionAndApi>(), new UseVersionAttribute("1.0"))
                .SetName("With version and api"),
            new TestCaseData(typeof(IClientWithOther), GetMethodInfo<IClientWithOther>(), null)
                .SetName("With other attribute"),
            new TestCaseData(typeof(IClientWithVersionAndOther), GetMethodInfo<IClientWithVersionAndOther>(), new UseVersionAttribute("1.0"))
                .SetName("With version and other attribute"),

            new TestCaseData(typeof(IClientInheritance), GetMethodInfo<IClientInheritance>(), new UseVersionAttribute("1.0"))
                .SetName("With inheritance"),
            new TestCaseData(typeof(IClientDeepInheritance), GetMethodInfo<IClientDeepInheritance>(), new UseVersionAttribute("1.0"))
                .SetName("With deep inheritance"),


            new TestCaseData(typeof(IClientWithMethodVersion), GetMethodInfo<IClientWithMethodVersion>(), new UseVersionAttribute("1.0"))
                .SetName("Without method version"),

            new TestCaseData(typeof(IClientWithMethod), GetMethodInfo<IClientWithMethod>(), null)
                .SetName("With method attribute"),
            new TestCaseData(typeof(IClientWithMethodVersionAndMethod), GetMethodInfo<IClientWithMethodVersionAndMethod>(), new UseVersionAttribute("1.0"))
                .SetName("With version and method attributes"),
            new TestCaseData(typeof(IClientWithMethodOther), GetMethodInfo<IClientWithMethodOther>(), null)
                .SetName("With other method attribute"),
            new TestCaseData(typeof(IClientWithMethodVersionAndOther), GetMethodInfo<IClientWithMethodVersionAndOther>(), new UseVersionAttribute("1.0"))
                .SetName("With version and other method attributes"),


            new TestCaseData(typeof(IClientWithInterfaceAndMethodVersion), GetMethodInfo<IClientWithInterfaceAndMethodVersion>(), new UseVersionAttribute("2.0"))
                .SetName("Without method version"),
            new TestCaseData(typeof(IClientMethodOverride), GetMethodInfo<IClientMethodOverride>(), new UseVersionAttribute("2.0"))
                .SetName("Without method version"),
        };

        [UseVersion("2.0")] public interface IClientOverride : IControllerOverride { void Method(); }
        [UseVersion("1.0")] public interface IControllerOverride { }

        public static IEnumerable InvalidTestCases = new[]
        {
            new TestCaseData(typeof(IClientOverride), GetMethodInfo<IClientOverride>())
                .SetName("With override version"),
        };

        private UseVersionAttributeProvider _useVersionAttributeProvider = null!;

        [SetUp]
        public void SetUp()
        {
            var attributeMapper = new AttributeMapper();
            var clientRequestExceptionFactory = new ClientValidationExceptionFactory();
            _useVersionAttributeProvider = new UseVersionAttributeProvider(attributeMapper, clientRequestExceptionFactory);
        }

        [TestCaseSource(nameof(ValidTestCases))]
        public void Find_ValidTestCase_VersionAttribute(Type clientType, MethodInfo methodInfo, UseVersionAttribute expectedAttribute)
        {
            var actualAttribute = _useVersionAttributeProvider.Find(clientType, methodInfo, overridingMethods: Array.Empty<MethodInfo>());

            actualAttribute.Should().BeEquivalentTo(expectedAttribute);
        }

        [TestCaseSource(nameof(InvalidTestCases))]
        public void Find_InvalidTestCase_ThrowNClientException(Type clientType, MethodInfo methodInfo)
        {
            _useVersionAttributeProvider
                .Invoking(x => x.Find(clientType, methodInfo, overridingMethods: Array.Empty<MethodInfo>()))
                .Should()
                .Throw<NClientException>();
        }

        private class OtherAttribute : Attribute { }

        private static MethodInfo GetMethodInfo<T>()
        {
            return typeof(T).GetMethods().Single();
        }
    }
}