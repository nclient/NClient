using System.IO;
using System.Text;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    public interface IContent
    {
        /// <summary>
        /// Gets stream representation of response content
        /// </summary>
        Stream StreamContent { get; }
        
        /// <summary>
        /// Gets response content encoding.
        /// </summary>
        Encoding Encoding { get; }
        
        /// <summary>
        /// Gets metadata returned by server with the response content.
        /// </summary>
        IMetadataContainer Metadatas { get; }
    }
}
