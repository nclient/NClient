using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

#if NET6_0_OR_GREATER
using NClient.Providers.Serialization.SystemTextJson.Tests.Contexts;
#endif

namespace NClient.Providers.Serialization.SystemTextJson.Tests
{
    public class SystemTextJsonClientTest
    {
        [Test]
        public async Task Serialize_JsonSerializerOptionsWithPropertyNamingPolicy_ReturnBasicEntity()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var expectedJson = JsonSerializer.Serialize(entity, jsonSerializerOptions);
            using var api = BasicApiMockFactory.MockPostMethod(expectedJson);

            await NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(host: api.Urls.First())
                .WithSystemTextJsonSerialization(jsonSerializerOptions)
                .Build()
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public void Deserialize_JsonSerializerOptionsWithPropertyNamingPolicy_ReturnEmptyBasicEntity()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var camelCaseContentBytes = JsonSerializer.SerializeToUtf8Bytes(entity, jsonSerializerOptions);
            using var api = ReturnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = NClientGallery.Clients.GetRest().For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithSystemTextJsonSerialization(jsonSerializerOptions)
                .Build()
                .GetIHttpResponse(id);
            
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(new BasicEntity());
            result.Content.Bytes.Should().NotBeEquivalentTo(camelCaseContentBytes);
        }

        #if NET6_0_OR_GREATER
        [Test]
        public async Task Serialize_JsonSerializerOptionsWithJsonSerializerContext_ReturnBasicEntity()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            var jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.AddContext<BasicEntityJsonContext>();
            var expectedJson = JsonSerializer.Serialize(entity, jsonSerializerOptions);
            using var api = BasicApiMockFactory.MockPostMethod(expectedJson);

            await NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(host: api.Urls.First())
                .WithSystemTextJsonSerialization(jsonSerializerOptions)
                .Build()
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test, Ignore(reason: "JsonSourceGenerationOptions don't work like JsonSerializerOptions. For example, JsonSerializerOptions ignore fields if the case is different, and JsonSourceGenerationOptions ignore the case.")]
        public void Deserialize_JsonSerializerOptionsWithJsonSerializerContext_ReturnEmptyBasicEntity()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            var jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.AddContext<BasicEntityJsonContext>();
            var camelCaseContentBytes = JsonSerializer.SerializeToUtf8Bytes(entity, jsonSerializerOptions);
            using var api = ReturnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = NClientGallery.Clients.GetRest().For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithSystemTextJsonSerialization(jsonSerializerOptions)
                .Build()
                .GetIHttpResponse(id);

            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(new BasicEntity());
            result.Content.Bytes.Should().NotBeEquivalentTo(camelCaseContentBytes);
        }
        #endif
    }
}
