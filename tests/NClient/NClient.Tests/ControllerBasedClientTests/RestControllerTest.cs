using System.Threading.Tasks;
using FluentAssertions;
using NClient.Extensions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NClient.Tests.Controllers;
using NUnit.Framework;

#pragma warning disable 618

namespace NClient.Tests.ControllerBasedClientTests
{
    [Parallelizable]
    public class RestControllerTest
    {
        private IRestClient _restClient = null!;
        private RestApiMockFactory _restApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _restApiMockFactory = new RestApiMockFactory(port: 5004);
            _restClient = new NClientBuilder()
                .Use<IRestClient, RestController>(_restApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task RestClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = _restApiMockFactory.MockIntGetMethod(id);

            var result = await _restClient.GetAsync(id);
            result.Should().Be(1);
        }

        [Test]
        public async Task RestClient_GetAsync_StringInBody()
        {
            const string id = "1";
            using var api = _restApiMockFactory.MockStringGetMethod(id);

            var result = await _restClient.GetAsync(id);
            result.Should().Be(id);
        }

        [Test]
        public async Task RestClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _restApiMockFactory.MockPostMethod(entity);

            await _restClient
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task RestClient_PutAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _restApiMockFactory.MockPutMethod(entity);

            await _restClient
                .Invoking(async x => await x.PutAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task RestClient_DeleteAsync_NotThrow()
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
