using System.Threading.Tasks;
using FluentAssertions;
using NClient.InterfaceProxy.Extensions;
using NClient.InterfaceProxy.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.InterfaceProxy.Tests.ClientTests
{
    [Parallelizable]
    public class QueryClientTest
    {
        private IQueryClientWithMetadata _queryClient = null!;
        private QueryApiMockFactory _queryApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _queryApiMockFactory = new QueryApiMockFactory();
            _queryClient = new ClientProvider()
                .Use<IQueryClientWithMetadata>(_queryApiMockFactory.ApiUri)
                .SetDefaultHttpClientProvider()
                .WithoutResiliencePolicy()
                .Build();
        }

        [Test]
        public async Task QueryClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = _queryApiMockFactory.MockGetMethod(id);

            var result = await _queryClient.GetAsync(id);
            result.Should().Be(1);
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
