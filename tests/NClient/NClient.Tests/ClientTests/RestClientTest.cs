using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class RestClientTest
    {
        [Test]
        public async Task RestClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = RestApiMockFactory.MockIntGetMethod(id);

            var result = await NClientGallery.Clients.GetBasic().For<IRestClientWithMetadata>(api.Urls.First()).Build()
                .GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task RestClient_GetAsync_StringInBody()
        {
            const string id = "1";
            using var api = RestApiMockFactory.MockStringGetMethod(id);

            var result = await NClientGallery.Clients.GetBasic().For<IRestClientWithMetadata>(api.Urls.First()).Build()
                .GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task RestClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = RestApiMockFactory.MockPostMethod(entity);

            await NClientGallery.Clients.GetBasic().For<IRestClientWithMetadata>(api.Urls.First()).Build()
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task RestClient_PutAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = RestApiMockFactory.MockPutMethod(entity);

            await NClientGallery.Clients.GetBasic().For<IRestClientWithMetadata>(api.Urls.First()).Build()
                .Invoking(async x => await x.PutAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task RestClient_DeleteAsync_NotThrow()
        {
            const int id = 1;
            using var api = RestApiMockFactory.MockDeleteMethod(id);

            await NClientGallery.Clients.GetBasic().For<IRestClientWithMetadata>(api.Urls.First()).Build()
                .Invoking(async x => await x.DeleteAsync(id))
                .Should()
                .NotThrowAsync();
        }
    }
}
