using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Abstractions.Builders;
using NClient.Abstractions.Resilience.Settings;
using NClient.Resilience;
using NClient.Testing.Common.Apis;
using NClient.Tests.Clients;
using NUnit.Framework;

namespace NClient.Api.Tests.BasicClientUseCases
{
    [Parallelizable]
    public class ResilienceTest
    {
        private INClientOptionalBuilder<IBasicClientWithMetadata, HttpRequestMessage, HttpResponseMessage> _optionalBuilder = null!;
        private BasicApiMockFactory _api = null!;

        [SetUp]
        public void SetUp()
        {
            _api = new BasicApiMockFactory(5026);
            _optionalBuilder = new NClientBuilder().For<IBasicClientWithMetadata>(_api.ApiUri.ToString());
        }
        
        [Test]
        public async Task NClientBuilder_ForceResilience_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .WithSafeResilience()
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
                .WithIdempotentResilience()
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
                .WithSafeResilience()
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
                .WithSafeResilience(new ResiliencePolicySettings<HttpRequestMessage, HttpResponseMessage>(
                    retryCount: 3,
                    sleepDuration: _ => TimeSpan.FromSeconds(2),
                    resultPredicate: x => x.Response.IsSuccessStatusCode)) // TODO: неудобно (дженерик + конструктор)
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
                .WithCustomResilience(customizer => customizer     //TODO: неудачное название параметра
                    .ForAllMethods().Use(new NoResiliencePolicySettings())                                         // TODO: нужен алиас
                    .ForMethod(x => (Func<int, Task<int>>)x.GetAsync).Use(new ResiliencePolicySettings<HttpRequestMessage, HttpResponseMessage>( // TODO: если заменить на конкретные релизации, то можно писать просто new{}
                        retryCount: 3,
                        sleepDuration: _ => TimeSpan.FromSeconds(2),
                        resultPredicate: x => x.Response.IsSuccessStatusCode)))
                .Build();
                        
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test, Ignore("NotImplemente")]
        public async Task NClientBuilder_IdempotentResilienceExceptSelectedMethod_NotThrow()
        {
            throw new NotImplementedException();
        }
        
        [Test, Ignore("NotImplemente")]
        public async Task NClientBuilder_CustomResilienceForMethodsByCondition_NotThrow()
        {
            throw new NotImplementedException();
        }
        
        [Test, Ignore("NotImplemente")]
        public async Task NClientBuilder_CustomResilienceForPostMethods_NotThrow()
        {
            throw new NotImplementedException();
        }
    }
}
