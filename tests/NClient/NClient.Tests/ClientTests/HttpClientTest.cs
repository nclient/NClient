using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class HttpClientTest
    {
        [Test]
        public async Task HttpClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = HttpApiMockFactory.MockGetMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IHttpClientWithMetadata>(api.Urls.First().ToUri()).Build()
                .GetAsync(id);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.Data.Should().Be(id);
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);
        }

        [Test]
        public async Task HttpClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = HttpApiMockFactory.MockPostMethod(entity);

            var result = await NClientGallery.Clients.GetRest().For<IHttpClientWithMetadata>(api.Urls.First().ToUri()).Build()
                .PostAsync(entity);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.Data.Should().BeEquivalentTo(entity);
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);
        }

        [Test]
        public async Task HttpClient_PutAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = HttpApiMockFactory.MockPutMethod(entity);

            var result = await NClientGallery.Clients.GetRest().For<IHttpClientWithMetadata>(api.Urls.First().ToUri()).Build()
                .PutAsync(entity);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);
        }

        [Test]
        public void HttpClient_Delete_NotThrow()
        {
            const int id = 1;
            using var api = HttpApiMockFactory.MockDeleteMethod(id);

            var result = NClientGallery.Clients.GetRest().For<IHttpClientWithMetadata>(api.Urls.First().ToUri()).Build()
                .Delete(id);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);
        }
    }
}
