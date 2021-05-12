using System;
using System.Collections;
using System.Net.Http;
using FluentAssertions;
using NClient.Annotations.Methods;
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
            new TestCaseData(new PostMethodAttribute(), HttpMethod.Post),
            new TestCaseData(new PutMethodAttribute(), HttpMethod.Put),
            new TestCaseData(new DeleteMethodAttribute(), HttpMethod.Delete),
        };

        public static IEnumerable InvalidTestCases = new[]
        {
            new TestCaseData(null),
            new TestCaseData(new NotSupportedAttribute())
        };

        [TestCaseSource(nameof(ValidTestCases))]
        public void Get_MethodAttribute_HttpMethod(MethodAttribute methodAttribute, HttpMethod expectedHttpMethod)
        {
            var httpMethod = new HttpMethodProvider().Get(methodAttribute);

            httpMethod.Should().Be(expectedHttpMethod);
        }

        [TestCaseSource(nameof(InvalidTestCases))]
        public void Get_MethodAttribute_ThrowException(MethodAttribute methodAttribute)
        {
            new HttpMethodProvider()
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
