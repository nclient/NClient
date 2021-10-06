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
    public class RequestBuilderBodyTest : RequestBuilderTestBase
    {
        private interface ICustomTypeBody { [GetMethod] int Get([BodyParam] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeBody_JsonObjectInBody()
        {
            var basicEntity = new BasicEntity { Id = 1 };

            var httpRequest = BuildRequest(BuildMethod<ICustomTypeBody>(), basicEntity);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                body: basicEntity);
        }

        private interface IMultipleBodyParameters { [GetMethod] int Get([BodyParam] BasicEntity entity1, [BodyParam] BasicEntity entity2); }

        [Test]
        public void Build_MultipleBodyParameters_ThrowClientValidationException()
        {
            Func<IHttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IMultipleBodyParameters>(), new BasicEntity { Id = 1 }, new BasicEntity { Id = 2 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .ThrowExactly<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.MultipleBodyParametersNotSupported().Message);
        }

        private interface ICustomTypeBodyWithoutAttribute { [GetMethod] int Get(BasicEntity entity); }

        [Test]
        public void Build_CustomTypeBodyWithoutAttribute_JsonObjectInBody()
        {
            var basicEntity = new BasicEntity { Id = 1 };

            var httpRequest = BuildRequest(BuildMethod<ICustomTypeBodyWithoutAttribute>(), basicEntity);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                body: basicEntity);
        }

        private interface IMultipleBodyParametersWithoutAttributes { [GetMethod] int Get(BasicEntity entity1, BasicEntity entity2); }

        [Test]
        public void Build_MultipleBodyParametersWithoutAttributes_ThrowClientValidationException()
        {
            Func<IHttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IMultipleBodyParametersWithoutAttributes>(), new BasicEntity { Id = 1 }, new BasicEntity { Id = 2 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .ThrowExactly<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.MultipleBodyParametersNotSupported().Message);
        }

        private interface IPrimitiveBody { [GetMethod] int Get([BodyParam] int id); }

        [Test]
        public void Build_PrimitiveBody_PrimitiveInBody()
        {
            const int id = 1;

            var httpRequest = BuildRequest(BuildMethod<IPrimitiveBody>(), id);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                body: id);
        }

        [Path("api")] private interface IMultiplyPrimitiveBodyParameters { [GetMethod] int Get([BodyParam] int id, [BodyParam] string value); }

        [Test]
        public void Build_MultiplyPrimitiveBodyParameters_ThrowClientValidationException()
        {
            Func<IHttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IMultiplyPrimitiveBodyParameters>(), 1, "val");

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .ThrowExactly<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.MultipleBodyParametersNotSupported().Message);
        }
    }
}
