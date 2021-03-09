using System;
using System.Net.Http;
using NClient.Core.Interceptors;
using NClient.InterfaceProxy.Attributes;
using NClient.InterfaceProxy.Attributes.Methods;
using NClient.Testing.Common;
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

        [Api("api")] public interface IStaticRoute {[AsHttpGet("action")] int Method(); }

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

        [Api("api")] public interface IStaticRouteWithActionToken {[AsHttpGet("action/[action]")] int Method(); }

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
    }
}
