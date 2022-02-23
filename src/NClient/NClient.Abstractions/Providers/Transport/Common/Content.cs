using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    /// <summary>Response content.</summary>
    public class Content : IContent
    {
        /// <summary>Gets byte representation of response content.</summary>
        public byte[] Bytes { get; }
        
        /// <summary>Gets response content encoding.</summary>
        public Encoding? Encoding { get; }
        
        /// <summary>Gets metadata returned by server with the response content.</summary>
        public IMetadataContainer Metadatas { get; }
        
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public Content(byte[]? bytes = null, string? encoding = null, IMetadataContainer? headerContainer = null)
        {
            Bytes = bytes ?? Array.Empty<byte>();
            Encoding = string.IsNullOrEmpty(encoding) ? null : Encoding.GetEncoding(encoding);
            Metadatas = headerContainer ?? new MetadataContainer(Array.Empty<IMetadata>());
        }

        /// <summary>Gets string representation of response content.</summary>
        public override string ToString()
        {
            try
            {
                return AsString(Bytes, Encoding ?? Encoding.UTF8);
            }
            catch (ArgumentException)
            {
                return AsString(Bytes, Encoding.UTF8);
            }
        }

        private static string AsString(byte[]? buffer, Encoding encoding)
        {
            return buffer == null ? "" : encoding.GetString(buffer, 0, buffer.Length);
        }
    }
}
