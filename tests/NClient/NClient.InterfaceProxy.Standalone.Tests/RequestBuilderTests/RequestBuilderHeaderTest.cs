using System;
using System.Net.Http;
using FluentAssertions;
using NClient.Abstractions.HttpClients;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions;
using NClient.Core.Interceptors;
using NClient.Core.Mappers;
using NClient.Testing.Common;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.InterfaceProxy.Standalone.Tests.RequestBuilderTests
{
    [Parallelizable]
    public class RequestBuilderHeaderTest : RequestBuilderTestBase
    {
        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            AttributeMapper = new AttributeMapper();
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        public interface IPrimitiveHeader { [GetMethod] int Get([HeaderParam] int id); }

        [Test]
        public void Build_PrimitiveHeader_PrimitiveInHeader()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveHeader>(KeepDataInterceptor)
                .Get(1);

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                headers: new[] { new HttpHeader("id", "1") });
        }

        public interface IStringHeader {[GetMethod] int Get([HeaderParam] string str); }

        [Test]
        public void Build_StringHeader_StringInHeader()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStringHeader>(KeepDataInterceptor)
                .Get("value");

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                headers: new[] { new HttpHeader("str", "value") });
        }

        public interface IMultiplyPrimitiveHeaders {[GetMethod] int Get([HeaderParam] int id, [HeaderParam] string value); }

        [Test]
        public void Build_MultiplyPrimitiveHeaders_MultiplyHeadersWithPrimitives()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMultiplyPrimitiveHeaders>(KeepDataInterceptor)
                .Get(1, "val");

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                headers: new[] { new HttpHeader("id", "1"), new HttpHeader("value", "val"), });
        }

        public interface ICustomTypeHeader { [GetMethod] int Get([HeaderParam] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeHeader_ThrowNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeHeader>(KeepDataInterceptor)
                .Get(new BasicEntity { Id = 1 });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NClientException>();
        }

        public interface IMultiplyCustomTypeHeader { [GetMethod] int Get([HeaderParam] BasicEntity entity1, [HeaderParam] BasicEntity entity2); }

        [Test]
        public void Build_MultiplyCustomTypeHeader_ThrowNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMultiplyCustomTypeHeader>(KeepDataInterceptor)
                .Get(new BasicEntity { Id = 1 }, new BasicEntity { Id = 2 });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NClientException>();
        }
    }
}
