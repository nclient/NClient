using System.Threading.Tasks;
using FluentAssertions;
using NClient.InterfaceProxy.Extensions;
using NClient.InterfaceProxy.Tests.Clients;
using NClient.Testing.Common.Apis;
using NUnit.Framework;

namespace NClient.InterfaceProxy.Tests.ClientTests
{
    [Parallelizable]
    public class HeaderClientTest
    {
        private IHeaderClientWithMetadata _headerClient = null!;
        private HeaderApiMockFactory _headerApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _headerApiMockFactory = new HeaderApiMockFactory(port: 5008);

            _headerClient = new NClientBuilder()
                .Use<IHeaderClientWithMetadata>(_headerApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task HeaderClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = _headerApiMockFactory.MockGetMethod(id);

            var result = await _headerClient.GetAsync(id);
            result.Should().Be(1);
        }

        [Test]
        public async Task HeaderClient_DeleteAsync_NotThrow()
        {
            const int id = 1;
            using var api = _headerApiMockFactory.MockDeleteMethod(id);

            await _headerClient
                .Invoking(async x => await x.DeleteAsync(id))
                .Should()
                .NotThrowAsync();
        }
    }
}
