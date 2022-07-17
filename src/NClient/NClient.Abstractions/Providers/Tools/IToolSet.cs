using Microsoft.Extensions.Logging;
using NClient.Providers.Serialization;

// ReSharper disable once CheckNamespace
namespace NClient.Providers
{
    /// <summary>Tools that help implement providers.</summary>
    public interface IToolset
    {
        /// <summary>Gets the serializer that provides functionality to serialize objects or value types to serialized string and to deserialize serialized string into objects or value types.</summary>
        ISerializer Serializer { get; }

        /// <summary>Gets the logger used to perform logging.</summary>
        ILogger Logger { get; }
    }
}
