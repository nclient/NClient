using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Api.Tests.Stubs;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Api.Tests.BasicClientUseCases
{
    [Parallelizable]
    public class ResponseValidationTest
    {
        [Test]
        public async Task NClientBuilder_NoResponseValidation_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithoutResponseValidation()
                .Build();

            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_ReplaceResponseValidationWithCustomSettings_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithoutResponseValidation()
                .WithAdvancedResponseValidation(x => x
                    .ForTransport().Use(new CustomResponseValidatorSettings()))
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_AddResponseValidationWithCustomSettings_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithAdvancedResponseValidation(x => x
                    .ForTransport().Use(new CustomResponseValidatorSettings()))
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_AddCustomResponseValidation_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithResponseValidation(
                    isSuccess: _ => true, 
                    onFailure: _ =>
                    {
                    })
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
    }
}
