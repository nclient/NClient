using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using FluentAssertions;
using NClient.Abstractions.HttpClients;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Exceptions;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Standalone.Tests.RequestBuilderTests
{
    [Parallelizable]
    [SuppressMessage("ReSharper", "BadDeclarationBracesLineBreaks")]
    public class RequestBuilderHeaderTest : RequestBuilderTestBase
    {
        private interface IPrimitiveHeader { [GetMethod] int Get([HeaderParam] int id); }

        [Test]
        public void Build_PrimitiveHeader_PrimitiveInHeader()
        {
            var httpRequest = BuildRequest(BuildMethod<IPrimitiveHeader>(), 1);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                headers: new[] { new HttpHeader("id", "1") });
        }

        private interface IStringHeader { [GetMethod] int Get([HeaderParam] string str); }

        [Test]
        public void Build_StringHeader_StringInHeader()
        {
            var httpRequest = BuildRequest(BuildMethod<IStringHeader>(), "value");

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                headers: new[] { new HttpHeader("str", "value") });
        }

        private interface IMultiplyPrimitiveHeaders { [GetMethod] int Get([HeaderParam] int id, [HeaderParam] string value); }

        [Test]
        public void Build_MultiplyPrimitiveHeaders_MultiplyHeadersWithPrimitives()
        {
            var httpRequest = BuildRequest(BuildMethod<IMultiplyPrimitiveHeaders>(), 1, "val");

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                headers: new[] { new HttpHeader("id", "1"), new HttpHeader("value", "val") });
        }

        private interface ICustomTypeHeader { [GetMethod] int Get([HeaderParam] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeHeader_ThrowClientValidationException()
        {
            Func<HttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<ICustomTypeHeader>(), new BasicEntity { Id = 1 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.ComplexTypeInHeaderNotSupported("entity").Message);
        }

        private interface IMultiplyCustomTypeHeader { [GetMethod] int Get([HeaderParam] BasicEntity entity1, [HeaderParam] BasicEntity entity2); }

        [Test]
        public void Build_MultiplyCustomTypeHeader_ThrowClientValidationException()
        {
            Func<HttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IMultiplyCustomTypeHeader>(), new BasicEntity { Id = 1 }, new BasicEntity { Id = 2 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.ComplexTypeInHeaderNotSupported("entity1").Message);
        }

        [Header("id", "1")] private interface IClientHeader { [GetMethod] int Get(); }

        [Test]
        public void Build_ClientHeader_PrimitiveInHeader()
        {
            var httpRequest = BuildRequest(BuildMethod<IClientHeader>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                headers: new[] { new HttpHeader("id", "1") });
        }

        [Header("id", "1")] private interface IDuplicateInClientAndMethodHeaders { [GetMethod, Header("id", "2")] int Get(); }

        [Test]
        public void Build_DuplicateInClientAndMethodHeaders_PrimitiveInHeader()
        {
            var httpRequest = BuildRequest(BuildMethod<IDuplicateInClientAndMethodHeaders>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                headers: new[] { new HttpHeader("id", "2") });
        }

        private interface IMethodHeader { [GetMethod, Header("id", "1")] int Get(); }

        [Test]
        public void Build_MethodHeader_PrimitiveInHeader()
        {
            var httpRequest = BuildRequest(BuildMethod<IMethodHeader>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                headers: new[] { new HttpHeader("id", "1") });
        }

        [Header("id", "1")] private interface IDuplicateInClientAndParamHeaders { [GetMethod] int Get([HeaderParam] int id); }

        [Test]
        public void Build_DuplicateInClientAndParamHeaders_ThrowClientValidationException()
        {
            Func<HttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IDuplicateInClientAndParamHeaders>(), 2);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.HeaderParamDuplicatesStaticHeader("id").Message);
        }

        private interface IDuplicateInMethodAndParamHeaders { [GetMethod, Header("id", "1")] int Get([HeaderParam] int id); }

        [Test]
        public void Build_DuplicateInMethodAndParamHeaders_ThrowClientValidationException()
        {
            Func<HttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IDuplicateInMethodAndParamHeaders>(), 2);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.HeaderParamDuplicatesStaticHeader("id").Message);
        }
    }
}
