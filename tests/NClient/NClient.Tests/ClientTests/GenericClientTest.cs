using System.Threading.Tasks;
using FluentAssertions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class GenericClientTest
    {
        private IGenericClientWithMetadata _genericClient = null!;
        private GenericApiMockFactory _genericApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _genericApiMockFactory = new GenericApiMockFactory(port: 5021);

            _genericClient = NClientGallery.NativeClients
                .GetBasic()
                .For<IGenericClientWithMetadata>(_genericApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task GenericClient_PostAsync_IntInBody()
        {
            const int id = 1;
            var entity = new BasicEntity { Value = 1 };
            using var api = _genericApiMockFactory.MockPostMethod(entity, id);

            var result = await _genericClient.PostAsync(entity);

            result.Should().Be(id);
        }
    }
}
