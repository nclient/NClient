using System.Threading.Tasks;
using FluentAssertions;
using NClient.Extensions;
using NClient.Standalone;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NClient.Tests.Controllers;
using NUnit.Framework;

#pragma warning disable 618

namespace NClient.Tests.ClientControllerTests
{
    [Parallelizable]
    public class QueryControllerTest
    {
        private IQueryClient _queryClient = null!;
        private QueryApiMockFactory _queryApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _queryApiMockFactory = new QueryApiMockFactory(port: 5003);
            _queryClient = new NClientControllerBuilder()
                .Use<IQueryClient, QueryController>(_queryApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task QueryClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = _queryApiMockFactory.MockGetMethod(id);

            var result = await _queryClient.GetAsync(id);
            result.Should().Be(1);
        }

        [Test]
        public async Task QueryClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _queryApiMockFactory.MockPostMethod(entity);

            await _queryClient
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task QueryClient_PutAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _queryApiMockFactory.MockPutMethod(entity);

            await _queryClient
                .Invoking(async x => await x.PutAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task QueryClient_DeleteAsync_NotThrow()
        {
            const int id = 1;
            using var api = _queryApiMockFactory.MockDeleteMethod(id);

            await _queryClient
                .Invoking(async x => await x.DeleteAsync(id))
                .Should()
                .NotThrowAsync();
        }
    }
}
