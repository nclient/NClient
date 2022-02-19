using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Providers.Transport;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Api.Tests.BasicClientUseCases
{
    [Parallelizable]
    public class ResilienceTest
    {
        [Test]
        public async Task NClientBuilder_FullResilience_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithSafeResilience(getDelay: _ => TimeSpan.FromSeconds(0))
                .Build();  
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_IdempotentResilience_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithIdempotentResilience(getDelay: _ => TimeSpan.FromSeconds(0))
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_SafeResilience_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithSafeResilience(getDelay: _ => TimeSpan.FromSeconds(0))
                .Build();
                        
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_SafeResilienceSettings_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithSafeResilience(
                    getDelay: _ => TimeSpan.FromSeconds(2),
                    shouldRetry: x => !x.Response.IsSuccessStatusCode)
                .Build();
                        
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }

        [Test]
        public async Task NClientBuilder_CustomResilience_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithResilience(selector => selector
                    .ForAllMethods().DoNotUse()
                    .ForMethod(x => (Func<int, Task<int>>) x.GetAsync).Use(
                        maxRetries: 3,
                        getDelay: _ => TimeSpan.FromSeconds(0),
                        shouldRetry: x => !x.Response.IsSuccessStatusCode))
                .Build();
                        
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_IdempotentResilienceExceptSelectedMethod_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithIdempotentResilience(getDelay: _ => TimeSpan.FromSeconds(0))
                .WithResilience(x => x
                    .ForMethod(client => (Func<BasicEntity, Task>) client.PostAsync).DoNotUse())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_CustomResilienceForMethodsByCondition_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithResilience(x => x
                    .ForMethodsThat((_, request) => request.Type == RequestType.Create).DoNotUse())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_CustomResilienceForPostMethods_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithResilience(x => x
                    .ForMethodsThat((_, request) => request.Type == RequestType.Create).Use())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
    }
}
