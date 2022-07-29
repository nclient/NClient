using System.IO;
using System.Text;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Models
{
    /// <summary>Represents a stream content uploaded/downloaded with a request.</summary>
    public class StreamContent : IStreamContent
    {
        /// <summary>Gets the content name.</summary>
        public string? Name { get; }
        
        /// <summary>Gets the stream for reading the uploaded/downloaded content.</summary>
        public Stream Value { get; }
        
        /// <summary>Gets the encoding type for reading the uploaded/downloaded content.</summary>
        public Encoding Encoding { get; }
        
        /// <summary>Gets the raw content type of the uploaded/downloaded content.</summary>
        public string ContentType { get; }
        
        /// <summary>Initializes a stream content.</summary>
        /// <param name="value">The content name.</param>
        /// <param name="encoding">The stream for reading the content.</param>
        /// <param name="contentType">The encoding type for reading the content.</param>
        /// <param name="name">The raw content type of the content.</param>
        public StreamContent(Stream value, Encoding encoding, string contentType, string? name = null)
        {
            Ensure.IsNotNull(value, nameof(value));

            Name = name;
            Value = value;
            Encoding = encoding;
            ContentType = contentType;
        }
    }
}
