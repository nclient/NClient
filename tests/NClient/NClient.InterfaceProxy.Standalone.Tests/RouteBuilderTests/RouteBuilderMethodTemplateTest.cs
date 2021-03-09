using System;
using FluentAssertions;
using NClient.Core.Exceptions;
using NClient.InterfaceProxy.Attributes;
using NClient.InterfaceProxy.Attributes.Methods;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.InterfaceProxy.Standalone.Tests.RouteBuilderTests
{
    [Parallelizable]
    public class RouteBuilderMethodTemplateTest : RouteBuilderTestBase
    {
        [Api] public interface IWithoutTemplate { [AsHttpGet] int Method(); }

        [Test]
        public void Build_WithoutTemplate_EmptyString()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IWithoutTemplate>(KeepDataInterceptor)
                .Method();

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().BeEmpty();
        }

        [Api] public interface IStaticTemplate { [AsHttpGet("api")] int Method(); }

        [Test]
        public void Build_StaticTemplate_TemplateWithoutChanges()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticTemplate>(KeepDataInterceptor)
                .Method();

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("api");
        }

        [Api] public interface IControllerToken { [AsHttpGet("[controller]")] int Method(); }

        [Test]
        public void Build_ControllerToken_InterfaceNameWithoutPrefix()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IControllerToken>(KeepDataInterceptor)
                .Method();

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("ControllerToken");
        }

        [Api] public interface IStaticPartWithControllerToken { [AsHttpGet("api/[controller]")] int Method(); }

        [Test]
        public void Build_StaticPartWithControllerToken_StaticPartWithInterfaceNameWithoutPrefix()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticPartWithControllerToken>(KeepDataInterceptor)
                .Method();

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("api/StaticPartWithControllerToken");
        }

        [Api] public interface IActionToken { [AsHttpGet("[action]")] int Method(); }

        [Test]
        public void Build_ActionToken_MethodName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IActionToken>(KeepDataInterceptor)
                .Method();

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("Method");
        }

        [Api] public interface IStaticPartWithActionToken { [AsHttpGet("api/[action]")] int Method(); }

        [Test]
        public void Build_StaticPartWithActionToken_StaticPartWithMethodName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticPartWithActionToken>(KeepDataInterceptor)
                .Method();

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("api/Method");
        }

        [Api] public interface IStaticPartWithControllerTokenWithActionToken { [AsHttpGet("api/[controller]/[action]")] int Method(); }

        [Test]
        public void Build_StaticPartWithControllerTokenWithActionToken_StaticPartWithInterfaceNameWithMethodName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticPartWithControllerTokenWithActionToken>(KeepDataInterceptor)
                .Method();

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("api/StaticPartWithControllerTokenWithActionToken/Method");
        }

        [Api] public interface IParameterToken { [AsHttpGet("{id}")] int Method(int id); }

        [Test]
        public void Build_ParameterToken_MethodParameterValue()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IParameterToken>(KeepDataInterceptor)
                .Method(1);

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("1");
        }

        [Api] public interface IStaticPartWithParameterToken { [AsHttpGet("api/{id}")] int Method(int id); }

        [Test]
        public void Build_StaticPartWithParameterToken_IStaticPartWithMethodParameterValue()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticPartWithParameterToken>(KeepDataInterceptor)
                .Method(1);

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("api/1");
        }

        [Api] public interface IConstrainedParameterToken { [AsHttpGet("{id:int}")] int Method(int id); }

        [Test]
        public void Build_ConstrainedParameterToken_MethodParameterValue()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IConstrainedParameterToken>(KeepDataInterceptor)
                .Method(1);

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("1");
        }

        [Api] public interface INotFitConstrainedParameterToken { [AsHttpGet("{id:uint}")] int Method(int id); }

        [Test]
        public void Build_NotFitConstrainedParameterToken_MethodParameterValue()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<INotFitConstrainedParameterToken>(KeepDataInterceptor)
                .Method(int.MaxValue);

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be($"{int.MaxValue}");
        }

        [Api] public interface IWrongControllerToken { [AsHttpGet("[controller1]")] int Method(); }

        [Test]
        public void Build_WrongControllerToken_ThrowInvalidRouteNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IWrongControllerToken>(KeepDataInterceptor)
                .Method();

            Func<string> buildRouteFunc = () => BuildRoute(KeepDataInterceptor);

            buildRouteFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        [Api] public interface IWrongActionToken { [AsHttpGet("[action1]")] int Method(); }

        [Test]
        public void Build_WrongActionToken_ThrowInvalidRouteNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IWrongActionToken>(KeepDataInterceptor)
                .Method();

            Func<string> buildRouteFunc = () => BuildRoute(KeepDataInterceptor);

            buildRouteFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        [Api] public interface IDuplicateParameterTokens { [AsHttpGet("{id}/{id}")] int Method(int id); }

        [Test]
        public void Build_DuplicateParameterTokens_ThrowInvalidRouteNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IDuplicateParameterTokens>(KeepDataInterceptor)
                .Method(1);

            Func<string> buildRouteFunc = () => BuildRoute(KeepDataInterceptor);

            buildRouteFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        [Api] public interface INotExistsParameterToken { [AsHttpGet("{prop}")] int Method(int id); }

        [Test]
        public void Build_NotExistsParameterToken_ThrowInvalidRouteNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<INotExistsParameterToken>(KeepDataInterceptor)
                .Method(1);

            Func<string> buildRouteFunc = () => BuildRoute(KeepDataInterceptor);

            buildRouteFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        [Api] public interface ICustomTypeParameterToken { [AsHttpGet("{entity}")] int Method(BasicEntity entity); }

        [Test]
        public void Build_CustomTypeParameterToken_ThrowInvalidRouteNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeParameterToken>(KeepDataInterceptor)
                .Method(new BasicEntity());

            Func<string> buildRouteFunc = () => BuildRoute(KeepDataInterceptor);

            buildRouteFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<InvalidRouteNClientException>();
        }
    }
}
