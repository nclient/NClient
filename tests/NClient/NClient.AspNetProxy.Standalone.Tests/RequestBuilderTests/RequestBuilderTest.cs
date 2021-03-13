using System;
using Castle.DynamicProxy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NClient.AspNetProxy.Attributes;
using NClient.Core.Exceptions;
using NClient.Core.Interceptors;
using NClient.Providers.HttpClient;
using NClient.Testing.Common;
using NUnit.Framework;
using NotSupportedNClientException = NClient.Core.Exceptions.NotSupportedNClientException;

namespace NClient.AspNetProxy.Standalone.Tests.RequestBuilderTests
{
    [Parallelizable]
    public class RequestBuilderTest : RequestBuilderTestBase
    {
        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            AttributeHelper = new AspNetCoreAttributeHelper();
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        internal override HttpRequest BuildRequest(IInvocation invocation)
        {
            return RequestBuilder.Build(invocation.InvocationTarget.GetType(), invocation.MethodInvocationTarget, invocation.Arguments);
        }

        public interface INoAttributes { int Method(); }

        public class NoAttributes : ControllerBase, INoAttributes
        {
            public int Method() => 1;
        }

        [Test]
        public void Build_NoAttributes_ThrowAttributeNotFoundNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithTarget<INoAttributes>(new NoAttributes(), KeepDataInterceptor)
                .Method();

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<AttributeNotFoundNClientException>();
        }

        public interface INoMethodAttribute { int Method(); }

        [Route("api")]
        public class NoMethodAttribute : ControllerBase, INoMethodAttribute
        {
            public int Method() => 1;
        }

        [Test]
        public void Build_NoMethodAttribute_ThrowAttributeNotFoundNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithTarget<INoMethodAttribute>(new NoMethodAttribute(), KeepDataInterceptor)
                .Method();

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<AttributeNotFoundNClientException>();
        }

        public interface IApiControllerAttribute { int Method(); }

        [Microsoft.AspNetCore.Mvc.ApiController]
        public class ApiControllerAttribute : ControllerBase, IApiControllerAttribute
        {
            public int Method() => 1;
        }

        [Test]
        public void Build_ApiControllerAttribute_ThrowAttributeNotFoundNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithTarget<IApiControllerAttribute>(new ApiControllerAttribute(), KeepDataInterceptor)
                .Method();

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<AttributeNotFoundNClientException>();
        }

        public interface IMultiplyCommonRoutes { int Method(); }

        [Route("api")]
        [Route("api/[controller]")]
        public class MultiplyCommonRoutes : ControllerBase, IMultiplyCommonRoutes
        {
            [HttpGet] public int Method() => 1;
        }

        [Test]
        public void Build_MultiplyCommonRoutes_ThrowNotSupportedNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithTarget<IMultiplyCommonRoutes>(new MultiplyCommonRoutes(), KeepDataInterceptor)
                .Method();

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NotSupportedNClientException>();
        }

        public interface IMultiplyMethodRoutes { int Method(int id); }

        [Route("api")]
        public class MultiplyMethodRoutes : ControllerBase, IMultiplyMethodRoutes
        {
            [HttpGet] 
            [HttpGet("[action]")] 
            [HttpGet("[action]/{id}")] 
            public int Method(int id) => 1;
        }

        [Test]
        public void Build_MultiplyMethodRoutes_ThrowNotSupportedNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithTarget<IMultiplyMethodRoutes>(new MultiplyMethodRoutes(), KeepDataInterceptor)
                .Method(1);

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NotSupportedNClientException>();
        }

        public interface IMultiplyMethodRoutesWithRouteAttribute { int Method(int id); }

        [Route("api")]
        public class MultiplyMethodRoutesWithRouteAttribute : ControllerBase, IMultiplyMethodRoutesWithRouteAttribute
        {
            [Route("")]
            [Route("[action]")]
            [Route("[action]/{id}")]
            public int Method(int id) => 1;
        }

        [Test]
        public void Build_MultiplyMethodRoutesWithRouteAttribute_ThrowNotSupportedNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithTarget<IMultiplyMethodRoutesWithRouteAttribute>(new MultiplyMethodRoutesWithRouteAttribute(), KeepDataInterceptor)
                .Method(1);

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NotSupportedNClientException>();
        }

        public interface IMultiplyMethodRoutesWithRouteAndGetAttribute { int Method(int id); }

        [Route("api")]
        public class MultiplyMethodRoutesWithRouteAndGetAttribute : ControllerBase, IMultiplyMethodRoutesWithRouteAndGetAttribute
        {
            [HttpGet]
            [HttpGet("[action]")]
            [Route("[action]/{id}")]
            public int Method(int id) => 1;
        }

        [Test]
        public void Build_MultiplyMethodRoutesWithRouteAndGetAttribute_ThrowNotSupportedNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithTarget<IMultiplyMethodRoutesWithRouteAndGetAttribute>(new MultiplyMethodRoutesWithRouteAndGetAttribute(), KeepDataInterceptor)
                .Method(1);

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NotSupportedNClientException>();
        }
    }
}
