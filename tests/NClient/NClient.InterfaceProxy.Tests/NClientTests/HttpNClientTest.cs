using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Core.Extensions;
using NClient.InterfaceProxy.Extensions;
using NClient.InterfaceProxy.Tests.Clients;
using NClient.Providers.HttpClient;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.InterfaceProxy.Tests.NClientTests
{
    [Parallelizable]
    public class HttpNClientTest
    {
        private IReturnClientWithMetadata _returnClient = null!;
        private ReturnApiMockFactory _returnApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _returnApiMockFactory = new ReturnApiMockFactory("http://localhost:5007/");
            _returnClient = new ClientProvider()
                .Use<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri)
                .SetDefaultHttpClientProvider()
                .WithoutResiliencePolicy()
                .Build();
        }

        [Test]
        public async Task GetHttpResponse_GetAsync_HttpResponseWithValue()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = await _returnClient.AsHttp().GetHttpResponse(client => client.GetAsync(id));
            result.Should().BeEquivalentTo(new HttpResponse<BasicEntity>(HttpStatusCode.OK, entity)
            {
                Content = "{\"Id\":1,\"Value\":2}"
            });
        }

        [Test]
        public void GetHttpResponse_Get_HttpResponseWithValue()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetMethod(id, entity);

            var result = _returnClient.AsHttp().GetHttpResponse(client => client.Get(id));
            result.Should().BeEquivalentTo(new HttpResponse<BasicEntity>(HttpStatusCode.OK, entity)
            {
                Content = "{\"Id\":1,\"Value\":2}"
            });
        }

        [Test]
        public async Task GetHttpResponse_PostAsync_HttpResponseWithoutValue()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostAsyncMethod(entity);

            var httpResponse = await _returnClient.AsHttp().GetHttpResponse(client => client.PostAsync(entity));
            httpResponse.Should().BeEquivalentTo(new HttpResponse(HttpStatusCode.OK));
        }

        [Test]
        public void GetHttpResponse_Post_HttpResponseWithoutValue()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostMethod(entity);

            var httpResponse = _returnClient.AsHttp().GetHttpResponse(client => client.Post(entity));
            httpResponse.Should().BeEquivalentTo(new HttpResponse(HttpStatusCode.OK));
        }

        [Test]
        public void GetHttpResponse_ServiceNotExists_InternalServerError()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };

            var httpResponse = _returnClient.AsHttp().GetHttpResponse(client => client.Post(entity));
            httpResponse.StatusCode.Should().Be((HttpStatusCode)0);
        }
    }
}
