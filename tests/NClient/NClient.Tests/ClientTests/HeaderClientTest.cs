using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class HeaderClientTest
    {
        [Test]
        public async Task HeaderClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = HeaderApiMockFactory.MockGetMethod(id);

            var result = await NClientGallery.Clients.GetBasic().For<IHeaderClientWithMetadata>(api.Urls.First()).Build()
                .GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        // TODO: Mock ignores header. Why?
        public async Task HeaderClient_DeleteAsync_NotThrow()
        {
            const int id = 1;
            using var api = HeaderApiMockFactory.MockDeleteMethod(id);

            await NClientGallery.Clients.GetBasic().For<IHeaderClientWithMetadata>(api.Urls.First()).Build()
                .Invoking(async x => await x.DeleteAsync(id))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        // TODO: Mock ignores header. Why?
        public async Task HeaderClient_DeleteAsyncWithStaticHeader_NotThrow()
        {
            const int id = 1;
            using var api = HeaderApiMockFactory.MockDeleteMethod(id);

            await NClientGallery.Clients.GetBasic().For<IHeaderClientWithMetadata>(api.Urls.First()).Build()
                .Invoking(async x => await x.DeleteAsync())
                .Should()
                .NotThrowAsync();
        }
    }
}
