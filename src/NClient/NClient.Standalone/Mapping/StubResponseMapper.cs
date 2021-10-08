using System;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Mapping;
using NClient.Abstractions.Serialization;

#pragma warning disable 1998

namespace NClient.Standalone.Mapping
{
    public class StubResponseMapper : IResponseMapper<IHttpResponse>
    {
        public bool CanMapTo(Type resultType)
        {
            return true;
        }
        
        public async Task<object?> MapAsync(Type resultType, IHttpRequest httpRequest, IHttpResponse response, ISerializer serializer)
        {
            return null;
        }
    }
}
