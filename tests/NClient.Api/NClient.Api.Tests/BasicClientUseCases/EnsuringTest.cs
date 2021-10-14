using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Abstractions.Builders;
using NClient.Api.Tests.Helpers;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NUnit.Framework;

namespace NClient.Api.Tests.BasicClientUseCases
{
    [Parallelizable]
    public class EnsuringTest
    {
        private INClientOptionalBuilder<IBasicClientWithMetadata, HttpRequestMessage, HttpResponseMessage> _optionalBuilder = null!;
        private BasicApiMockFactory _api = null!;

        [SetUp]
        public void SetUp()
        {
            _api = new BasicApiMockFactory(5023);
            _optionalBuilder = NClientGallery.Clients.GetBasic().For<IBasicClientWithMetadata>(_api.ApiUri.ToString());
        }
        
        [Test]
        public async Task NClientBuilder_NoEnsuringSuccess_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .NotEnsuringSuccess()
                .Build();

            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_EnsuringSuccessWithCustomSettings_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .EnsuringCustomSuccess(new CustomEnsuringSettings())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_EnsuringCustomSuccess_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .EnsuringCustomSuccess(
                    successCondition: _ => true, 
                    onFailure: _ =>
                    {
                    })
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
    }
}
