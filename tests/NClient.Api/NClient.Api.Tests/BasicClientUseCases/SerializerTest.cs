using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Abstractions.Builders;
using NClient.Providers.Serialization.Newtonsoft;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NUnit.Framework;

namespace NClient.Api.Tests.BasicClientUseCases
{
    [Parallelizable]
    public class SerializerTest
    {
        private INClientOptionalBuilder<IBasicClientWithMetadata, HttpRequestMessage, HttpResponseMessage> _optionalBuilder = null!;
        private BasicApiMockFactory _api = null!;

        [SetUp]
        public void SetUp()
        {
            _api = new BasicApiMockFactory(5027);
            _optionalBuilder = NClientGallery.NativeClients.GetBasic().For<IBasicClientWithMetadata>(_api.ApiUri.ToString());
        }
        
        [Test]
        public async Task NClientBuilder_WithNewtonsoft_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var client = _optionalBuilder
                .WithNewtonsoftJsonSerialization()
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
    }
}
