using System;
using System.Net.Http;
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
        private INClientOptionalBuilder<IBasicClientWithMetadata, HttpRequestMessage, HttpResponseMessage> _optionalBuilder = null!;
        private BasicApiMockFactory _api = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _api = new BasicApiMockFactory(PortsPool.Get());
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            PortsPool.Put(_api.ApiUri.Port);
        }
        
        [SetUp]
        public void SetUp()
        {
            _optionalBuilder = NClientGallery.Clients.GetBasic().For<IBasicClientWithMetadata>(_api.ApiUri.ToString());
        }
        
        [Test]
        public async Task NClientBuilder_FullResilience_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .WithSafeResilience(getDelay: _ => TimeSpan.FromSeconds(0))
                .Build();  
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_IdempotentResilience_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .WithIdempotentResilience(getDelay: _ => TimeSpan.FromSeconds(0))
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_SafeResilience_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .WithSafeResilience(getDelay: _ => TimeSpan.FromSeconds(0))
                .Build();
                        
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_SafeResilienceSettings_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
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
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .WithCustomResilience(selector => selector
                    .ForAllMethods().DoNotUse()
                    .ForMethod(x => (Func<int, Task<int>>)x.GetAsync).Use(
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
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .WithIdempotentResilience(getDelay: _ => TimeSpan.FromSeconds(0))
                .WithCustomResilience(x => x
                    .ForMethod(client => (Func<BasicEntity, Task>)client.PostAsync).DoNotUse())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_CustomResilienceForMethodsByCondition_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .WithCustomResilience(x => x
                    .ForMethodsThat((_, request) => request.Type == RequestType.Create).DoNotUse())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_CustomResilienceForPostMethods_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .WithCustomResilience(x => x
                    .ForMethodsThat((_, request) => request.Type == RequestType.Create).Use())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
    }
}
