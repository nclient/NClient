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
    public class ResponseValidationTest
    {
        [Test]
        public async Task NClientBuilder_NoResponseValidation_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetBasic().For<IBasicClientWithMetadata>(api.Urls.First())
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
            var client = NClientGallery.Clients.GetBasic().For<IBasicClientWithMetadata>(api.Urls.First())
                .WithoutResponseValidation()
                .WithCustomResponseValidation(new CustomResponseValidatorSettings())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_AddResponseValidationWithCustomSettings_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetBasic().For<IBasicClientWithMetadata>(api.Urls.First())
                .WithCustomResponseValidation(new CustomResponseValidatorSettings())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_AddCustomResponseValidation_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetBasic().For<IBasicClientWithMetadata>(api.Urls.First())
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
