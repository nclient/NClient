using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Exceptions;
using NClient.Providers.Host;
using NClient.Providers.Transport;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Providers.Api.Rest.Tests.RequestBuilderTests
{
    [Parallelizable]
    [SuppressMessage("ReSharper", "BadDeclarationBracesLineBreaks")]
    public class RequestBuilderRouteTest : RequestBuilderTestBase
    {
        [Path("api")] private interface IHostAndStaticRoute { [GetMethod] int Method(); }

        [Test]
        public async Task Build_HostAndStaticRoute_OnlyCommonStaticRoute()
        {
            var uri = new Uri("http://localhost:5000/");
            var hostMock = new Mock<IHost>();
            hostMock
                .Setup(x => x.TryGetUriAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Uri?>(uri));
            
            var httpRequest = BuildRequest(host: hostMock.Object, BuildMethod<IHostAndStaticRoute>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri(uri, "api"),
                RequestType.Read);
        }

        [Path("controller")] private interface IHostPathAndStaticRoute { [GetMethod] int Method(); }

        [Test]
        public async Task Build_HostPathAndStaticRoute_OnlyCommonStaticRoute()
        {
            var uri = new Uri("http://localhost:5000/");
            var hostMock = new Mock<IHost>();
            hostMock
                .Setup(x => x.TryGetUriAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Uri?>(new Uri(uri, "api")));
            
            var httpRequest = BuildRequest(host: hostMock.Object, BuildMethod<IHostPathAndStaticRoute>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri(uri, "api/controller"),
                RequestType.Read);
        }

        [Path("controller")] private interface IHostPathWithSlashAndStaticRoute { [GetMethod] int Method(); }

        [Test]
        public async Task Build_HostPathWithSlashAndStaticRoute_OnlyCommonStaticRoute()
        {
            var uri = new Uri("http://localhost:5000/");
            var hostMock = new Mock<IHost>();
            hostMock
                .Setup(x => x.TryGetUriAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Uri?>(new Uri(uri, "api")));
            
            var httpRequest = BuildRequest(host: hostMock.Object, BuildMethod<IHostPathWithSlashAndStaticRoute>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri(uri, "api/controller"),
                RequestType.Read);
        }

        [Path("api")] private interface ICommonStaticRoute { [GetMethod] int Method(); }

        [Test]
        public async Task Build_CommonStaticRoute_OnlyCommonStaticRoute()
        {
            var httpRequest = BuildRequest(BuildMethod<ICommonStaticRoute>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/api"),
                RequestType.Read);
        }

        [Path("api/[controller]")] private interface ICommonStaticRouteWithControllerToken { [GetMethod] int Method(); }

        [Test]
        public async Task Build_CommonStaticRouteWithControllerToken_StaticRouteWithInterfaceName()
        {
            var httpRequest = BuildRequest(BuildMethod<ICommonStaticRouteWithControllerToken>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/api/CommonStaticRouteWithControllerToken"),
                RequestType.Read);
        }

        [Path("api/[controller]")] private interface IStaticRouteWithControllerToken { [GetMethod("entity")] int Method(); }

        [Test]
        public async Task Build_StaticRouteWithControllerToken_StaticRouteWithInterfaceName()
        {
            var httpRequest = BuildRequest(BuildMethod<IStaticRouteWithControllerToken>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/api/StaticRouteWithControllerToken/entity"),
                RequestType.Read);
        }

        [Path("api/[controller]")] private interface IStaticRouteWithControllerAndActionTokens { [GetMethod("[action]")] int Method(); }

        [Test]
        public async Task Build_StaticRouteWithControllerAndActionTokens_StaticRouteWithInterfaceAndMethodNames()
        {
            var httpRequest = BuildRequest(BuildMethod<IStaticRouteWithControllerAndActionTokens>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/api/StaticRouteWithControllerAndActionTokens/Method"),
                RequestType.Read);
        }

        [Path("api")] private interface IStaticRoute { [GetMethod("action")] int Method(); }

        [Test]
        public async Task Build_StaticRoute_StaticRoute()
        {
            var httpRequest = BuildRequest(BuildMethod<IStaticRoute>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/api/action"),
                RequestType.Read);
        }

        [Path("/api")] private interface IClientWithRootedRoute { [GetMethod("action")] int Method(); }

        [Test]
        public async Task Build_ClientWithRootedRoute_ExtraSlashRemoved()
        {
            var httpRequest = BuildRequest(BuildMethod<IClientWithRootedRoute>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/api/action"),
                RequestType.Read);
        }

        [Path("api")] private interface IOverrideClientRoute { [GetMethod("/action")] int Method(); }

        [Test]
        public async Task Build_OverrideClientRoute_IgnoreClientRoute()
        {
            var httpRequest = BuildRequest(BuildMethod<IOverrideClientRoute>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/action"),
                RequestType.Read);
        }

        [Path("api/")] private interface IClientRouteEndsWithSlash { [GetMethod("action")] int Method(); }

        [Test]
        public async Task Build_ClientRouteEndsWithSlash_ExtraSlashRemoved()
        {
            var httpRequest = BuildRequest(BuildMethod<IClientRouteEndsWithSlash>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/api/action"),
                RequestType.Read);
        }

        [Path("api")] private interface IMethodRouteEndsWithSlash { [GetMethod("action/")] int Method(); }

        [Test]
        public async Task Build_MethodRouteEndsWithSlash_ExtraSlashRemoved()
        {
            var httpRequest = BuildRequest(BuildMethod<IMethodRouteEndsWithSlash>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/api/action"),
                RequestType.Read);
        }

        [Path("api")] private interface IStaticRouteWithActionToken { [GetMethod("action/[action]")] int Method(); }

        [Test]
        public async Task Build_StaticRouteWithActionToken_StaticRouteWithMethodName()
        {
            var httpRequest = BuildRequest(BuildMethod<IStaticRouteWithActionToken>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/api/action/Method"),
                RequestType.Read);
        }

        [Path("[action]")] private interface IApiRouteWithActionToken { [GetMethod] int Method(); }

        [Test]
        public async Task Build_ApiRouteWithActionToken_RouteWithMethodName()
        {
            var httpRequest = BuildRequest(BuildMethod<IApiRouteWithActionToken>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/Method"),
                RequestType.Read);
        }

        private interface IMethodRouteWithControllerToken { [GetMethod("[controller]")] int Method(); }

        [Test]
        public async Task Build_MethodRouteWithControllerToken_RouteWithInterfaceName()
        {
            var httpRequest = BuildRequest(BuildMethod<IMethodRouteWithControllerToken>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/MethodRouteWithControllerToken"),
                RequestType.Read);
        }

        private interface IMethodRouteWithPrimitiveParamTokenWithoutAttribute { [GetMethod("{id}")] int Method([RouteParam] int id); }

        [Test]
        public async Task Build_MethodRouteWithPrimitiveParamTokenWithoutAttribute_RouteWithParamValue()
        {
            var httpRequest = BuildRequest(
                BuildMethod<IMethodRouteWithPrimitiveParamTokenWithoutAttribute>(),
                1);

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/1"),
                RequestType.Read);
        }

        [Path("{id}")] private interface IApiRouteWithPrimitiveParamTokenWithoutAttribute { [GetMethod] int Method([RouteParam] int id); }

        [Test]
        public async Task Build_ApiRouteWithPrimitiveParamTokenWithoutAttribute_RouteWithParamValue()
        {
            var httpRequest = BuildRequest(
                BuildMethod<IApiRouteWithPrimitiveParamTokenWithoutAttribute>(),
                1);

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/1"),
                RequestType.Read);
        }

        private interface IPrimitiveRouteParam { [GetMethod("{id}")] int Method([RouteParam] int id); }

        [Test]
        public async Task Build_PrimitiveRouteParam_RouteWithParamValue()
        {
            var httpRequest = BuildRequest(
                BuildMethod<IPrimitiveRouteParam>(),
                1);

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/1"),
                RequestType.Read);
        }

        private interface IPrimitiveRouteParamWithoutTokenInRoute { [GetMethod] int Method([RouteParam] int id); }

        [Test]
        public void Build_PrimitiveRouteParamWithoutTokenInRoute_ThrowClientValidationException()
        {
            Func<IRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IPrimitiveRouteParamWithoutTokenInRoute>(),
                1);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(RestClientValidationExceptionFactory.RouteParamWithoutTokenInRoute("id").Message);
        }

        private interface IMethodRouteWithCustomTypeParamToken { [GetMethod("{entity}")] int Method(BasicEntity entity); }

        [Test]
        public void Build_MethodRouteWithCustomTypeParamToken_ThrowClientValidationException()
        {
            Func<IRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IMethodRouteWithCustomTypeParamToken>(),
                new BasicEntity { Id = 1, Value = 2 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(RestClientValidationExceptionFactory.TemplatePartContainsComplexType("entity").Message);
        }

        [Path("{entity}")] private interface IApiRouteWithCustomTypeParamToken { [GetMethod] int Method(BasicEntity entity); }

        [Test]
        public void Build_ApiRouteWithCustomTypeParamToken_ThrowClientValidationException()
        {
            Func<IRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IApiRouteWithCustomTypeParamToken>(),
                new BasicEntity { Id = 1, Value = 2 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(RestClientValidationExceptionFactory.TemplatePartContainsComplexType("entity").Message);
        }

        private interface ICustomTypeRouteParam { [GetMethod("{id}")] int Method([RouteParam] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeRouteParam_ThrowClientValidationException()
        {
            Func<IRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<ICustomTypeRouteParam>(),
                new BasicEntity { Id = 1, Value = 2 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(RestClientValidationExceptionFactory.RouteParamWithoutTokenInRoute("entity").Message);
        }

        private interface ICustomTypeRouteParamWithoutTokenInRoute { [GetMethod] int Method([RouteParam] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeRouteParamWithoutTokenInRoute_ThrowClientValidationException()
        {
            Func<IRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<ICustomTypeRouteParamWithoutTokenInRoute>(),
                new BasicEntity { Id = 1, Value = 2 });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(RestClientValidationExceptionFactory.RouteParamWithoutTokenInRoute("entity").Message);
        }

        [UseVersion("1.0"), Path("api/v{version:apiVersion}")] private interface IPathWithApiVersionToken { [GetMethod] int Method(); }

        [Test]
        public async Task Build_PathWithApiVersionToken_RouteWithApiVersion()
        {
            var httpRequest = BuildRequest(BuildMethod<IPathWithApiVersionToken>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/api/v1.0"),
                RequestType.Read);
        }
    }
}
