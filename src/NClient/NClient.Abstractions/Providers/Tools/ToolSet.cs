using Microsoft.Extensions.Logging;
using NClient.Providers.Serialization;

// ReSharper disable once CheckNamespace
namespace NClient.Providers
{
    internal class Toolset : IToolset
    {
        public ISerializer Serializer { get; }
        
        public ILogger? Logger { get; }

        public Toolset(ISerializer serializer, ILogger? logger)
        {
            Serializer = serializer;
            Logger = logger;
        }
    }
}
