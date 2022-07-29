using System.IO;
using System.Text;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Models
{
    public class StreamContent : IStreamContent
    {
        public string? Name { get; }
        public Stream Value { get; }
        public Encoding Encoding { get; }
        public string ContentType { get; }
        
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
