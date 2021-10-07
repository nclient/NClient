using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class HttpClientTest
    {
        private IHttpClientWithMetadata _httpClient = null!;
        private HttpApiMockFactory _httpApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _httpApiMockFactory = new HttpApiMockFactory(port: 5016);

            _httpClient = NClientGallery.NativeClients
                .GetBasic()
                .For<IHttpClientWithMetadata>(_httpApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task HttpClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = _httpApiMockFactory.MockGetMethod(id);

            var result = await _httpClient.GetAsync(id);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(id);
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task HttpClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _httpApiMockFactory.MockPostMethod(entity);

            var result = await _httpClient.PostAsync(entity);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.Value.Should().BeEquivalentTo(entity);
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task HttpClient_PutAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _httpApiMockFactory.MockPutMethod(entity);

            var result = await _httpClient.PutAsync(entity);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void HttpClient_Delete_NotThrow()
        {
            const int id = 1;
            using var api = _httpApiMockFactory.MockDeleteMethod(id);

            var result = _httpClient.Delete(id);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
