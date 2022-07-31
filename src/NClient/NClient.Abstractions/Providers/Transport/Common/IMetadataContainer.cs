using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    /// <summary>Represents the collection of additional information of a request or response.</summary>
    public interface IMetadataContainer : IEnumerable<KeyValuePair<string, IEnumerable<IMetadata>>>
    {
        /// <summary>Returns metadata by name.</summary>
        /// <param name="name">The metadata name.</param>
        IEnumerable<IMetadata> Get(string name);
        
        /// <summary>Returns metadata by name.</summary>
        /// <param name="name">The metadata name.</param>
        IEnumerable<IMetadata> GetValueOrDefault(string name);
        
        /// <summary>Adds metadata to the collection.</summary>
        /// <param name="metadata">The metadata.</param>
        void Add(IMetadata metadata);
    }
}
