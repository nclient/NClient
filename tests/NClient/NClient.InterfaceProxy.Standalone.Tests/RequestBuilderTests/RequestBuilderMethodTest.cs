using System;
using System.Net.Http;
using NClient.Core.Attributes;
using NClient.Core.Attributes.Methods;
using NClient.Core.Interceptors;
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
            AttributeMapper = new AttributeMapper();
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        public interface IGetMethod { [GetMethod] int Method(); }

        [Test]
        public void Build_GetMethod_GetHttpMethodRequest()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IGetMethod>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get);
        }

        public interface IPostMethod { [PostMethod] int Method(); }

        [Test]
        public void Build_PostMethod_PostHttpMethodRequest()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPostMethod>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Post);
        }

        public interface IPutMethod { [PutMethod] int Method(); }

        [Test]
        public void Build_PutMethod_PutHttpMethodRequest()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPutMethod>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Put);
        }

        public interface IDeleteMethod { [DeleteMethod] int Method(); }

        [Test]
        public void Build_DeleteMethod_DeleteHttpMethodRequest()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IDeleteMethod>(KeepDataInterceptor)
                .Method();

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Delete);
        }
    }
}
