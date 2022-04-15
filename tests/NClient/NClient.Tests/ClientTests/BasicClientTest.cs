using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NClient.Tests.ClientTests.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class BasicClientTest : ClientTestBase<IBasicClientWithMetadata>
    {
        [Test]
        public async Task BasicClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(host: api.Urls.First()).Build()
                .GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task BasicClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = BasicApiMockFactory.MockPostMethod(entity);

            await NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(host: api.Urls.First()).Build()
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task BasicClient_PutAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = BasicApiMockFactory.MockPutMethod(entity);

            await NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(host: api.Urls.First()).Build()
                .Invoking(async x => await x.PutAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task BasicClient_DeleteAsync_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockDeleteMethod(id);

            await NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(host: api.Urls.First()).Build()
                .Invoking(async x => await x.DeleteAsync(id))
                .Should()
                .NotThrowAsync();
        }
    }
}
