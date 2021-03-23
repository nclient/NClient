using System;
using System.Net.Http;
using FluentAssertions;
using NClient.Core.Attributes;
using NClient.Core.Attributes.Clients;
using NClient.Core.Attributes.Clients.Methods;
using NClient.Core.Attributes.Clients.Parameters;
using NClient.Core.Attributes.Services;
using NClient.Core.Exceptions;
using NClient.Core.Interceptors;
using NClient.Providers.HttpClient;
using NClient.Testing.Common;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.InterfaceProxy.Standalone.Tests.RequestBuilderTests
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
        [Service("[controller]")] public interface IInheritanceController { [AsHttpGet] int Get([ToBody] BasicEntity entity); }

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
        [Service("[controller]")] public interface IDeepInheritanceController {[AsHttpGet] int Get([ToBody] BasicEntity entity); }

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

        [Client("OverrideClient")] public interface IOverrideClient : IOverrideController { }
        [Service("[controller]")] public interface IOverrideController {[AsHttpGet] int Get([ToBody] BasicEntity entity); }

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

        public interface IOverrideMethodClient : IOverrideMethodController { [AsHttpGet] new int Get([ToBody] BasicEntity entity); }
        [Service("[controller]")] public interface IOverrideMethodController { [AsHttpPost] int Get([ToBody] BasicEntity entity); }

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

        public interface IOverrideParamClient : IOverrideParamController { [AsHttpGet] new int Get([ToBody] BasicEntity entity); }
        [Service("[controller]")] public interface IOverrideParamController { [AsHttpGet] int Get([ToQuery] BasicEntity entity); }

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
