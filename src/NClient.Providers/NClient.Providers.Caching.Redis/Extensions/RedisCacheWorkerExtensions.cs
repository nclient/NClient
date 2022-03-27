using NClient.Common.Helpers;
using NClient.Providers.Caching.Redis;
using StackExchange.Redis;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class RedisCacheWorkerExtensions
    {
        /// <summary>Sets the Redis cache for the client.</summary>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithRedisCaching<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IDatabaseAsync dataBase)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));

            return optionalBuilder.WithResponseCaching(new RedisCacheWorkerProvider(dataBase));
        }
    }
}
