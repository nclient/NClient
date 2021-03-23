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
    public class RequestBuilderRouteTest : RequestBuilderTestBase
    {
        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            AttributeMapper = new AttributeMapper();
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        [Client("api")] public interface ICommonStaticRoute { [AsHttpGet] int Method(); }

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

        [Client("api/[controller]")] public interface ICommonStaticRouteWithControllerToken { [AsHttpGet] int Method(); }

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

        [Client("api/[controller]")] public interface IStaticRouteWithControllerToken { [AsHttpGet("entity")] int Method(); }

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

        [Client("api/[controller]")] public interface IStaticRouteWithControllerAndActionTokens { [AsHttpGet("[action]")] int Method(); }

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

        [Client("api")] public interface IStaticRoute { [AsHttpGet("action")] int Method(); }

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

        [Client("/api")] public interface IClientWithRootedRoute { [AsHttpGet("action")] int Method(); }

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

        [Client("api")] public interface IOverrideClientRoute { [AsHttpGet("/action")] int Method(); }

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

        [Client("api/")] public interface IClientRouteEndsWithSlash { [AsHttpGet("action")] int Method(); }

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

        [Client("api")] public interface IMethodRouteEndsWithSlash { [AsHttpGet("action/")] int Method(); }

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

        [Client("api")] public interface IStaticRouteWithActionToken { [AsHttpGet("action/[action]")] int Method(); }

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

        [Client("[action]")] public interface IApiRouteWithActionToken { [AsHttpGet] int Method(); }

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

        [Client] public interface IMethodRouteWithControllerToken { [AsHttpGet("[controller]")] int Method(); }

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

        [Client] public interface IMethodRouteWithPrimitiveParamTokenWithoutAttribute { [AsHttpGet("{id}")] int Method(int id); }

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

        [Client("{id}")] public interface IApiRouteWithPrimitiveParamTokenWithoutAttribute { [AsHttpGet] int Method(int id); }

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

        [Client] public interface IPrimitiveRouteParam { [AsHttpGet("{id}")] int Method([ToRoute] int id); }

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

        [Client] public interface IPrimitiveRouteParamWithoutTokenInRoute { [AsHttpGet] int Method([ToRoute] int id); }

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

        [Client] public interface IMethodRouteWithCustomTypeParamToken { [AsHttpGet("{entity}")] int Method(BasicEntity entity); }

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

        [Client("{entity}")] public interface IApiRouteWithCustomTypeParamToken { [AsHttpGet] int Method(BasicEntity entity); }

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

        [Client] public interface ICustomTypeRouteParam { [AsHttpGet("{id}")] int Method([ToRoute] BasicEntity entity); }

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

        [Client] public interface ICustomTypeRouteParamWithoutTokenInRoute { [AsHttpGet] int Method([ToRoute] BasicEntity entity); }

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
