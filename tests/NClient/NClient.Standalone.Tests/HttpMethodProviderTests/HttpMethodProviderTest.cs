using System;
using System.Collections;
using System.Net.Http;
using FluentAssertions;
using NClient.Annotations.Methods;
using NClient.Common.Helpers;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Interceptors.RequestBuilders;
using NUnit.Framework;

namespace NClient.Standalone.Tests.HttpMethodProviderTests
{
    [Parallelizable]
    public class HttpMethodProviderTest
    {
        private static readonly ClientValidationExceptionFactory ClientValidationExceptionFactory = new();

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
            new TestCaseData(null,
                EnsureExceptionFactory.CreateArgumentNullException("methodAttribute")),
            new TestCaseData(new NotSupportedAttribute(),
                ClientValidationExceptionFactory.MethodAttributeNotSupported(nameof(NotSupportedAttribute)))
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
        public void Get_MethodAttribute_ThrowException(MethodAttribute methodAttribute, Exception exception)
        {
            _httpMethodProvider
                .Invoking(x => x.Get(methodAttribute))
                .Should()
                .Throw<Exception>()
                .Where(x => x.GetType() == exception.GetType())
                .WithMessage(exception.Message);
        }

        private class NotSupportedAttribute : MethodAttribute
        {
            public NotSupportedAttribute() : base(null)
            {
            }
        }
    }
}
