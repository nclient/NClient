using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NUnit.Framework;
using StackExchange.Redis;

namespace NClient.Providers.Caching.Redis.Tests
{
    public class RedisCacheWorkerTest
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

            var db = (await ConnectionMultiplexer.ConnectAsync(ConfigurationOptions.Parse("localhost"))).GetDatabase();
            var redisWorker = new RedisCacheWorkerProvider(db);
            await NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(host: api.Urls.First())
                .WithSystemTextJsonSerialization(jsonSerializerOptions)
                .WithResponseCaching(redisWorker)
                .Build()
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }
    }
}
