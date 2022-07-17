using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Api.Tests.Stubs;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NUnit.Framework;

namespace NClient.Api.Tests.BasicClientUseCases
{
    [Parallelizable]
    public class HandlingTest
    {
        [Test]
        public async Task NClientBuilder_WithSingleCustomHandler_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(host: api.Urls.First())
                .WithHandling(new CustomHandler())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_WithCollectionOfCustomHandlers_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(host: api.Urls.First())
                .WithoutHandling()
                .WithHandling(new CustomHandler(), new CustomHandler())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_WithAdditionalSingleCustomHandler_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(host: api.Urls.First())
                .WithHandling(new CustomHandler())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_WithAdditionalCollectionOfCustomHandlers_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(host: api.Urls.First())
                .WithHandling(new CustomHandler(), new CustomHandler())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
    }
}
