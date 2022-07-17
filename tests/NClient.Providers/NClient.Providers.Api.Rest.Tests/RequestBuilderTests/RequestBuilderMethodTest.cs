using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NClient.Annotations.Http;
using NClient.Providers.Transport;
using NUnit.Framework;

namespace NClient.Providers.Api.Rest.Tests.RequestBuilderTests
{
    [Parallelizable]
    [SuppressMessage("ReSharper", "BadDeclarationBracesLineBreaks")]
    public class RequestBuilderMethodTest : RequestBuilderTestBase
    {
        private interface IGetMethod { [GetMethod] int Method(); }

        [Test]
        public async Task Build_GetMethod_GetHttpMethodRequest()
        {
            var httpRequest = BuildRequest(BuildMethod<IGetMethod>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/"),
                RequestType.Read);
        }

        private interface IHeadMethod {[HeadMethod] int Method(); }

        [Test]
        public async Task Build_HeadMethod_HeadHttpMethodRequest()
        {
            var httpRequest = BuildRequest(BuildMethod<IHeadMethod>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/"),
                RequestType.Check);
        }

        private interface IPostMethod {[PostMethod] int Method(); }

        [Test]
        public async Task Build_PostMethod_PostHttpMethodRequest()
        {
            var httpRequest = BuildRequest(BuildMethod<IPostMethod>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/"),
                RequestType.Create);
        }

        private interface IPutMethod {[PutMethod] int Method(); }

        [Test]
        public async Task Build_PutMethod_PutHttpMethodRequest()
        {
            var httpRequest = BuildRequest(BuildMethod<IPutMethod>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/"),
                RequestType.Update);
        }

        private interface IDeleteMethod {[DeleteMethod] int Method(); }

        [Test]
        public async Task Build_DeleteMethod_DeleteHttpMethodRequest()
        {
            var httpRequest = BuildRequest(BuildMethod<IDeleteMethod>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/"),
                RequestType.Delete);
        }

        private interface IOptionsMethod {[OptionsMethod] int Method(); }

        [Test]
        public async Task Build_OptionsMethod_OptionsHttpMethodRequest()
        {
            var httpRequest = BuildRequest(BuildMethod<IOptionsMethod>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/"),
                RequestType.Info);
        }

        #if !NETFRAMEWORK
        private interface IPatchMethod {[PatchMethod] int Method(); }

        [Test]
        public async Task Build_PatchMethod_PatchHttpMethodRequest()
        {
            var httpRequest = BuildRequest(BuildMethod<IPatchMethod>());

            await AssertHttpRequestAsync(httpRequest,
                new Uri("http://localhost:5000/"),
                RequestType.PartialUpdate);
        }
        #endif
    }
}
