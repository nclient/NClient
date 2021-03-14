using System.Threading.Tasks;
using FluentAssertions;
using NClient.AspNetProxy.Extensions;
using NClient.AspNetProxy.Tests.Controllers;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NUnit.Framework;

namespace NClient.AspNetProxy.Tests.ClientTests
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

            _headerClient = new AspNetClientProvider()
                .Use<IHeaderClient, HeaderController>(_headerApiMockFactory.ApiUri)
                .SetDefaultHttpClientProvider()
                .WithoutResiliencePolicy()
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
