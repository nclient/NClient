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
    public class RequestBuilderMethodTest : RequestBuilderTestBase
    {
        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            AttributeHelper = new AttributeHelper();
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        [Api] public interface IGetMethod { [AsHttpGet] int Method(); }

        [Test]
        public void Build_GetMethod_GetHttpMethodRequest()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IGetMethod>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get);
        }

        [Api] public interface IPostMethod { [AsHttpPost] int Method(); }

        [Test]
        public void Build_PostMethod_PostHttpMethodRequest()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPostMethod>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Post);
        }

        [Api] public interface IPutMethod { [AsHttpPut] int Method(); }

        [Test]
        public void Build_PutMethod_PutHttpMethodRequest()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPutMethod>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Put);
        }

        [Api] public interface IDeleteMethod { [AsHttpDelete] int Method(); }

        [Test]
        public void Build_DeleteMethod_DeleteHttpMethodRequest()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IDeleteMethod>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Delete);
        }
    }
}
