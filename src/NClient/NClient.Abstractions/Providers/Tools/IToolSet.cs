using Microsoft.Extensions.Logging;
using NClient.Providers.Serialization;

// ReSharper disable once CheckNamespace
namespace NClient.Providers
{
    public interface IToolSet
    {
        ISerializer Serializer { get; } 
        ILogger? Logger { get; set; }
    }
}
