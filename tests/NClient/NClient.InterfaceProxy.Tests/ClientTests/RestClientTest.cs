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
    public class RestClientTest
    {
        private IRestClientWithMetadata _restClient = null!;
        private RestApiMockFactory _restApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _restApiMockFactory = new RestApiMockFactory(port: 5010);
            _restClient = new ClientProvider()
                .Use<IRestClientWithMetadata>(_restApiMockFactory.ApiUri)
                .SetDefaultHttpClientProvider()
                .WithoutResiliencePolicy()
                .Build();
        }

        [Test]
        public async Task BasicClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = _restApiMockFactory.MockGetMethod(id);

            var result = await _restClient.GetAsync(id);
            result.Should().Be(1);
        }

        [Test]
        public async Task BasicClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _restApiMockFactory.MockPostMethod(entity);

            await _restClient
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task BasicClient_PutAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _restApiMockFactory.MockPutMethod(entity);

            await _restClient
                .Invoking(async x => await x.PutAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task BasicClient_DeleteAsync_NotThrow()
        {
            const int id = 1;
            using var api = _restApiMockFactory.MockDeleteMethod(id);

            await _restClient
                .Invoking(async x => await x.DeleteAsync(id))
                .Should()
                .NotThrowAsync();
        }
    }
}
