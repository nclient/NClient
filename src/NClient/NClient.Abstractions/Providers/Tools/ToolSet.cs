using Microsoft.Extensions.Logging;
using NClient.Providers.Serialization;

// ReSharper disable once CheckNamespace
namespace NClient.Providers
{
    internal class ToolSet : IToolSet
    {
        public ISerializer Serializer { get; }
        public ILogger? Logger { get; set; }

        public ToolSet(ISerializer serializer, ILogger? logger)
        {
            Serializer = serializer;
            Logger = logger;
        }
    }
}
