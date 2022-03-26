using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;
using StackExchange.Redis;

namespace NClient.Providers.Caching.Redis
{
    /// <summary>
    /// The provider of the cache worker that store HTTP response data to Redis.
    /// </summary>
    public class RedisCacheWorkerProvider : IResponseCacheProvider<IRequest, IResponse>
    {
        private readonly IDatabaseAsync _redisDb;

        /// <summary>Initializes the Redis cache worker based serializer provider.</summary>
        /// <param name="redisDb">The redis database.</param>
        public RedisCacheWorkerProvider(IDatabaseAsync redisDb)
        {
            Ensure.IsNotNull(redisDb, nameof(redisDb));

            _redisDb = redisDb;
        }
        
        /// <summary>Creates System.Text.Json <see cref="ISerializer"/> instance.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        public IResponseCacheWorker<IRequest, IResponse> Create(IToolset toolset)
        {
            return new RedisCacheWorker(_redisDb);
        }
    }
}
