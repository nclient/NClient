using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
        public async Task PostAsync_WithIntValue_ShouldReturnOk()
        {
            const int id = 1;
            var keyValues = new Dictionary<string, object>
            {
                [nameof(id)] = id
            };
            using var api = FormUrlencodedApiMockFactory.MockPostMethod(keyValues);

            var result = await NClientGallery.Clients.GetRest().For<IFormUrlencodedClientWithMetadata>(host: api.Urls.First()).Build()
                .PostAsync(id);

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
        
        [Test]
        public async Task PostAsync_WithDictionary_ShouldReturnOk()
        {
            var keyValues = new Dictionary<string, object>
            {
                ["id"] = 1,
                ["value"] = "test"
            };
            using var api = FormUrlencodedApiMockFactory.MockPostMethod(keyValues);

            var result = await NClientGallery.Clients.GetRest().For<IFormUrlencodedClientWithMetadata>(host: api.Urls.First()).Build()
                .PostAsync(keyValues);

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
        
        [Test]
        public async Task PostAsync_WithCustomObject_ShouldReturnOk()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            var keyValues = new Dictionary<string, object>
            {
                [nameof(BasicEntity.Id)] = entity.Id,
                [nameof(BasicEntity.Value)] = entity.Value
            };
            using var api = FormUrlencodedApiMockFactory.MockPostMethod(keyValues);

            var result = await NClientGallery.Clients.GetRest().For<IFormUrlencodedClientWithMetadata>(host: api.Urls.First()).Build()
                .PostAsync(entity);

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
