using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NClient.Common.Helpers;
using NClient.Providers.Transport;
using StackExchange.Redis;

namespace NClient.Providers.Caching.Redis
{
    internal class RedisCacheWorker : IResponseCacheWorker<IRequest, IResponse>
    {
        private readonly IDatabaseAsync _redisDb;
        public RedisCacheWorker(IDatabaseAsync redisDb)
        {
            Ensure.IsNotNull(redisDb, nameof(redisDb));

            _redisDb = redisDb;
        }
        public async Task<IResponse?> FindAsync(IRequest request, CancellationToken cancellationToken = default)
        {
            Ensure.IsNotNull(request, nameof(request));
            IResponse? result = null;
            var bytes = (byte[]) await _redisDb.StringGetAsync(GenerateKey(request));

            if (bytes == null)
                return result;
            
            using var stream = new MemoryStream(bytes);
            result = (IResponse) new BinaryFormatter().Deserialize(stream);

            return result;
        }
        public async Task PutAsync(IRequest request, IResponse response, TimeSpan? lifeTime = null, CancellationToken cancellationToken = default)
        {
            Ensure.IsNotNull(request, nameof(request));
            Ensure.IsNotNull(response, nameof(response));
            byte[] bytes;

            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, response);
                bytes = stream.ToArray();
            }

            if (!await _redisDb.StringSetAsync(GenerateKey(request), bytes, lifeTime))
                throw new InvalidOperationException("Couldn't save data to Redis");
        }

        private string GenerateKey(IRequest request)
        {
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
