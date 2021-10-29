using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Core.Mappers;
using NClient.Exceptions;
using NClient.Standalone.Exceptions.Factories;
using NUnit.Framework;

namespace NClient.Providers.Api.Rest.Tests.MethodBuilders.Providers
{
    [Parallelizable]
    [SuppressMessage("ReSharper", "BadDeclarationBracesLineBreaks")]
    [SuppressMessage("ReSharper", "BadEmptyBracesLineBreaks")]
    public class MethodAttributeProviderTest
    {
        private interface IGetMethod { [GetMethod] void Method(); }
        private interface IPostMethod { [PostMethod] void Method(); }
        private interface IPutMethod { [PutMethod] void Method(); }
        private interface IDeleteMethod { [DeleteMethod] void Method(); }

        private interface IMethodWithTemplate { [GetMethod("template")] void Method(); }
        private interface IMethodWithName { [GetMethod(Name = "name")] void Method(); }
        private interface IMethodWithOrder { [GetMethod(Order = 1)] void Method(); }
        private interface IMethodWithFull { [GetMethod("template", Name = "name", Order = 1)] void Method(); }

        private interface IOtherAndMethod { [Other, GetMethod] void Method(); }
        private interface INotSupported { [NotSupported] void Method(); }
        private interface IMNotSupportedWithTemplate { [NotSupported] void Method(); }

        public static IEnumerable ValidTestCases = new[]
        {
            new TestCaseData(GetMethodInfo<IGetMethod>(), new GetMethodAttribute())
                .SetName("With get attribute"),
            new TestCaseData(GetMethodInfo<IPostMethod>(), new PostMethodAttribute())
                .SetName("With post attribute"),
            new TestCaseData(GetMethodInfo<IPutMethod>(), new PutMethodAttribute())
                .SetName("With put attribute"),
            new TestCaseData(GetMethodInfo<IDeleteMethod>(), new DeleteMethodAttribute())
                .SetName("With delete attribute"),

            new TestCaseData(GetMethodInfo<IMethodWithTemplate>(), new GetMethodAttribute("template"))
                .SetName("With template"),
            new TestCaseData(GetMethodInfo<IMethodWithName>(), new GetMethodAttribute { Name = "name" })
                .SetName("With name"),
            new TestCaseData(GetMethodInfo<IMethodWithOrder>(), new GetMethodAttribute { Order = 1 })
                .SetName("With order"),
            new TestCaseData(GetMethodInfo<IMethodWithFull>(), new GetMethodAttribute("template") { Name = "name", Order = 1 })
                .SetName("With template, name and order"),

            new TestCaseData(GetMethodInfo<IOtherAndMethod>(), new GetMethodAttribute())
                .SetName("With other and method attribute"),
            new TestCaseData(GetMethodInfo<INotSupported>(), new NotSupportedAttribute())
                .SetName("With not supported attribute"),
            new TestCaseData(GetMethodInfo<IMNotSupportedWithTemplate>(), new NotSupportedAttribute())
                .SetName("With not supported method attribute with template")
        };

        private interface IWithout { void Method(); }
        private interface IOther { [Other] void Method(); }
        private interface INotSupportedAndMethod { [NotSupported, GetMethod] void Method(); }

        public static IEnumerable InvalidTestCases = new[]
        {
            new TestCaseData(GetMethodInfo<IWithout>())
                .SetName("Without attribute"),
            new TestCaseData(GetMethodInfo<IOther>())
                .SetName("With other attribute"),
            new TestCaseData(GetMethodInfo<INotSupportedAndMethod>())
                .SetName("With not supported and method attribute")
        };

        private OperationAttributeProvider _operationAttributeProvider = null!;

        [SetUp]
        public void SetUp()
        {
            var attributeMapper = new AttributeMapper();
            var clientRequestExceptionFactory = new ClientValidationExceptionFactory();
            _operationAttributeProvider = new OperationAttributeProvider(attributeMapper, clientRequestExceptionFactory);
        }

        [TestCaseSource(nameof(ValidTestCases))]
        public void Get_ValidTestCase_MethodAttribute(MethodInfo methodInfo, OperationAttribute expectedAttribute)
        {
            var actualAttribute = _operationAttributeProvider.Get(methodInfo, overridingMethods: Array.Empty<MethodInfo>());

            actualAttribute.Should().BeEquivalentTo(expectedAttribute);
        }

        [TestCaseSource(nameof(InvalidTestCases))]
        public void Get_InvalidTestCase_ThrowNClientException(MethodInfo methodInfo)
        {
            _operationAttributeProvider
                .Invoking(x => x.Get(methodInfo, overridingMethods: Array.Empty<MethodInfo>()))
                .Should()
                .Throw<NClientException>();
        }

        private static MethodInfo GetMethodInfo<T>()
        {
            return typeof(T).GetMethods().Single();
        }

        private class OtherAttribute : Attribute { }

        private class NotSupportedAttribute : OperationAttribute
        {
        }
    }
}