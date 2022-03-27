using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NClient.Common.Helpers;
using NClient.Providers.Transport;
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
        public async Task<IResponse?> FindAsync(IRequest request, CancellationToken cancellationToken = default)
        {
            Ensure.IsNotNull(request, nameof(request));
            
            var serializedResponse = (await _redisDb.StringGetAsync(GenerateKey(request))).ToString();

            if (string.IsNullOrEmpty(serializedResponse))
                return default;

            return (IResponse) _toolset.Serializer.Deserialize(serializedResponse, typeof(IResponse))!;
        }
        public async Task PutAsync(IRequest request, IResponse response, TimeSpan? lifeTime = null, CancellationToken cancellationToken = default)
        {
            Ensure.IsNotNull(request, nameof(request));
            Ensure.IsNotNull(response, nameof(response));

            var serializedResponse = _toolset.Serializer.Serialize(response);

            if (!await _redisDb.StringSetAsync(GenerateKey(request), serializedResponse, lifeTime))
                throw new InvalidOperationException("Couldn't save data to Redis");
        }

        private string GenerateKey(IRequest request)
        {
            Ensure.IsNotNull(request, nameof(request));
            
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
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
