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
    public class RouteBuilderCommonTemplateTest : RouteBuilderTestBase
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

        [Api("api")] public interface IStaticTemplate { [AsHttpGet] int Method(); }

        [Test]
        public void Build_StaticTemplate_TemplateWithoutChanges()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticTemplate>(KeepDataInterceptor)
                .Method();

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("api");
        }

        [Api("[controller]")] public interface IControllerToken { [AsHttpGet] int Method(); }

        [Test]
        public void Build_ControllerToken_InterfaceNameWithoutPrefix()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IControllerToken>(KeepDataInterceptor)
                .Method();

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("ControllerToken");
        }

        [Api("api/[controller]")] public interface IStaticPartWithControllerToken { [AsHttpGet] int Method(); }

        [Test]
        public void Build_StaticPartWithControllerToken_StaticPartWithInterfaceNameWithoutPrefix()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticPartWithControllerToken>(KeepDataInterceptor)
                .Method();

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("api/StaticPartWithControllerToken");
        }

        [Api("[action]")] public interface IActionToken { [AsHttpGet] int Method(); }

        [Test]
        public void Build_ActionToken_MethodName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IActionToken>(KeepDataInterceptor)
                .Method();

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("Method");
        }

        [Api("api/[action]")] public interface IStaticPartWithActionToken { [AsHttpGet] int Method(); }

        [Test]
        public void Build_StaticPartWithActionToken_StaticPartWithMethodName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticPartWithActionToken>(KeepDataInterceptor)
                .Method();

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("api/Method");
        }

        [Api("api/[controller]/[action]")] public interface IStaticPartWithControllerTokenWithActionToken { [AsHttpGet] int Method(); }

        [Test]
        public void Build_StaticPartWithControllerTokenWithActionToken_StaticPartWithInterfaceNameWithMethodName()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticPartWithControllerTokenWithActionToken>(KeepDataInterceptor)
                .Method();

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("api/StaticPartWithControllerTokenWithActionToken/Method");
        }

        [Api("{id}")] public interface IParameterToken { [AsHttpGet] int Method(int id); }

        [Test]
        public void Build_ParameterToken_MethodParameterValue()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IParameterToken>(KeepDataInterceptor)
                .Method(1);

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("1");
        }

        [Api("api/{id}")] public interface IStaticPartWithParameterToken { [AsHttpGet] int Method(int id); }

        [Test]
        public void Build_StaticPartWithParameterToken_IStaticPartWithMethodParameterValue()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IStaticPartWithParameterToken>(KeepDataInterceptor)
                .Method(1);

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("api/1");
        }

        [Api("{id:int}")] public interface IConstrainedParameterToken { [AsHttpGet] int Method(int id); }

        [Test]
        public void Build_ConstrainedParameterToken_MethodParameterValue()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IConstrainedParameterToken>(KeepDataInterceptor)
                .Method(1);

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("1");
        }

        [Api("{id:uint}")] public interface INotFitConstrainedParameterToken { [AsHttpGet] int Method(int id); }

        [Test]
        public void Build_NotFitConstrainedParameterToken_MethodParameterValue()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<INotFitConstrainedParameterToken>(KeepDataInterceptor)
                .Method(int.MaxValue);

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be($"{int.MaxValue}");
        }

        [Api("[controller1]")] public interface IWrongControllerToken { [AsHttpGet] int Method(); }

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

        [Api("[action1]")] public interface IWrongActionToken { [AsHttpGet] int Method(); }

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

        [Api("{id}/{id}")] public interface IDuplicateParameterTokens { [AsHttpGet] int Method(int id); }

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

        [Api("{prop}")] public interface INotExistsParameterToken { [AsHttpGet] int Method(int id); }

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

        [Api("{entity}")] public interface ICustomTypeParameterToken { [AsHttpGet] int Method(BasicEntity entity); }

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
