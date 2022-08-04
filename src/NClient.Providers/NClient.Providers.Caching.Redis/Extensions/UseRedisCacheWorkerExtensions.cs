using NClient.Providers.Caching.Redis;
using NClient.Providers.Transport;
using StackExchange.Redis;

namespace NClient
{
    public static class UseRedisCacheWorkerExtensions
    {
        /// <summary>Sets the cache worker that can store data to Redis.</summary>
        public static INClientResponseCachingSelector<IRequest, IResponse> UseRedisCaching(
            this INClientTransportResponseCachingSetter<IRequest, IResponse> optionalBuilder, IDatabaseAsync dataBase)
        {
            return optionalBuilder.Use(new RedisCacheWorkerProvider(dataBase));
        }
    }
}
