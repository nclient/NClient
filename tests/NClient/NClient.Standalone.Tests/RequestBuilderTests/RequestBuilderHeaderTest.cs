using System;
using System.Net.Http;
using FluentAssertions;
using NClient.Abstractions.HttpClients;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions;
using NClient.Testing.Common;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Standalone.Tests.RequestBuilderTests
{
    [Parallelizable]
    public class RequestBuilderHeaderTest : RequestBuilderTestBase
    {
        private interface IPrimitiveHeader {[GetMethod] int Get([HeaderParam] int id); }

        [Test]
        public void Build_PrimitiveHeader_PrimitiveInHeader()
        {
            var httpRequest = BuildRequest(BuildMethod<IPrimitiveHeader>(), 1);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                headers: new[] { new HttpHeader("id", "1") });
        }

        private interface IStringHeader {[GetMethod] int Get([HeaderParam] string str); }

        [Test]
        public void Build_StringHeader_StringInHeader()
        {
            var httpRequest = BuildRequest(BuildMethod<IStringHeader>(), "value");

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                headers: new[] { new HttpHeader("str", "value") });
        }

        private interface IMultiplyPrimitiveHeaders {[GetMethod] int Get([HeaderParam] int id, [HeaderParam] string value); }

        [Test]
        public void Build_MultiplyPrimitiveHeaders_MultiplyHeadersWithPrimitives()
        {
            var httpRequest = BuildRequest(BuildMethod<IMultiplyPrimitiveHeaders>(), 1, "val");

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                headers: new[] { new HttpHeader("id", "1"), new HttpHeader("value", "val"), });
        }

        private interface ICustomTypeHeader {[GetMethod] int Get([HeaderParam] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeHeader_ThrowNClientException()
        {
            Func<HttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<ICustomTypeHeader>(), new BasicEntity { Id = 1 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NClientException>();
        }

        private interface IMultiplyCustomTypeHeader {[GetMethod] int Get([HeaderParam] BasicEntity entity1, [HeaderParam] BasicEntity entity2); }

        [Test]
        public void Build_MultiplyCustomTypeHeader_ThrowNClientException()
        {
            Func<HttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IMultiplyCustomTypeHeader>(), new BasicEntity { Id = 1 }, new BasicEntity { Id = 2 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NClientException>();
        }
    }
}
