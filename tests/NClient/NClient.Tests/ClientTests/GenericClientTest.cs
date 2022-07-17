using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NClient.Tests.ClientTests.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class GenericClientTest : ClientTestBase<IGenericClientWithMetadata>
    {
        [Test]
        public async Task GenericClient_PostAsync_IntInBody()
        {
            const int id = 1;
            var entity = new BasicEntity { Value = 1 };
            using var api = GenericApiMockFactory.MockPostMethod(entity, id);

            var result = await NClientGallery.Clients.GetRest().For<IGenericClientWithMetadata>(host: api.Urls.First()).Build()
                .PostAsync(entity);

            result.Should().Be(id);
        }
    }
}
