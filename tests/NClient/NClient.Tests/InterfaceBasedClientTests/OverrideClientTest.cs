using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Tests.Clients;
using NUnit.Framework;

namespace NClient.Tests.InterfaceBasedClientTests
{
    [Parallelizable]
    public class OverrideClientTest
    {
        private IOverriddenClientWithMetadata _overriddenClient = null!;
        private OverriddenApiMockFactory _overriddenApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _overriddenApiMockFactory = new OverriddenApiMockFactory(port: 5020);

            _overriddenClient = new NClientBuilder()
                .Use<IOverriddenClientWithMetadata>(_overriddenApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task OverriddenClient_GetAsync_SendsGetRequestAndReceivesHttpResponseWithIntContent()
        {
            const int id = 1;
            using var api = _overriddenApiMockFactory.MockGetMethod(id);

            var result = await _overriddenClient.GetAsync(id);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Value.Should().Be(1);
        }

        [Test]
        public async Task OverriddenClient_PostAsync_SendsPutRequestAndReceivesHttpResponse()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _overriddenApiMockFactory.MockPostMethod(entity);

            var result = await _overriddenClient.PostAsync(entity);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task OverriddenClient_PutAsync_SendsPutWithTestHeaderAndNotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _overriddenApiMockFactory.MockPutMethod(entity);

            await _overriddenClient
                .Invoking(async x => await x.PutAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task OverriddenClient_DeleteAsync_SendsIntInBodyAndReceivesOkString()
        {
            const int id = 1;
            using var api = _overriddenApiMockFactory.MockDeleteMethod(id);

            var result = await _overriddenClient.DeleteAsync(id);

            result.Should().NotBeNull();
            result.Should().Be("OK");
        }
    }
}
