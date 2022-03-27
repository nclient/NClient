using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using NClient.Common.Helpers;
using StackExchange.Redis;

namespace NClient.Providers.Caching.Redis
{
    internal class RedisCacheWorker : IResponseCacheWorker
    {
        private readonly IDatabaseAsync _redisDb;
        private readonly IToolset _toolset;
        public RedisCacheWorker(IDatabaseAsync redisDb, IToolset toolset)
        {
            Ensure.IsNotNull(redisDb, nameof(redisDb));

            _redisDb = redisDb;
            _toolset = toolset;
        }
        public async Task<TResponse?> FindAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        {
            Ensure.IsNotNull(request, nameof(request));
            var result = default(TResponse);
            var bytes = (byte[]) await _redisDb.StringGetAsync(GenerateKey(request));

            if (bytes == null)
                return result;
            
            using var stream = new MemoryStream(bytes);
            result = (TResponse) new BinaryFormatter().Deserialize(stream);

            return result;
        }
        public async Task PutAsync<TRequest, TResponse>(TRequest request, TResponse response, TimeSpan? lifeTime = null, CancellationToken cancellationToken = default)
        {
            Ensure.IsNotNull(request, nameof(request));
            Ensure.IsNotNull(response, nameof(response));

            var serializedResponse = _toolset.Serializer.Serialize(response);

            if (!await _redisDb.StringSetAsync(GenerateKey(request), serializedResponse, lifeTime))
                throw new InvalidOperationException("Couldn't save data to Redis");
        }

        private string GenerateKey<TRequest>(TRequest request)
        {
            Ensure.IsNotNull(request, nameof(request));
            return request!.ToString();
            /*
            var key = new StringBuilder(request.Type.ToString());
            key.Append(request.Resource.Scheme);
            key.Append(request.Resource.Host);
            key.Append(request.Resource.Query);
            foreach (var requestMetadata in request.Metadatas)
            {
                key.Append(requestMetadata.Key);
                foreach (var metadata in requestMetadata.Value)
                {
                    key.Append(metadata.Name);
                    key.Append(metadata.Value);
                }
            }
            var plainTextBytes = Encoding.UTF8.GetBytes(key.ToString());
            return Convert.ToBase64String(plainTextBytes);*/
        }
    }
}
