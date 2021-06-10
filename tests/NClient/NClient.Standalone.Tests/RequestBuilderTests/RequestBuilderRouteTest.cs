using System;
using System.Net.Http;
using FluentAssertions;
using NClient.Abstractions.HttpClients;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Annotations.Versioning;
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
        [Path("api")] private interface ICommonStaticRoute {[GetMethod] int Method(); }

        [Test]
        public void Build_CommonStaticRoute_OnlyCommonStaticRoute()
        {
            var httpRequest = BuildRequest(BuildMethod<ICommonStaticRoute>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api"),
                HttpMethod.Get);
        }

        [Path("api/[controller]")] private interface ICommonStaticRouteWithControllerToken {[GetMethod] int Method(); }

        [Test]
        public void Build_CommonStaticRouteWithControllerToken_StaticRouteWithInterfaceName()
        {
            var httpRequest = BuildRequest(BuildMethod<ICommonStaticRouteWithControllerToken>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/CommonStaticRouteWithControllerToken"),
                HttpMethod.Get);
        }

        [Path("api/[controller]")] private interface IStaticRouteWithControllerToken {[GetMethod("entity")] int Method(); }

        [Test]
        public void Build_StaticRouteWithControllerToken_StaticRouteWithInterfaceName()
        {
            var httpRequest = BuildRequest(BuildMethod<IStaticRouteWithControllerToken>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/StaticRouteWithControllerToken/entity"),
                HttpMethod.Get);
        }

        [Path("api/[controller]")] private interface IStaticRouteWithControllerAndActionTokens {[GetMethod("[action]")] int Method(); }

        [Test]
        public void Build_StaticRouteWithControllerAndActionTokens_StaticRouteWithInterfaceAndMethodNames()
        {
            var httpRequest = BuildRequest(BuildMethod<IStaticRouteWithControllerAndActionTokens>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/StaticRouteWithControllerAndActionTokens/Method"),
                HttpMethod.Get);
        }

        [Path("api")] private interface IStaticRoute {[GetMethod("action")] int Method(); }

        [Test]
        public void Build_StaticRoute_StaticRoute()
        {
            var httpRequest = BuildRequest(BuildMethod<IStaticRoute>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/action"),
                HttpMethod.Get);
        }

        [Path("/api")] private interface IClientWithRootedRoute {[GetMethod("action")] int Method(); }

        [Test]
        public void Build_ClientWithRootedRoute_ExtraSlashRemoved()
        {
            var httpRequest = BuildRequest(BuildMethod<IClientWithRootedRoute>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/action"),
                HttpMethod.Get);
        }

        [Path("api")] private interface IOverrideClientRoute {[GetMethod("/action")] int Method(); }

        [Test]
        public void Build_OverrideClientRoute_IgnoreClientRoute()
        {
            var httpRequest = BuildRequest(BuildMethod<IOverrideClientRoute>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/action"),
                HttpMethod.Get);
        }

        [Path("api/")] private interface IClientRouteEndsWithSlash {[GetMethod("action")] int Method(); }

        [Test]
        public void Build_ClientRouteEndsWithSlash_ExtraSlashRemoved()
        {
            var httpRequest = BuildRequest(BuildMethod<IClientRouteEndsWithSlash>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/action"),
                HttpMethod.Get);
        }

        [Path("api")] private interface IMethodRouteEndsWithSlash {[GetMethod("action/")] int Method(); }

        [Test]
        public void Build_MethodRouteEndsWithSlash_ExtraSlashRemoved()
        {
            var httpRequest = BuildRequest(BuildMethod<IMethodRouteEndsWithSlash>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/action"),
                HttpMethod.Get);
        }

        [Path("api")] private interface IStaticRouteWithActionToken {[GetMethod("action/[action]")] int Method(); }

        [Test]
        public void Build_StaticRouteWithActionToken_StaticRouteWithMethodName()
        {
            var httpRequest = BuildRequest(BuildMethod<IStaticRouteWithActionToken>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/action/Method"),
                HttpMethod.Get);
        }

        [Path("[action]")] private interface IApiRouteWithActionToken {[GetMethod] int Method(); }

        [Test]
        public void Build_ApiRouteWithActionToken_RouteWithMethodName()
        {
            var httpRequest = BuildRequest(BuildMethod<IApiRouteWithActionToken>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/Method"),
                HttpMethod.Get);
        }

        private interface IMethodRouteWithControllerToken {[GetMethod("[controller]")] int Method(); }

        [Test]
        public void Build_MethodRouteWithControllerToken_RouteWithInterfaceName()
        {
            var httpRequest = BuildRequest(BuildMethod<IMethodRouteWithControllerToken>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/MethodRouteWithControllerToken"),
                HttpMethod.Get);
        }

        private interface IMethodRouteWithPrimitiveParamTokenWithoutAttribute {[GetMethod("{id}")] int Method([RouteParam] int id); }

        [Test]
        public void Build_MethodRouteWithPrimitiveParamTokenWithoutAttribute_RouteWithParamValue()
        {
            var httpRequest = BuildRequest(
                BuildMethod<IMethodRouteWithPrimitiveParamTokenWithoutAttribute>(),
                1);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/1"),
                HttpMethod.Get);
        }

        [Path("{id}")] private interface IApiRouteWithPrimitiveParamTokenWithoutAttribute {[GetMethod] int Method([RouteParam] int id); }

        [Test]
        public void Build_ApiRouteWithPrimitiveParamTokenWithoutAttribute_RouteWithParamValue()
        {
            var httpRequest = BuildRequest(
                BuildMethod<IApiRouteWithPrimitiveParamTokenWithoutAttribute>(),
                1);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/1"),
                HttpMethod.Get);
        }

        private interface IPrimitiveRouteParam {[GetMethod("{id}")] int Method([RouteParam] int id); }

        [Test]
        public void Build_PrimitiveRouteParam_RouteWithParamValue()
        {
            var httpRequest = BuildRequest(
                BuildMethod<IPrimitiveRouteParam>(),
                1);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/1"),
                HttpMethod.Get);
        }

        private interface IPrimitiveRouteParamWithoutTokenInRoute {[GetMethod] int Method([RouteParam] int id); }

        [Test]
        public void Build_PrimitiveRouteParamWithoutTokenInRoute_ThrowClientValidationException()
        {
            Func<HttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IPrimitiveRouteParamWithoutTokenInRoute>(),
                1);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.RouteParamWithoutTokenInRoute("id").Message);
        }

        private interface IMethodRouteWithCustomTypeParamToken {[GetMethod("{entity}")] int Method(BasicEntity entity); }

        [Test]
        public void Build_MethodRouteWithCustomTypeParamToken_ThrowClientValidationException()
        {
            Func<HttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IMethodRouteWithCustomTypeParamToken>(),
                new BasicEntity { Id = 1, Value = 2 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.TemplatePartContainsComplexType("entity").Message);
        }

        [Path("{entity}")] private interface IApiRouteWithCustomTypeParamToken {[GetMethod] int Method(BasicEntity entity); }

        [Test]
        public void Build_ApiRouteWithCustomTypeParamToken_ThrowClientValidationException()
        {
            Func<HttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IApiRouteWithCustomTypeParamToken>(),
                new BasicEntity { Id = 1, Value = 2 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.TemplatePartContainsComplexType("entity").Message);
        }

        private interface ICustomTypeRouteParam {[GetMethod("{id}")] int Method([RouteParam] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeRouteParam_ThrowClientValidationException()
        {
            Func<HttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<ICustomTypeRouteParam>(),
                new BasicEntity { Id = 1, Value = 2 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.RouteParamWithoutTokenInRoute("entity").Message);
        }

        private interface ICustomTypeRouteParamWithoutTokenInRoute {[GetMethod] int Method([RouteParam] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeRouteParamWithoutTokenInRoute_ThrowClientValidationException()
        {
            Func<HttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<ICustomTypeRouteParamWithoutTokenInRoute>(),
                new BasicEntity { Id = 1, Value = 2 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.RouteParamWithoutTokenInRoute("entity").Message);
        }

        [UseVersion("1.0"), Path("api/v{version:apiVersion}")] private interface IPathWithApiVersionToken {[GetMethod] int Method(); }

        [Test]
        public void Build_PathWithApiVersionToken_RouteWithApiVersion()
        {
            var httpRequest = BuildRequest(BuildMethod<IPathWithApiVersionToken>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/api/v1.0"),
                HttpMethod.Get);
        }
    }
}
