using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
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
        public async Task Caching_RedisCaching_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var expectedJson = JsonSerializer.Serialize(entity, jsonSerializerOptions);
            using var api = BasicApiMockFactory.MockPostMethod(expectedJson);

            var dbMock = new Mock<IDatabaseAsync>();
            dbMock.Setup(x => x.StringGetAsync(It.IsAny<RedisKey>(), CommandFlags.None)).ReturnsAsync(It.IsAny<RedisValue>());
            dbMock.Setup(x => x.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), TimeSpan.MaxValue, When.Always, CommandFlags.None)).ReturnsAsync(true);

            await NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(host: api.Urls.First())
                .WithSystemTextJsonSerialization(jsonSerializerOptions)
                .WithRedisCaching(dbMock.Object)
                .Build()
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }
    }
}
