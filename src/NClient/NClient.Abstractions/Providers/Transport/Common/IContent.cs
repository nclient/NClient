using System.IO;
using System.Text;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    /// <summary>Response content.</summary>
    public interface IContent
    {
        /// <summary>Gets stream representation of response content</summary>
        Stream Stream { get; }
        
        /// <summary>Gets response content encoding.</summary>
        Encoding Encoding { get; }
        
        /// <summary>Gets metadata returned by server with the response content.</summary>
        IMetadataContainer Metadatas { get; }
    }
}
