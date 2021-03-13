using System.Threading.Tasks;
using FluentAssertions;
using NClient.AspNetProxy.Extensions;
using NClient.AspNetProxy.Tests.Controllers;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.AspNetProxy.Tests.ClientTests
{
    [Parallelizable]
    public class BasicControllerTest
    {
        private IBasicClient _basicClient = null!;
        private BasicApiMockFactory _basicApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _basicApiMockFactory = new BasicApiMockFactory(port: 5001);

            _basicClient = new AspNetClientProvider()
                .Use<IBasicClient, BasicController>(_basicApiMockFactory.ApiUri)
                .SetDefaultHttpClientProvider()
                .WithoutResiliencePolicy()
                .Build();
        }

        [Test]
        public async Task BasicClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = _basicApiMockFactory.MockGetMethod(id);

            var result = await _basicClient.GetAsync(id);
            result.Should().Be(1);
        }

        [Test]
        public async Task BasicClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _basicApiMockFactory.MockPostMethod(entity);

            await _basicClient
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task BasicClient_PutAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _basicApiMockFactory.MockPutMethod(entity);

            await _basicClient
                .Invoking(async x => await x.PutAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task BasicClient_DeleteAsync_NotThrow()
        {
            const int id = 1;
            using var api = _basicApiMockFactory.MockDeleteMethod(id);

            await _basicClient
                .Invoking(async x => await x.DeleteAsync(id))
                .Should()
                .NotThrowAsync();
        }
    }
}
