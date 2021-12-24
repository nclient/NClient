using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    /// <summary>
    /// Response content.
    /// </summary>
    public class Content : IContent
    {
        // ReSharper disable once ConvertToConstant.Local
        private readonly string _inconsistentErrorMessage = "There is only one content is available in the same time: bytes or stream";
        
        /// <summary>
        /// Gets byte representation of response content.
        /// </summary>
        public byte[] Bytes { get; }
        
        /// <summary>
        /// Gets stream representation of response content
        /// </summary>
        public Stream StreamContent { get; }

        /// <summary>
        /// Gets response content encoding.
        /// </summary>
        public Encoding? Encoding { get; }
        
        /// <summary>
        /// Gets metadata returned by server with the response content.
        /// </summary>
        public IMetadataContainer Metadatas { get; }

        [SuppressMessage("ReSharper", "UnusedVariable")]
        public Content(byte[]? bytes = null, Stream? streamContent = null, string? encoding = null, IMetadataContainer? headerContainer = null)
        {
            if (bytes?.Length > 0 && streamContent?.Length > 0)
                throw new ArgumentOutOfRangeException(nameof(Content), _inconsistentErrorMessage);
            
            Bytes = bytes ?? Array.Empty<byte>();
            StreamContent = streamContent ?? new MemoryStream(Array.Empty<byte>());
            Encoding = string.IsNullOrEmpty(encoding) ? null : Encoding.GetEncoding(encoding);
            Metadatas = headerContainer ?? new MetadataContainer(Array.Empty<IMetadata>());
        }

        /// <summary>
        /// Gets string representation of response content.
        /// </summary>
        public override string ToString()
        {
            try
            {
                return Bytes.Length > 0 ? AsString(Bytes, Encoding ?? Encoding.UTF8) :
                    StreamContent.Length > 0 ? new StreamReader(StreamContent).ReadToEnd() : string.Empty;
            }
            catch (ArgumentException)
            {
                return Bytes.Length > 0 ? AsString(Bytes, Encoding.UTF8) :
                    StreamContent.Length > 0 ? new StreamReader(StreamContent).ReadToEnd() : string.Empty;
            }
        }

        public Stream GetContentAsStream() => Bytes.Length > 0 ? new MemoryStream(Bytes) :
            StreamContent.Length > 0 ? StreamContent : throw new ArgumentOutOfRangeException(nameof(Content), _inconsistentErrorMessage);

        private static string AsString(byte[]? buffer, Encoding encoding) => buffer == null ? string.Empty : encoding.GetString(buffer, 0, buffer.Length);
    }
}
