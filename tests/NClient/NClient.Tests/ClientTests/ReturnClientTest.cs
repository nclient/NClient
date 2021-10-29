using System.Threading.Tasks;
using FluentAssertions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class ReturnClientTest
    {
        private IReturnClientWithMetadata _returnClient = null!;
        private ReturnApiMockFactory _returnApiMockFactory = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _returnApiMockFactory = new ReturnApiMockFactory(PortsPool.Get());
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            PortsPool.Put(_returnApiMockFactory.ApiUri.Port);
        }
        
        [SetUp]
        public void Setup()
        {
            _returnClient = NClientGallery.Clients
                .GetBasic()
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

            var result = _returnClient.GetIHttpResponse(id);

            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(entity);
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
