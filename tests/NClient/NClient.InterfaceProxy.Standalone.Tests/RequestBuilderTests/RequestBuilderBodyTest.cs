using System;
using System.Net.Http;
using FluentAssertions;
using NClient.Core.Attributes;
using NClient.Core.Attributes.Methods;
using NClient.Core.Attributes.Parameters;
using NClient.Core.Exceptions;
using NClient.Core.Interceptors;
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
            AttributeMapper = new AttributeMapper();
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        public interface ICustomTypeBody { [GetMethod] int Get([BodyParam] BasicEntity entity); }

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

        public interface IMultipleBodyParameters { [GetMethod] int Get([BodyParam] BasicEntity entity1, [BodyParam] BasicEntity entity2); }

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

        public interface ICustomTypeBodyWithoutAttribute { [GetMethod] int Get(BasicEntity entity); }

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

        public interface IMultipleBodyParametersWithoutAttributes { [GetMethod] int Get(BasicEntity entity1, BasicEntity entity2); }

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

        public interface IPrimitiveBody { [GetMethod] int Get([BodyParam] int id); }

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

        [Path("api")] public interface IMultiplyPrimitiveBodyParameters { [GetMethod] int Get([BodyParam] int id, [BodyParam] string value); }

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
