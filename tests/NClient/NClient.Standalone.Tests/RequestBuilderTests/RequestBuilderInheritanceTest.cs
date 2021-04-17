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
    public class RequestBuilderInheritanceTest : RequestBuilderTestBase
    {
        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            AttributeMapper = new AttributeMapper();
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        public interface IInheritanceClient : IInheritanceController { }
        [Path("[controller]")] public interface IInheritanceController {[GetMethod] int Get([BodyParam] BasicEntity entity); }

        [Test]
        public void Build_ClientInterfaceInheritControllerInterface_UsedControllerAttributes()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IInheritanceClient>(KeepDataInterceptor)
                .Get(new BasicEntity { Id = 1 });

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/Inheritance"),
                HttpMethod.Get,
                body: new BasicEntity { Id = 1 });
        }

        public interface IDeepInheritanceClient : IDeepInheritanceClientBase { }
        public interface IDeepInheritanceClientBase : IDeepInheritanceController { }
        [Path("[controller]")] public interface IDeepInheritanceController {[GetMethod] int Get([BodyParam] BasicEntity entity); }

        [Test]
        public void Build_DeepInheritanceControllerInterface_UsedControllerAttributes()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IDeepInheritanceClient>(KeepDataInterceptor)
                .Get(new BasicEntity { Id = 1 });

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/DeepInheritance"),
                HttpMethod.Get,
                body: new BasicEntity { Id = 1 });
        }

        [Path("OverrideClient")] public interface IOverrideClient : IOverrideController { }
        [Path("[controller]")] public interface IOverrideController {[GetMethod] int Get([BodyParam] BasicEntity entity); }

        [Test]
        public void Build_ClientOverrideControllerAttribute_ThrowNotSupportedNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IOverrideClient>(KeepDataInterceptor)
                .Get(new BasicEntity { Id = 1 });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NotSupportedNClientException>();
        }

        public interface IOverrideMethodClient : IOverrideMethodController {[GetMethod] new int Get([BodyParam] BasicEntity entity); }
        [Path("[controller]")] public interface IOverrideMethodController {[PostMethod] int Get([BodyParam] BasicEntity entity); }

        [Test]
        public void Build_ClientOverrideControllerMethodAttribute_UsedOverridenMethod()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IOverrideMethodClient>(KeepDataInterceptor)
                .Get(new BasicEntity { Id = 1 });

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/OverrideMethod"),
                HttpMethod.Get,
                body: new BasicEntity { Id = 1 });
        }

        public interface IOverrideParamClient : IOverrideParamController {[GetMethod] new int Get([BodyParam] BasicEntity entity); }
        [Path("[controller]")] public interface IOverrideParamController {[GetMethod] int Get([QueryParam] BasicEntity entity); }

        [Test]
        public void Build_ClientOverrideControllerParamAttribute_UsedOverridenParam()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IOverrideParamClient>(KeepDataInterceptor)
                .Get(new BasicEntity { Id = 1 });

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/OverrideParam"),
                HttpMethod.Get,
                body: new BasicEntity { Id = 1 });
        }
    }
}
