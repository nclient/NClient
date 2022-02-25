using Microsoft.Extensions.Logging;
using NClient.Providers.Serialization;

// ReSharper disable once CheckNamespace
namespace NClient.Providers
{
    /// <summary>Tools that help implement providers.</summary>
    internal class Toolset : IToolset
    {
        /// <summary>Gets the serializer that provides functionality to serialize objects or value types to serialized string and to deserialize serialized string into objects or value types.</summary>
        public ISerializer Serializer { get; }
        
        /// <summary>Gets represents a type used to perform logging.</summary>
        public ILogger? Logger { get; }

        /// <summary>Initializes tools that help implement providers.</summary>
        /// <param name="serializer">The serializer that provides functionality to serialize objects or value types to serialized string and to deserialize serialized string into objects or value types.</param>
        /// <param name="logger">The represents a type used to perform logging.</param>
        public Toolset(ISerializer serializer, ILogger? logger)
        {
            Serializer = serializer;
            Logger = logger;
        }
    }
}
