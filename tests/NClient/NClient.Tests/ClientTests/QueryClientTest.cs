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
    public class QueryClientTest
    {
        private IQueryClientWithMetadata _queryClient = null!;
        private QueryApiMockFactory _queryApiMockFactory = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _queryApiMockFactory = new QueryApiMockFactory(PortsPool.Get());
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            PortsPool.Put(_queryApiMockFactory.ApiUri.Port);
        }
        
        [SetUp]
        public void Setup()
        {
            _queryClient = NClientGallery.Clients
                .GetBasic()
                .For<IQueryClientWithMetadata>(_queryApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task QueryClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = _queryApiMockFactory.MockGetMethod(id);

            var result = await _queryClient.GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task QueryClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _queryApiMockFactory.MockPostMethod(entity);

            await _queryClient
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task QueryClient_PutAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _queryApiMockFactory.MockPutMethod(entity);

            await _queryClient
                .Invoking(async x => await x.PutAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task QueryClient_DeleteAsync_NotThrow()
        {
            const int id = 1;
            using var api = _queryApiMockFactory.MockDeleteMethod(id);

            await _queryClient
                .Invoking(async x => await x.DeleteAsync(id))
                .Should()
                .NotThrowAsync();
        }
    }
}
