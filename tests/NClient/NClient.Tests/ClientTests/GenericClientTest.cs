using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class GenericClientTest
    {
        [Test]
        public async Task GenericClient_PostAsync_IntInBody()
        {
            const int id = 1;
            var entity = new BasicEntity { Value = 1 };
            using var api = GenericApiMockFactory.MockPostMethod(entity, id);

            var result = await NClientGallery.Clients.GetRest().For<IGenericClientWithMetadata>(api.Urls.First()).Build()
                .PostAsync(entity);

            result.Should().Be(id);
        }
    }
}
