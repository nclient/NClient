using System;
using System.Threading.Tasks;
using NClient.Providers.Serialization;

namespace NClient.Providers.Results
{
    public interface IResultBuilder<TResponse>
    {
        bool CanBuild(Type resultType, TResponse response);
        Task<object?> BuildAsync(Type resultType, TResponse response, ISerializer serializer);
    }
}
