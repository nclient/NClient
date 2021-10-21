using System;
using System.Threading.Tasks;
using NClient.Abstractions.Providers.Serialization;

namespace NClient.Abstractions.Providers.Results
{
    public interface IResultBuilder<TResponse>
    {
        bool CanBuild(Type resultType, TResponse response);
        Task<object?> BuildAsync(Type resultType, TResponse response, ISerializer serializer);
    }
}
