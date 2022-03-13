using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class QueryClientTest
    {
        [Test]
        public async Task GetAsync_IntParam_IntInBody()
        {
            const int id = 1;
            using var api = QueryApiMockFactory.MockGetMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IQueryClientWithMetadata>(host: api.Urls.First()).Build()
                .GetAsync(id);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task GetAsync_EnumerableOfIntValuesParam_EnumerableOfIntValuesInBody()
        {
            var ids = new[] { 1, 2, 3 };
            using var api = QueryApiMockFactory.MockGetMethod(ids);

            var result = await NClientGallery.Clients.GetRest().For<IQueryClientWithMetadata>(host: api.Urls.First()).Build()
                .GetAsync(ids);

            result.Should().BeEquivalentTo(ids);
        }
        
        [Test]
        public async Task GetAsync_DictionaryOfIntValuesParam_DictionaryOfIntValuesInBody()
        {
            var keyValues = new Dictionary<string, int> { ["key1"] = 1, ["key2"] = 2, ["key3"] = 3 };
            using var api = QueryApiMockFactory.MockGetMethod(nameof(keyValues), keyValues);

            var result = await NClientGallery.Clients.GetRest().For<IQueryClientWithMetadata>(host: api.Urls.First()).Build()
                .GetAsync(keyValues);

            result.Should().BeEquivalentTo(keyValues);
        }

        [Test]
        public async Task PostAsync_EntityParam_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = QueryApiMockFactory.MockPostMethod(entity);

            await NClientGallery.Clients.GetRest().For<IQueryClientWithMetadata>(host: api.Urls.First()).Build()
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task PutAsync_EntityParam_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = QueryApiMockFactory.MockPutMethod(entity);

            await NClientGallery.Clients.GetRest().For<IQueryClientWithMetadata>(host: api.Urls.First()).Build()
                .Invoking(async x => await x.PutAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task DeleteAsync_IntParam_NotThrow()
        {
            const int id = 1;
            using var api = QueryApiMockFactory.MockDeleteMethod(id);

            await NClientGallery.Clients.GetRest().For<IQueryClientWithMetadata>(host: api.Urls.First()).Build()
                .Invoking(async x => await x.DeleteAsync(id))
                .Should()
                .NotThrowAsync();
        }
    }
}
