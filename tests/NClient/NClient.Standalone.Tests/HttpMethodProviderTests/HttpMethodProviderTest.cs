using System;
using System.Collections;
using FluentAssertions;
using NClient.Annotations.Methods;
using NClient.Common.Helpers;
using NClient.Providers.Transport;
using NClient.Standalone.ClientProxy.Interceptors.RequestBuilders;
using NClient.Standalone.Exceptions.Factories;
using NUnit.Framework;

namespace NClient.Standalone.Tests.HttpMethodProviderTests
{
    [Parallelizable]
    public class HttpMethodProviderTest
    {
        private static readonly ClientValidationExceptionFactory ClientValidationExceptionFactory = new();

        public static IEnumerable ValidTestCases = new[]
        {
            new TestCaseData(new GetMethodAttribute(), RequestType.Read),
            new TestCaseData(new HeadMethodAttribute(), RequestType.Head),
            new TestCaseData(new PostMethodAttribute(), RequestType.Create),
            new TestCaseData(new PutMethodAttribute(), RequestType.Update),
            new TestCaseData(new DeleteMethodAttribute(), RequestType.Delete),
            new TestCaseData(new OptionsMethodAttribute(), RequestType.Options),
            new TestCaseData(new PatchMethodAttribute(), RequestType.Patch)
        };

        public static IEnumerable InvalidTestCases = new[]
        {
            new TestCaseData(null,
                EnsureExceptionFactory.CreateArgumentNullException("methodAttribute")),
            new TestCaseData(new NotSupportedAttribute(),
                ClientValidationExceptionFactory.MethodAttributeNotSupported(nameof(NotSupportedAttribute)))
        };

        private TransportMethodProvider _transportMethodProvider = null!;

        [SetUp]
        public void SetUp()
        {
            var clientRequestExceptionFactory = new ClientValidationExceptionFactory();
            _transportMethodProvider = new TransportMethodProvider(clientRequestExceptionFactory);
        }

        [TestCaseSource(nameof(ValidTestCases))]
        public void Get_MethodAttribute_HttpMethod(MethodAttribute methodAttribute, RequestType expectedHttpMethod)
        {
            var httpMethod = _transportMethodProvider.Get(methodAttribute);

            httpMethod.Should().Be(expectedHttpMethod);
        }

        [TestCaseSource(nameof(InvalidTestCases))]
        public void Get_MethodAttribute_ThrowException(MethodAttribute methodAttribute, Exception exception)
        {
            _transportMethodProvider
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
