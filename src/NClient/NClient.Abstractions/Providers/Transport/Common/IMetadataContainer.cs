using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    public interface IMetadataContainer : IEnumerable<KeyValuePair<string, IEnumerable<IMetadata>>>
    {
        IEnumerable<IMetadata> Get(string name);
        void Add(IMetadata metadata);
    }
}
