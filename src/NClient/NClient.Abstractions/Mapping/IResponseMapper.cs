using System;
using System.Threading.Tasks;
using NClient.Abstractions.Serialization;
using NClient.Providers.Results.HttpMessages;

namespace NClient.Abstractions.Mapping
{
    public interface IResponseMapper
    {
        bool CanMapTo(Type resultType);
        Task<object?> MapAsync(Type resultType, IHttpResponse httpResponse, ISerializer serializer);
    }
}
