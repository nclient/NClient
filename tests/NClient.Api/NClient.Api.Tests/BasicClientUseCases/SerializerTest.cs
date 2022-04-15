using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NUnit.Framework;

namespace NClient.Api.Tests.BasicClientUseCases
{
    [Parallelizable]
    public class SerializerTest
    {
        [Test]
        public async Task NClientBuilder_WithNewtonsoft_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(host: api.Urls.First())
                .WithNewtonsoftJsonSerialization()
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
    }
}
