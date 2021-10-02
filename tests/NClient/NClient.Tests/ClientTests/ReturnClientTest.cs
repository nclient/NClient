using System.Threading.Tasks;
using FluentAssertions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Tests.Clients;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class ReturnClientTest
    {
        private IReturnClientWithMetadata _returnClient = null!;
        private ReturnApiMockFactory _returnApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _returnApiMockFactory = new ReturnApiMockFactory(port: 5011);
            _returnClient = new NClientBuilder()
                .For<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .Build();
        }
        
        [Test]
        public void ResultClient_Get_BasicEntity()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetMethod(id, entity);

            var result = _returnClient.Get(id);

            result.Should().BeEquivalentTo(entity);
        }

        [Test]
        public async Task ResultClient_GetAsync_TaskWithBasicEntity()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = await _returnClient.GetAsync(id);

            result.Should().BeEquivalentTo(entity);
        }

        [Test]
        public void ResultClient_GetHttpResponse_HttpResponseWithBasicEntity()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = _returnClient.GetHttpResponse(id);

            result.IsSuccessful.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(entity);
        }
        
        [Test]
        public void ResultClient_Post_Void()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostMethod(entity);

            _returnClient.Post(entity);
        }

        [Test]
        public async Task ResultClient_PostAsync_Task()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostAsyncMethod(entity);

            await _returnClient.PostAsync(entity);
        }

        [Test]
        public void ResultClient_PostHttpResponse_HttpResponseWithoutValue()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostMethod(entity);

            var result = _returnClient.PostHttpResponse(entity);

            result.IsSuccessful.Should().BeTrue();
        }
    }
}
