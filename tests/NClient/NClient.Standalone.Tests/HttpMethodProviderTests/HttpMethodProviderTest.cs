using System;
using System.Collections;
using System.Net.Http;
using FluentAssertions;
using NClient.Annotations.Methods;
using NClient.Core.Exceptions.Factories;
using NClient.Core.RequestBuilders;
using NUnit.Framework;

namespace NClient.Standalone.Tests.HttpMethodProviderTests
{
    [Parallelizable]
    public class HttpMethodProviderTest
    {
        public static IEnumerable ValidTestCases = new[]
        {
            new TestCaseData(new GetMethodAttribute(), HttpMethod.Get),
            new TestCaseData(new HeadMethodAttribute(), HttpMethod.Head),
            new TestCaseData(new PostMethodAttribute(), HttpMethod.Post),
            new TestCaseData(new PutMethodAttribute(), HttpMethod.Put),
            new TestCaseData(new DeleteMethodAttribute(), HttpMethod.Delete),
            new TestCaseData(new OptionsMethodAttribute(), HttpMethod.Options),
            new TestCaseData(new PatchMethodAttribute(), HttpMethod.Patch),
        };

        public static IEnumerable InvalidTestCases = new[]
        {
            new TestCaseData(null),
            new TestCaseData(new NotSupportedAttribute())
        };

        private HttpMethodProvider _httpMethodProvider = null!;

        [SetUp]
        public void SetUp()
        {
            var clientRequestExceptionFactory = new ClientValidationExceptionFactory();
            _httpMethodProvider = new HttpMethodProvider(clientRequestExceptionFactory);
        }

        [TestCaseSource(nameof(ValidTestCases))]
        public void Get_MethodAttribute_HttpMethod(MethodAttribute methodAttribute, HttpMethod expectedHttpMethod)
        {
            var httpMethod = _httpMethodProvider.Get(methodAttribute);

            httpMethod.Should().Be(expectedHttpMethod);
        }

        [TestCaseSource(nameof(InvalidTestCases))]
        public void Get_MethodAttribute_ThrowException(MethodAttribute methodAttribute)
        {
            _httpMethodProvider
                .Invoking(x => x.Get(methodAttribute))
                .Should()
                .Throw<Exception>();
        }

        private class NotSupportedAttribute : MethodAttribute
        {
            public NotSupportedAttribute() : base(null)
            {
            }
        }
    }
}
