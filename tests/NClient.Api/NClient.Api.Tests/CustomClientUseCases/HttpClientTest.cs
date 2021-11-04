using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Providers.Api.Rest.Extensions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NUnit.Framework;

namespace NClient.Api.Tests.CustomClientUseCases
{
    [Parallelizable]
    public class HttpClientTest
    {
        [Test]
        public async Task AdvancedNClientBuilder_WithRestSharp_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var client = NClientGallery.Clients.GetAdvanced().For<IBasicClientWithMetadata>(api.Urls.First())
                .UsingRestApi()
                .UsingRestSharpTransport()
                .UsingJsonSerializer()
                .WithResponseValidation(x => x
                    .ForTransport().UseRestSharpResponseValidation())
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
    }
}
