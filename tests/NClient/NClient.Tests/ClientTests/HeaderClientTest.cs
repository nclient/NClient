using System.Threading.Tasks;
using FluentAssertions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class HeaderClientTest
    {
        private IHeaderClientWithMetadata _headerClient = null!;
        private HeaderApiMockFactory _headerApiMockFactory = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _headerApiMockFactory = new HeaderApiMockFactory(PortsPool.Get());
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            PortsPool.Put(_headerApiMockFactory.ApiUri.Port);
        }
        
        [SetUp]
        public void Setup()
        {
            _headerClient = NClientGallery.Clients
                .GetBasic()
                .For<IHeaderClientWithMetadata>(_headerApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task HeaderClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = _headerApiMockFactory.MockGetMethod(id);

            var result = await _headerClient.GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        // TODO: Mock ignores header. Why?
        public async Task HeaderClient_DeleteAsync_NotThrow()
        {
            const int id = 1;
            using var api = _headerApiMockFactory.MockDeleteMethod(id);

            await _headerClient
                .Invoking(async x => await x.DeleteAsync(id))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        // TODO: Mock ignores header. Why?
        public async Task HeaderClient_DeleteAsyncWithStaticHeader_NotThrow()
        {
            const int id = 1;
            using var api = _headerApiMockFactory.MockDeleteMethod(id);

            await _headerClient
                .Invoking(async x => await x.DeleteAsync())
                .Should()
                .NotThrowAsync();
        }
    }
}
