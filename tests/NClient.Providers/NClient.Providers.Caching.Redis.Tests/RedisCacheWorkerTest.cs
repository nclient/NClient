using System;
using System.Linq;
using System.Text.Json;
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
        public void Caching_RedisCaching_NotThrow()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var camelCaseContentBytes = JsonSerializer.SerializeToUtf8Bytes(entity, jsonSerializerOptions);
            
            var dbMock = new Mock<IDatabaseAsync>();
            dbMock.Setup(x => x.StringGetAsync(It.IsAny<RedisKey>(), CommandFlags.None)).ReturnsAsync(It.IsAny<RedisValue>());
            dbMock.Setup(x => x.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan>(), When.Always, CommandFlags.None)).ReturnsAsync(true);
            
            using var api = ReturnApiMockFactory.MockGetAsyncMethod(id, entity);
            
            var result = NClientGallery.Clients.GetRest().For<IReturnClientWithMetadata>(host: api.Urls.First())
                .WithNewtonsoftJsonSerialization()
                .WithRedisCaching(dbMock.Object)
                .Build()
                .GetIHttpResponse(id);
            
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(entity);
            result.Content.Bytes.Should().NotBeEquivalentTo(camelCaseContentBytes);

            dbMock.Verify(mock => mock.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan>(), When.Always, CommandFlags.None), Times.Once());
        }
    }
}
