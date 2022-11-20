using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    /// <summary>Response content.</summary>
    public class Content : IContent
    {
        /// <summary>Gets stream representation of response content.</summary>
        public Stream Stream { get; }

        /// <summary>Gets response content encoding.</summary>
        public Encoding? Encoding { get; }
        
        /// <summary>Gets metadata returned by server with the response content.</summary>
        public IMetadataContainer Metadatas { get; }
        
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public Content(Stream? streamContent = null, string? encoding = null, IMetadataContainer? headerContainer = null)
        {
            Stream = streamContent ?? new MemoryStream(Array.Empty<byte>());
            Encoding = string.IsNullOrEmpty(encoding) ? null : Encoding.GetEncoding(encoding);
            Metadatas = headerContainer ?? new MetadataContainer(Array.Empty<IMetadata>());
        }
    }
}
