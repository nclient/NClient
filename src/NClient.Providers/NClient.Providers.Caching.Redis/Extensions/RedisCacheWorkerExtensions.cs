using NClient.Common.Helpers;
using NClient.Providers.Caching.Redis;
using NClient.Providers.Transport;
using StackExchange.Redis;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class RedisCacheWorkerExtensions
    {
        /// <summary>Sets the Redis cache for the client.</summary>
        public static INClientOptionalBuilder<TClient, IRequest, IResponse> WithRedisCaching<TClient>(
            this INClientOptionalBuilder<TClient, IRequest, IResponse> optionalBuilder,
            IDatabaseAsync dataBase)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));

            return optionalBuilder.WithResponseCaching(new RedisCacheWorkerProvider(dataBase));
        }
    }
}
