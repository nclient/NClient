using System.Linq;
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
        [Test]
        public async Task QueryClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = QueryApiMockFactory.MockGetMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IQueryClientWithMetadata>(api.Urls.First().ToUri()).Build()
                .GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task QueryClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = QueryApiMockFactory.MockPostMethod(entity);

            await NClientGallery.Clients.GetRest().For<IQueryClientWithMetadata>(api.Urls.First().ToUri()).Build()
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task QueryClient_PutAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = QueryApiMockFactory.MockPutMethod(entity);

            await NClientGallery.Clients.GetRest().For<IQueryClientWithMetadata>(api.Urls.First().ToUri()).Build()
                .Invoking(async x => await x.PutAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task QueryClient_DeleteAsync_NotThrow()
        {
            const int id = 1;
            using var api = QueryApiMockFactory.MockDeleteMethod(id);

            await NClientGallery.Clients.GetRest().For<IQueryClientWithMetadata>(api.Urls.First().ToUri()).Build()
                .Invoking(async x => await x.DeleteAsync(id))
                .Should()
                .NotThrowAsync();
        }
    }
}
