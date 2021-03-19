using System;
using System.Net.Http;
using FluentAssertions;
using NClient.Core.Attributes;
using NClient.Core.Attributes.Clients;
using NClient.Core.Attributes.Clients.Methods;
using NClient.Core.Attributes.Clients.Parameters;
using NClient.Core.Exceptions;
using NClient.Core.Interceptors;
using NClient.Providers.HttpClient;
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
            AttributeMapper = new StubAttributeMapper();
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        [Api] public interface IPrimitiveHeader { [AsHttpGet] int Get([ToHeader] int id); }

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

        [Api] public interface IStringHeader {[AsHttpGet] int Get([ToHeader] string str); }

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

        [Api] public interface IMultiplyPrimitiveHeaders {[AsHttpGet] int Get([ToHeader] int id, [ToHeader] string value); }

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

        [Api] public interface ICustomTypeHeader { [AsHttpGet] int Get([ToHeader] BasicEntity entity); }

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

        [Api] public interface IMultiplyCustomTypeHeader { [AsHttpGet] int Get([ToHeader] BasicEntity entity1, [ToHeader] BasicEntity entity2); }

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
