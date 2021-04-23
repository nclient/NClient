using System;
using System.Net.Http;
using NClient.Annotations.Methods;
using NClient.Testing.Common;
using NUnit.Framework;

namespace NClient.Standalone.Tests.RequestBuilderTests
{
    [Parallelizable]
    public class RequestBuilderMethodTest : RequestBuilderTestBase
    {
        private interface IGetMethod {[GetMethod] int Method(); }

        [Test]
        public void Build_GetMethod_GetHttpMethodRequest()
        {
            var httpRequest = BuildRequest(BuildMethod<IGetMethod>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get);
        }

        private interface IPostMethod {[PostMethod] int Method(); }

        [Test]
        public void Build_PostMethod_PostHttpMethodRequest()
        {
            var httpRequest = BuildRequest(BuildMethod<IPostMethod>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Post);
        }

        private interface IPutMethod {[PutMethod] int Method(); }

        [Test]
        public void Build_PutMethod_PutHttpMethodRequest()
        {
            var httpRequest = BuildRequest(BuildMethod<IPutMethod>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Put);
        }

        private interface IDeleteMethod {[DeleteMethod] int Method(); }

        [Test]
        public void Build_DeleteMethod_DeleteHttpMethodRequest()
        {
            var httpRequest = BuildRequest(BuildMethod<IDeleteMethod>());

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Delete);
        }
    }
}
