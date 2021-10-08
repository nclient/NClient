using System;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Mapping;
using NClient.Abstractions.Serialization;

#pragma warning disable 1998

namespace NClient.Standalone.Mapping
{
    public class StubResponseMapper : IResponseMapper<object>
    {
        public bool CanMapTo(Type resultType)
        {
            return true;
        }
        
        public async Task<object?> MapAsync(Type resultType, IHttpRequest httpRequest, object response, ISerializer serializer)
        {
            return null;
        }
    }
}
