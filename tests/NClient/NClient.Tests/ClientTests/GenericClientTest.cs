using System.Threading.Tasks;
using FluentAssertions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class GenericClientTest
    {
        private IGenericClientWithMetadata _genericClient = null!;
        private GenericApiMockFactory _genericApiMockFactory = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _genericApiMockFactory = new GenericApiMockFactory(PortsPool.Get());
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            PortsPool.Put(_genericApiMockFactory.ApiUri.Port);
        }
        
        [SetUp]
        public void Setup()
        {
            _genericClient = NClientGallery.Clients
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
