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
    public class RequestBuilderRouteTest : RequestBuilderTestBase
    {
        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            AttributeHelper = new AttributeHelper();
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        [Api("api")] public interface ICommonStaticRoute { [AsHttpGet] int Method(); }

        [Test]
        public void Build_CommonStaticRoute_OnlyCommonStaticRoute()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICommonStaticRoute>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api"),
                HttpMethod.Get);
        }

        [Api("api/[controller]")] public interface ICommonStaticRouteWithControllerToken { [AsHttpGet] int Method(); }

        [Test]
        public void Build_CommonStaticRouteWithControllerToken_StaticRouteWithInterfaceName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICommonStaticRouteWithControllerToken>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/CommonStaticRouteWithControllerToken"),
                HttpMethod.Get);
        }

        [Api("api/[controller]")] public interface IStaticRouteWithControllerToken { [AsHttpGet("entity")] int Method(); }

        [Test]
        public void Build_StaticRouteWithControllerToken_StaticRouteWithInterfaceName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticRouteWithControllerToken>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/StaticRouteWithControllerToken/entity"),
                HttpMethod.Get);
        }

        [Api("api/[controller]")] public interface IStaticRouteWithControllerAndActionTokens { [AsHttpGet("[action]")] int Method(); }

        [Test]
        public void Build_StaticRouteWithControllerAndActionTokens_StaticRouteWithInterfaceAndMethodNames()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticRouteWithControllerAndActionTokens>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/StaticRouteWithControllerAndActionTokens/Method"),
                HttpMethod.Get);
        }

        [Api("api")] public interface IStaticRoute { [AsHttpGet("action")] int Method(); }

        [Test]
        public void Build_StaticRoute_StaticRoute()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticRoute>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/action"),
                HttpMethod.Get);
        }

        [Api("api")] public interface IStaticRouteWithActionToken { [AsHttpGet("action/[action]")] int Method(); }

        [Test]
        public void Build_StaticRouteWithActionToken_StaticRouteWithMethodName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticRouteWithActionToken>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/action/Method"),
                HttpMethod.Get);
        }

        [Api("[action]")] public interface IApiRouteWithActionToken { [AsHttpGet] int Method(); }

        [Test]
        public void Build_ApiRouteWithActionToken_RouteWithMethodName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IApiRouteWithActionToken>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/Method"),
                HttpMethod.Get);
        }

        [Api] public interface IMethodRouteWithControllerToken { [AsHttpGet("[controller]")] int Method(); }

        [Test]
        public void Build_MethodRouteWithControllerToken_RouteWithInterfaceName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMethodRouteWithControllerToken>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/MethodRouteWithControllerToken"),
                HttpMethod.Get);
        }

        [Api] public interface IMethodRouteWithPrimitiveParamTokenWithoutAttribute { [AsHttpGet("{id}")] int Method(int id); }

        [Test]
        public void Build_MethodRouteWithPrimitiveParamTokenWithoutAttribute_RouteWithParamValue()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMethodRouteWithPrimitiveParamTokenWithoutAttribute>(KeepDataInterceptor)
                .Method(1);

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/1"),
                HttpMethod.Get);
        }

        [Api("{id}")] public interface IApiRouteWithPrimitiveParamTokenWithoutAttribute { [AsHttpGet] int Method(int id); }

        [Test]
        public void Build_ApiRouteWithPrimitiveParamTokenWithoutAttribute_RouteWithParamValue()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IApiRouteWithPrimitiveParamTokenWithoutAttribute>(KeepDataInterceptor)
                .Method(1);

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/1"),
                HttpMethod.Get);
        }

        [Api] public interface IPrimitiveRouteParam { [AsHttpGet("{id}")] int Method([ToRoute] int id); }

        [Test]
        public void Build_PrimitiveRouteParam_RouteWithParamValue()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveRouteParam>(KeepDataInterceptor)
                .Method(1);

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/1"),
                HttpMethod.Get);
        }

        [Api] public interface IPrimitiveRouteParamWithoutTokenInRoute { [AsHttpGet] int Method([ToRoute] int id); }

        [Test]
        public void Build_PrimitiveRouteParamWithoutTokenInRoute_ThrowInvalidRouteNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveRouteParamWithoutTokenInRoute>(KeepDataInterceptor)
                .Method(1);

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        [Api] public interface IMethodRouteWithCustomTypeParamToken { [AsHttpGet("{entity}")] int Method(BasicEntity entity); }

        [Test]
        public void Build_MethodRouteWithCustomTypeParamToken_ThrowInvalidRouteNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMethodRouteWithCustomTypeParamToken>(KeepDataInterceptor)
                .Method(new BasicEntity { Id = 1, Value = 2 });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        [Api("{entity}")] public interface IApiRouteWithCustomTypeParamToken { [AsHttpGet] int Method(BasicEntity entity); }

        [Test]
        public void Build_ApiRouteWithCustomTypeParamToken_ThrowInvalidRouteNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IApiRouteWithCustomTypeParamToken>(KeepDataInterceptor)
                .Method(new BasicEntity { Id = 1, Value = 2 });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        [Api] public interface ICustomTypeRouteParam { [AsHttpGet("{id}")] int Method([ToRoute] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeRouteParam_ThrowInvalidRouteNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeRouteParam>(KeepDataInterceptor)
                .Method(new BasicEntity { Id = 1, Value = 2 });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        [Api] public interface ICustomTypeRouteParamWithoutTokenInRoute { [AsHttpGet] int Method([ToRoute] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeRouteParamWithoutTokenInRoute_ThrowInvalidRouteNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeRouteParamWithoutTokenInRoute>(KeepDataInterceptor)
                .Method(new BasicEntity { Id = 1, Value = 2 });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidRouteNClientException>();
        }
    }
}
