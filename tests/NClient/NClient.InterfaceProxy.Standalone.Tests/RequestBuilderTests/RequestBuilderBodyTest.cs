using System;
using System.Net.Http;
using FluentAssertions;
using NClient.Core.Exceptions;
using NClient.Core.Interceptors;
using NClient.InterfaceProxy.Attributes;
using NClient.InterfaceProxy.Attributes.Methods;
using NClient.InterfaceProxy.Attributes.Parameters;
using NClient.Providers.HttpClient;
using NClient.Testing.Common;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.InterfaceProxy.Standalone.Tests.RequestBuilderTests
{
    [Parallelizable]
    public class RequestBuilderBodyTest : RequestBuilderTestBase
    {
        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            AttributeHelper = new AttributeHelper();
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        [Api] public interface ICustomTypeBody { [AsHttpGet] int Get([ToBody] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeBody_JsonObjectInBody()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeBody>(KeepDataInterceptor)
                .Get(new BasicEntity { Id = 1 });

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                body: new BasicEntity { Id = 1 });
        }

        [Api] public interface IMultipleBodyParameters { [AsHttpGet] int Get([ToBody] BasicEntity entity1, [ToBody] BasicEntity entity2); }

        [Test]
        public void Build_MultipleBodyParameters_ThrowNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMultipleBodyParameters>(KeepDataInterceptor)
                .Get(new BasicEntity { Id = 1 }, new BasicEntity { Id = 2 });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NClientException>();
        }

        [Api] public interface ICustomTypeBodyWithoutAttribute { [AsHttpGet] int Get(BasicEntity entity); }

        [Test]
        public void Build_CustomTypeBodyWithoutAttribute_JsonObjectInBody()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeBodyWithoutAttribute>(KeepDataInterceptor)
                .Get(new BasicEntity { Id = 1 });

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest, 
                new Uri("http://localhost:5000/"), 
                HttpMethod.Get, 
                body: new BasicEntity { Id = 1 });
        }

        [Api] public interface IMultipleBodyParametersWithoutAttributes {[AsHttpGet] int Get(BasicEntity entity1, BasicEntity entity2); }

        [Test]
        public void Build_MultipleBodyParametersWithoutAttributes_NClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMultipleBodyParametersWithoutAttributes>(KeepDataInterceptor)
                .Get(new BasicEntity { Id = 1 }, new BasicEntity { Id = 2 });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NClientException>();
        }

        [Api] public interface IPrimitiveBody {[AsHttpGet] int Get([ToBody] int id); }

        [Test]
        public void Build_PrimitiveBody_PrimitiveInBody()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveBody>(KeepDataInterceptor)
                .Get(1);

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                body: 1);
        }

        [Api("api")] public interface IMultiplyPrimitiveBodyParameters {[AsHttpGet] int Get([ToBody] int id, [ToBody] string value); }

        [Test]
        public void Build_MultiplyPrimitiveBodyParameters_ThrowNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMultiplyPrimitiveBodyParameters>(KeepDataInterceptor)
                .Get(1, "val");

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NClientException>();
        }
    }
}
