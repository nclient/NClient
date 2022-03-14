using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NClient.Tests.ClientTests.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class HttpResponseClientTest : ClientTestBase<IHttpResponseClientWithMetadata>
    {
        [Test]
        public async Task HttpClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = HttpResponseApiMockFactory.MockGetMethod(id);

            var httpResponse = await NClientGallery.Clients.GetRest().For<IHttpResponseClientWithMetadata>(host: api.Urls.First()).Build()
                .GetAsync(id);

            httpResponse.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            httpResponse.Data.Should().Be(id);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task HttpClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = HttpResponseApiMockFactory.MockPostMethod(entity);

            var httpResponse = await NClientGallery.Clients.GetRest().For<IHttpResponseClientWithMetadata>(host: api.Urls.First()).Build()
                .PostAsync(entity);

            httpResponse.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            httpResponse.Data.Should().BeEquivalentTo(entity);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task HttpClient_PutAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = HttpResponseApiMockFactory.MockPutMethod(entity);

            var httpResponse = await NClientGallery.Clients.GetRest().For<IHttpResponseClientWithMetadata>(host: api.Urls.First()).Build()
                .PutAsync(entity);

            httpResponse.Should().NotBeNull();
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void HttpClient_Delete_NotThrow()
        {
            const int id = 1;
            using var api = HttpResponseApiMockFactory.MockDeleteMethod(id);

            var httpResponse = NClientGallery.Clients.GetRest().For<IHttpResponseClientWithMetadata>(host: api.Urls.First()).Build()
                .Delete(id);

            httpResponse.Should().NotBeNull();
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
