using System.Threading.Tasks;
using FluentAssertions;
using NClient.Extensions;
using NClient.Standalone;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Tests.Controllers;
using NUnit.Framework;

#pragma warning disable 618

namespace NClient.Tests.ClientControllerTests
{
    [Parallelizable]
    public class HeaderControllerTest
    {
        private IHeaderClient _headerClient = null!;
        private HeaderApiMockFactory _headerApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _headerApiMockFactory = new HeaderApiMockFactory(port: 5002);

            _headerClient = new NClientControllerBuilder()
                .Use<IHeaderClient, HeaderController>(_headerApiMockFactory.ApiUri.ToString())
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
