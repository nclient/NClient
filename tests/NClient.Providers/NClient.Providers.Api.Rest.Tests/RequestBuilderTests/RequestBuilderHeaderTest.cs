using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Annotations.Http;
using NClient.Exceptions;
using NClient.Providers.Transport;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Providers.Api.Rest.Tests.RequestBuilderTests
{
    [Parallelizable]
    [SuppressMessage("ReSharper", "BadDeclarationBracesLineBreaks")]
    public class RequestBuilderHeaderTest : RequestBuilderTestBase
    {
        private interface IPrimitiveHeader { [GetMethod] int Get([HeaderParam] int id); }

        [Test]
        public async Task Build_PrimitiveHeader_PrimitiveInHeader()
        {
            var httpRequest = BuildRequest(BuildMethod<IPrimitiveHeader>(), 1);

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/"),
                RequestType.Read,
                metadatas: new[] { new Metadata("id", "1") });
        }

        private interface IStringHeader { [GetMethod] int Get([HeaderParam] string str); }

        [Test]
        public async Task Build_StringHeader_StringInHeader()
        {
            var httpRequest = BuildRequest(BuildMethod<IStringHeader>(), "value");

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/"),
                RequestType.Read,
                metadatas: new[] { new Metadata("str", "value") });
        }

        private interface IMultiplyPrimitiveHeaders { [GetMethod] int Get([HeaderParam] int id, [HeaderParam] string value); }

        [Test]
        public async Task Build_MultiplyPrimitiveHeaders_MultiplyHeadersWithPrimitives()
        {
            var httpRequest = BuildRequest(BuildMethod<IMultiplyPrimitiveHeaders>(), 1, "val");

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/"),
                RequestType.Read,
                metadatas: new[] { new Metadata("id", "1"), new Metadata("value", "val") });
        }

        private interface ICustomTypeHeader { [GetMethod] int Get([HeaderParam] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeHeader_ThrowClientValidationException()
        {
            Func<IRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<ICustomTypeHeader>(), new BasicEntity { Id = 1 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(RestClientValidationExceptionFactory.ComplexTypeInHeaderNotSupported("entity").Message);
        }

        private interface IMultiplyCustomTypeHeader { [GetMethod] int Get([HeaderParam] BasicEntity entity1, [HeaderParam] BasicEntity entity2); }

        [Test]
        public void Build_MultiplyCustomTypeHeader_ThrowClientValidationException()
        {
            Func<IRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IMultiplyCustomTypeHeader>(), new BasicEntity { Id = 1 }, new BasicEntity { Id = 2 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(RestClientValidationExceptionFactory.ComplexTypeInHeaderNotSupported("entity1").Message);
        }

        [Header("id", "1")] private interface IClientHeader { [GetMethod] int Get(); }

        [Test]
        public async Task Build_ClientHeader_PrimitiveInHeader()
        {
            var httpRequest = BuildRequest(BuildMethod<IClientHeader>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/"),
                RequestType.Read,
                metadatas: new[] { new Metadata("id", "1") });
        }

        [Header("id", "1")] private interface IDuplicateInClientAndMethodHeaders { [GetMethod, Header("id", "2")] int Get(); }

        [Test]
        public async Task Build_DuplicateInClientAndMethodHeaders_PrimitiveInHeader()
        {
            var httpRequest = BuildRequest(BuildMethod<IDuplicateInClientAndMethodHeaders>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/"),
                RequestType.Read,
                metadatas: new[] { new Metadata("id", "1"), new Metadata("id", "2") });
        }

        private interface IMethodHeader { [GetMethod, Header("id", "1")] int Get(); }

        [Test]
        public async Task Build_MethodHeader_PrimitiveInHeader()
        {
            var httpRequest = BuildRequest(BuildMethod<IMethodHeader>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/"),
                RequestType.Read,
                metadatas: new[] { new Metadata("id", "1") });
        }

        [Header("id", "1")] private interface IDuplicateInClientAndParamHeaders { [GetMethod] int Get([HeaderParam] int id); }

        [Test]
        public async Task Build_DuplicateInClientAndParamHeaders_ThrowClientValidationException()
        {
            var httpRequest = BuildRequest(
                BuildMethod<IDuplicateInClientAndParamHeaders>(), 2);

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/"),
                RequestType.Read,
                metadatas: new[] { new Metadata("id", "1"), new Metadata("id", "2") });
        }

        private interface IDuplicateInMethodAndParamHeaders { [GetMethod, Header("id", "1")] int Get([HeaderParam] int id); }

        [Test]
        public async Task Build_DuplicateInMethodAndParamHeaders_ThrowClientValidationException()
        {
            var httpRequest = BuildRequest(
                BuildMethod<IDuplicateInMethodAndParamHeaders>(), 2);

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/"),
                RequestType.Read,
                metadatas: new[] { new Metadata("id", "1"), new Metadata("id", "2") });
        }
    }
}
