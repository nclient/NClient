using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Tests.ClientTests.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class HeaderClientTest : ClientTestBase<IHeaderClientWithMetadata>
    {
        [Test]
        public async Task GetWithSingleHeaderAsync_ShouldPassSingleHeader_ThenReturnIntInBody()
        {
            const int id = 1;
            using var api = HeaderApiMockFactory.MockGetMethodWithSingleHeader(id);

            var result = await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .GetWithSingleHeaderAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task GetWithMultipleHeaderValuesAsync_ShouldPassMultipleHeaderValues_ThenReturnIntArrayInBody()
        {
            const int id1 = 1;
            const int id2 = 2;
            using var api = HeaderApiMockFactory.MockGetMethodWithMultipleHeaderValues(id1, id2);

            var result = await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .GetWithMultipleHeaderValuesAsync(id1, id2);

            result.Should().BeEquivalentTo(new[] { id1, id2 });
        }
        
        [Test]
        public async Task GetWithMultipleHeadersAsync_ShouldPassMultipleHeaders_ThenReturnIntArrayInBody()
        {
            const int id1 = 1;
            const int id2 = 2;
            using var api = HeaderApiMockFactory.MockGetMethodWithMultipleHeaders(id1, id2);

            var result = await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .GetWithMultipleHeadersAsync(id1, id2);

            result.Should().BeEquivalentTo(new[] { id1, id2 });
        }

        [Test]
        public async Task GetWithSingleStaticHeaderAsync_ShouldPassSingleHeader_ThenReturnIntInBody()
        {
            const int id = 1;
            using var api = HeaderApiMockFactory.MockGetMethodWithSingleHeader(id);

            var result = await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .GetWithSingleStaticHeaderAsync();
            
            result.Should().Be(id);
        }
        
        [Test]
        public async Task GetWithMultipleStaticHeaderValuesAsync_ShouldPassMultipleHeaderValues_ThenReturnIntArrayInBody()
        {
            const int id1 = 1;
            const int id2 = 2;
            using var api = HeaderApiMockFactory.MockGetMethodWithMultipleHeaderValues(id1, id2);

            var result = await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .GetWithMultipleStaticHeaderValuesAsync();
            
            result.Should().BeEquivalentTo(new[] { id1, id2 });
        }
        
        [Test]
        public async Task GetWithMultipleStaticHeadersAsync_ShouldPassMultipleHeaders_ThenReturnIntArrayInBody()
        {
            const int id1 = 1;
            const int id2 = 2;
            using var api = HeaderApiMockFactory.MockGetMethodWithMultipleHeaders(id1, id2);

            var result = await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .GetWithMultipleStaticHeadersAsync();
            
            result.Should().BeEquivalentTo(new[] { id1, id2 });
        }
    }
}
