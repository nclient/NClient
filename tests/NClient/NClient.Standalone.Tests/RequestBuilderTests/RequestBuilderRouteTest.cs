using System;
using System.Net.Http;
using FluentAssertions;
using NClient.Abstractions.HttpClients;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions;
using NClient.Core.Interceptors;
using NClient.Core.Mappers;
using NClient.Testing.Common;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Standalone.Tests.RequestBuilderTests
{
    [Parallelizable]
    public class RequestBuilderRouteTest : RequestBuilderTestBase
    {
        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            AttributeMapper = new AttributeMapper();
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        [Path("api")] public interface ICommonStaticRoute { [GetMethod] int Method(); }

        [Test]
        public void Build_CommonStaticRoute_OnlyCommonStaticRoute()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICommonStaticRoute>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api"),
                HttpMethod.Get);
        }

        [Path("api/[controller]")] public interface ICommonStaticRouteWithControllerToken { [GetMethod] int Method(); }

        [Test]
        public void Build_CommonStaticRouteWithControllerToken_StaticRouteWithInterfaceName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICommonStaticRouteWithControllerToken>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/CommonStaticRouteWithControllerToken"),
                HttpMethod.Get);
        }

        [Path("api/[controller]")] public interface IStaticRouteWithControllerToken { [GetMethod("entity")] int Method(); }

        [Test]
        public void Build_StaticRouteWithControllerToken_StaticRouteWithInterfaceName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticRouteWithControllerToken>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/StaticRouteWithControllerToken/entity"),
                HttpMethod.Get);
        }

        [Path("api/[controller]")] public interface IStaticRouteWithControllerAndActionTokens { [GetMethod("[action]")] int Method(); }

        [Test]
        public void Build_StaticRouteWithControllerAndActionTokens_StaticRouteWithInterfaceAndMethodNames()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticRouteWithControllerAndActionTokens>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/StaticRouteWithControllerAndActionTokens/Method"),
                HttpMethod.Get);
        }

        [Path("api")] public interface IStaticRoute { [GetMethod("action")] int Method(); }

        [Test]
        public void Build_StaticRoute_StaticRoute()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticRoute>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/action"),
                HttpMethod.Get);
        }

        [Path("/api")] public interface IClientWithRootedRoute { [GetMethod("action")] int Method(); }

        [Test]
        public void Build_ClientWithRootedRoute_ExtraSlashRemoved()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IClientWithRootedRoute>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/action"),
                HttpMethod.Get);
        }

        [Path("api")] public interface IOverrideClientRoute { [GetMethod("/action")] int Method(); }

        [Test]
        public void Build_OverrideClientRoute_IgnoreClientRoute()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IOverrideClientRoute>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/action"),
                HttpMethod.Get);
        }

        [Path("api/")] public interface IClientRouteEndsWithSlash { [GetMethod("action")] int Method(); }

        [Test]
        public void Build_ClientRouteEndsWithSlash_ExtraSlashRemoved()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IClientRouteEndsWithSlash>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/action"),
                HttpMethod.Get);
        }

        [Path("api")] public interface IMethodRouteEndsWithSlash { [GetMethod("action/")] int Method(); }

        [Test]
        public void Build_MethodRouteEndsWithSlash_ExtraSlashRemoved()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMethodRouteEndsWithSlash>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/action"),
                HttpMethod.Get);
        }

        [Path("api")] public interface IStaticRouteWithActionToken { [GetMethod("action/[action]")] int Method(); }

        [Test]
        public void Build_StaticRouteWithActionToken_StaticRouteWithMethodName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticRouteWithActionToken>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/action/Method"),
                HttpMethod.Get);
        }

        [Path("[action]")] public interface IApiRouteWithActionToken { [GetMethod] int Method(); }

        [Test]
        public void Build_ApiRouteWithActionToken_RouteWithMethodName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IApiRouteWithActionToken>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/Method"),
                HttpMethod.Get);
        }

        public interface IMethodRouteWithControllerToken { [GetMethod("[controller]")] int Method(); }

        [Test]
        public void Build_MethodRouteWithControllerToken_RouteWithInterfaceName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMethodRouteWithControllerToken>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/MethodRouteWithControllerToken"),
                HttpMethod.Get);
        }

        public interface IMethodRouteWithPrimitiveParamTokenWithoutAttribute { [GetMethod("{id}")] int Method(int id); }

        [Test]
        public void Build_MethodRouteWithPrimitiveParamTokenWithoutAttribute_RouteWithParamValue()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMethodRouteWithPrimitiveParamTokenWithoutAttribute>(KeepDataInterceptor)
                .Method(1);

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/1"),
                HttpMethod.Get);
        }

        [Path("{id}")] public interface IApiRouteWithPrimitiveParamTokenWithoutAttribute { [GetMethod] int Method(int id); }

        [Test]
        public void Build_ApiRouteWithPrimitiveParamTokenWithoutAttribute_RouteWithParamValue()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IApiRouteWithPrimitiveParamTokenWithoutAttribute>(KeepDataInterceptor)
                .Method(1);

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/1"),
                HttpMethod.Get);
        }

        public interface IPrimitiveRouteParam { [GetMethod("{id}")] int Method([RouteParam] int id); }

        [Test]
        public void Build_PrimitiveRouteParam_RouteWithParamValue()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveRouteParam>(KeepDataInterceptor)
                .Method(1);

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/1"),
                HttpMethod.Get);
        }

        public interface IPrimitiveRouteParamWithoutTokenInRoute { [GetMethod] int Method([RouteParam] int id); }

        [Test]
        public void Build_PrimitiveRouteParamWithoutTokenInRoute_ThrowInvalidRouteNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveRouteParamWithoutTokenInRoute>(KeepDataInterceptor)
                .Method(1);

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        public interface IMethodRouteWithCustomTypeParamToken { [GetMethod("{entity}")] int Method(BasicEntity entity); }

        [Test]
        public void Build_MethodRouteWithCustomTypeParamToken_ThrowInvalidRouteNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMethodRouteWithCustomTypeParamToken>(KeepDataInterceptor)
                .Method(new BasicEntity { Id = 1, Value = 2 });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        [Path("{entity}")] public interface IApiRouteWithCustomTypeParamToken { [GetMethod] int Method(BasicEntity entity); }

        [Test]
        public void Build_ApiRouteWithCustomTypeParamToken_ThrowInvalidRouteNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IApiRouteWithCustomTypeParamToken>(KeepDataInterceptor)
                .Method(new BasicEntity { Id = 1, Value = 2 });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        public interface ICustomTypeRouteParam { [GetMethod("{id}")] int Method([RouteParam] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeRouteParam_ThrowInvalidRouteNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeRouteParam>(KeepDataInterceptor)
                .Method(new BasicEntity { Id = 1, Value = 2 });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        public interface ICustomTypeRouteParamWithoutTokenInRoute { [GetMethod] int Method([RouteParam] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeRouteParamWithoutTokenInRoute_ThrowInvalidRouteNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeRouteParamWithoutTokenInRoute>(KeepDataInterceptor)
                .Method(new BasicEntity { Id = 1, Value = 2 });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidRouteNClientException>();
        }
    }
}
