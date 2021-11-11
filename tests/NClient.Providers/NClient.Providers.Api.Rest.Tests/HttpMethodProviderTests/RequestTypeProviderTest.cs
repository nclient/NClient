using System;
using System.Collections;
using FluentAssertions;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Providers.Api.Rest.Exceptions.Factories;
using NClient.Providers.Api.Rest.Providers;
using NClient.Providers.Transport;
using NUnit.Framework;

namespace NClient.Providers.Api.Rest.Tests.HttpMethodProviderTests
{
    [Parallelizable]
    public class RequestTypeProviderTest
    {
        private static readonly ClientValidationExceptionFactory ClientValidationExceptionFactory = new();

        public static IEnumerable ValidTestCases = new[]
        {
            new TestCaseData(new OptionsMethodAttribute(), RequestType.Info),
            new TestCaseData(new HeadMethodAttribute(), RequestType.Check),
            new TestCaseData(new GetMethodAttribute(), RequestType.Read),
            new TestCaseData(new PostMethodAttribute(), RequestType.Create),
            new TestCaseData(new PutMethodAttribute(), RequestType.Update),
            #if !NETFRAMEWORK
            new TestCaseData(new PatchMethodAttribute(), RequestType.PartialUpdate),
            #endif
            new TestCaseData(new DeleteMethodAttribute(), RequestType.Delete)
        };

        public static IEnumerable InvalidTestCases = new[]
        {
            new TestCaseData(null,
                new ArgumentNullException("operationAttribute")),
            new TestCaseData(new NotSupportedAttribute(),
                ClientValidationExceptionFactory.MethodAttributeNotSupported(nameof(NotSupportedAttribute)))
        };

        private RequestTypeProvider _requestTypeProvider = null!;

        [SetUp]
        public void SetUp()
        {
            var clientRequestExceptionFactory = new ClientValidationExceptionFactory();
            _requestTypeProvider = new RequestTypeProvider(clientRequestExceptionFactory);
        }

        [TestCaseSource(nameof(ValidTestCases))]
        public void Get_OperationAttribute_RequestType(OperationAttribute operationAttribute, RequestType expectedHttpMethod)
        {
            var requestType = _requestTypeProvider.Get(operationAttribute);

            requestType.Should().Be(expectedHttpMethod);
        }

        [TestCaseSource(nameof(InvalidTestCases))]
        public void Get_OperationAttribute_ThrowException(OperationAttribute operationAttribute, Exception exception)
        {
            _requestTypeProvider
                .Invoking(x => x.Get(operationAttribute))
                .Should()
                .Throw<Exception>()
                .Where(x => x.GetType() == exception.GetType())
                .WithMessage(exception.Message);
        }

        private class NotSupportedAttribute : OperationAttribute
        {
        }
    }
}
