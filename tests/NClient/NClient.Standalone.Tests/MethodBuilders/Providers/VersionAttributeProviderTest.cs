using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NClient.Abstractions.Exceptions;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Versioning;
using NClient.Core.Mappers;
using NClient.Core.MethodBuilders.Providers;
using NUnit.Framework;

namespace NClient.Standalone.Tests.MethodBuilders.Providers
{
    [Parallelizable]
    public class VersionAttributeProviderTest
    {
        private interface IClientWithout { void Method(); }
        [Version("1.0")] private interface IClientWithVersion { void Method(); }

        [Api] private interface IClientWithApi { void Method(); }
        [Api, Version("1.0")] private interface IClientWithVersionAndApi { void Method(); }
        [Other] private interface IClientWithOther { void Method(); }
        [Other, Version("1.0")] private interface IClientWithVersionAndOther { void Method(); }

        public interface IClientInheritance : IControllerInheritance { void Method(); }
        [Version("1.0")] public interface IControllerInheritance { }

        public interface IClientDeepInheritance : IClientDeepInheritanceBase { void Method(); }
        public interface IClientDeepInheritanceBase : IControllerDeepInheritance { }
        [Version("1.0")] public interface IControllerDeepInheritance { }
        
        
        private interface IClientWithMethodVersion { [Version("1.0")] void Method(); }

        private interface IClientWithMethod { [PostMethod] void Method(); }
        private interface IClientWithMethodVersionAndMethod { [PostMethod, Version("1.0")] void Method(); }
        private interface IClientWithMethodOther { [Other] void Method(); }
        
        
        private interface IClientWithMethodVersionAndOther { [Other, Version("1.0")] void Method(); }

        public interface IClientMethodOverride : IControllerMethodOverride { [Version("2.0")] new void Method(); }
        public interface IControllerMethodOverride { [Version("1.0")] void Method(); }
        
        [Version("1.0")] private interface IClientWithInterfaceAndMethodVersion { [Version("2.0")] void Method(); }

        public static IEnumerable ValidTestCases = new[]
        {
            new TestCaseData(typeof(IClientWithout), GetMethodInfo<IClientWithout>(), null)
                .SetName("Without version"),
            new TestCaseData(typeof(IClientWithVersion), GetMethodInfo<IClientWithVersion>(), new VersionAttribute("1.0"))
                .SetName("With version"),

            new TestCaseData(typeof(IClientWithApi), GetMethodInfo<IClientWithApi>(), null)
                .SetName("With api"),
            new TestCaseData(typeof(IClientWithVersionAndApi), GetMethodInfo<IClientWithVersionAndApi>(), new VersionAttribute("1.0"))
                .SetName("With version and api"),
            new TestCaseData(typeof(IClientWithOther), GetMethodInfo<IClientWithOther>(), null)
                .SetName("With other attribute"),
            new TestCaseData(typeof(IClientWithVersionAndOther), GetMethodInfo<IClientWithVersionAndOther>(), new VersionAttribute("1.0"))
                .SetName("With version and other attribute"),

            new TestCaseData(typeof(IClientInheritance), GetMethodInfo<IClientInheritance>(), new VersionAttribute("1.0"))
                .SetName("With inheritance"),
            new TestCaseData(typeof(IClientDeepInheritance), GetMethodInfo<IClientDeepInheritance>(), new VersionAttribute("1.0"))
                .SetName("With deep inheritance"),
            
            
            new TestCaseData(typeof(IClientWithMethodVersion), GetMethodInfo<IClientWithMethodVersion>(), new VersionAttribute("1.0"))
                .SetName("Without method version"),
            
            new TestCaseData(typeof(IClientWithMethod), GetMethodInfo<IClientWithMethod>(), null)
                .SetName("With method attribute"),
            new TestCaseData(typeof(IClientWithMethodVersionAndMethod), GetMethodInfo<IClientWithMethodVersionAndMethod>(), new VersionAttribute("1.0"))
                .SetName("With version and method attributes"),
            new TestCaseData(typeof(IClientWithMethodOther), GetMethodInfo<IClientWithMethodOther>(), null)
                .SetName("With other method attribute"),
            new TestCaseData(typeof(IClientWithMethodVersionAndOther), GetMethodInfo<IClientWithMethodVersionAndOther>(), new VersionAttribute("1.0"))
                .SetName("With version and other method attributes"),
            
            
            new TestCaseData(typeof(IClientWithInterfaceAndMethodVersion), GetMethodInfo<IClientWithInterfaceAndMethodVersion>(), new VersionAttribute("2.0"))
                .SetName("Without method version"),
            new TestCaseData(typeof(IClientMethodOverride), GetMethodInfo<IClientMethodOverride>(), new VersionAttribute("2.0"))
                .SetName("Without method version"),
        };
        
        [Version("2.0")] public interface IClientOverride : IControllerOverride { void Method(); }
        [Version("1.0")] public interface IControllerOverride { }

        public static IEnumerable InvalidTestCases = new[]
        {
            new TestCaseData(typeof(IClientOverride), GetMethodInfo<IClientOverride>())
                .SetName("With override version"),
        };

        [TestCaseSource(nameof(ValidTestCases))]
        public void Find_ValidTestCase_VersionAttribute(Type clientType, MethodInfo methodInfo, VersionAttribute expectedAttribute)
        {
            var attributeMapper = new AttributeMapper();
            var attributeProvider = new VersionAttributeProvider(attributeMapper);

            var actualAttribute = attributeProvider.Find(clientType, methodInfo);

            actualAttribute.Should().BeEquivalentTo(expectedAttribute);
        }
        
        [TestCaseSource(nameof(InvalidTestCases))]
        public void Find_InvalidTestCase_ThrowNClientException(Type clientType, MethodInfo methodInfo)
        {
            var attributeMapper = new AttributeMapper();
            var attributeProvider = new VersionAttributeProvider(attributeMapper);

            attributeProvider
                .Invoking(x => x.Find(clientType, methodInfo))
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