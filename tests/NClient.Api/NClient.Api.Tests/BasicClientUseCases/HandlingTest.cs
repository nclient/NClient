using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Api.Tests.Stubs;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
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
            var client = NClientGallery.Clients.GetBasic().For<IBasicClientWithMetadata>(api.Urls.First())
                .AsAdvanced()
                .WithCustomHandling(x => x
                    .WithCustomTransportHandling(new CustomHandler()))
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_WithCollectionOfCustomHandlers_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetBasic().For<IBasicClientWithMetadata>(api.Urls.First())
                .AsAdvanced()
                .WithoutHandling()
                .WithCustomHandling(x => x
                    .WithCustomTransportHandling(new CustomHandler(), new CustomHandler()))
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_WithAdditionalSingleCustomHandler_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetBasic().For<IBasicClientWithMetadata>(api.Urls.First())
                .AsAdvanced()
                .WithCustomHandling(x => x
                    .WithCustomTransportHandling(new CustomHandler()))
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_WithAdditionalCollectionOfCustomHandlers_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetBasic().For<IBasicClientWithMetadata>(api.Urls.First())
                .AsAdvanced()
                .WithCustomHandling(x => x
                    .WithCustomTransportHandling(new CustomHandler(), new CustomHandler()))
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
    }
}
