using System.Collections.Generic;
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
    public class FormUrlencodedClientTest : ClientTestBase<IFormUrlencodedClientWithMetadata>
    {
        [Test]
        public async Task PostAsync_WithIntValue_ShouldReturnIntInBody()
        {
            const int id = 1;
            var keyValues = new Dictionary<string, object>
            {
                [nameof(id)] = id
            };
            using var api = FormUrlencodedApiMockFactory.MockPostMethod(id, keyValues);

            var result = await NClientGallery.Clients.GetRest().For<IFormUrlencodedClientWithMetadata>(host: api.Urls.First()).Build()
                .PostAsync(id);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task PostAsync_WithDictionary_ShouldReturnIntInBody()
        {
            const int id = 1;
            const int value = 1;
            var keyValues = new Dictionary<string, object>
            {
                [nameof(id)] = id,
                [nameof(value)] = value
            };
            using var api = FormUrlencodedApiMockFactory.MockPostMethod(id, keyValues);

            var result = await NClientGallery.Clients.GetRest().For<IFormUrlencodedClientWithMetadata>(host: api.Urls.First()).Build()
                .PostAsync(keyValues);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task PostAsync_WithCustomObject_ShouldReturnIntInBody()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            var keyValues = new Dictionary<string, object>
            {
                [nameof(BasicEntity.Id)] = entity.Id,
                [nameof(BasicEntity.Value)] = entity.Value
            };
            using var api = FormUrlencodedApiMockFactory.MockPostMethod(entity.Id, keyValues);

            var result = await NClientGallery.Clients.GetRest().For<IFormUrlencodedClientWithMetadata>(host: api.Urls.First()).Build()
                .PostAsync(entity);

            result.Should().Be(entity.Id);
        }
    }
}
