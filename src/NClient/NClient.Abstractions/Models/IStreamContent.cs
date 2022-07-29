using System.IO;
using System.Text;

namespace NClient.Models
{
    /// <summary>Represents a stream content uploaded/downloaded with a request.</summary>
    public interface IStreamContent
    {
        /// <summary>Gets the content name.</summary>
        string? Name { get; }
        
        /// <summary>Gets the stream for reading the uploaded/downloaded content.</summary>
        Stream Value { get; }
        
        /// <summary>Gets the encoding type for reading the uploaded/downloaded content.</summary>
        public Encoding Encoding { get; }
        
        /// <summary>Gets the raw content type of the uploaded/downloaded content.</summary>
        string ContentType { get; }
    }
}
