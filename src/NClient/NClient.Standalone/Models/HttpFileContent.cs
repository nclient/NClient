using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

// ReSharper disable once CheckNamespace
namespace NClient.Models
{
    /// <summary>Represents a file uploaded/downloaded with an HTTP request.</summary>
    public class HttpFileContent : IFormFile
    {
        private readonly Stream _stream;
        
        /// <summary>Gets the form field name from the Content-Disposition header.</summary>
        public string Name { get; }
        
        /// <summary>Gets the file name from the Content-Disposition header.</summary>
        public string FileName { get; }
        
        /// <summary>Gets the raw Content-Type header of the uploaded/downloaded file.</summary>
        public string ContentType { get; }
        
        /// <summary>Gets the raw Content-Disposition header of the uploaded/downloaded file.</summary>
        public string ContentDisposition { get; }
        
        /// <summary>Gets the header dictionary of the uploaded/downloaded file.</summary>
        public IHeaderDictionary Headers { get; }
        
        /// <summary>Gets the file length in bytes.</summary>
        public long Length => _stream.Length;

        /// <summary>Initializes a file content.</summary>
        /// <param name="stream">The stream with file content.</param>
        /// <param name="name">The form field name from the Content-Disposition header.</param>
        /// <param name="fileName">The file name from the Content-Disposition header.</param>
        /// <param name="contentType">The raw Content-Type header.</param>
        /// <param name="contentDisposition">The raw Content-Disposition header.</param>
        /// <param name="headerDictionary">The header dictionary.</param>
        public HttpFileContent(
            Stream stream,
            string name,
            string fileName,
            string contentType,
            string contentDisposition,
            IHeaderDictionary? headerDictionary = null)
        {
            Name = name;
            FileName = fileName;
            ContentType = contentType;
            ContentDisposition = contentDisposition;
            Headers = headerDictionary ?? new HeaderDictionary();
            _stream = stream;
        }
        
        /// <summary>Opens the request stream for reading the uploaded/downloaded file.</summary>
        public Stream OpenReadStream()
        {
            return _stream;
        }
        
        /// <summary>Copies the contents of the uploaded/downloaded file to the <paramref name="target"/> stream.</summary>
        /// <param name="target">The stream to copy the file contents to.</param>
        public void CopyTo(Stream target)
        {
            _stream.CopyTo(target);
        }
        
        /// <summary>Asynchronously copies the contents of the uploaded/downloaded file to the <paramref name="target"/> stream.</summary>
        /// <param name="target">The stream to copy the file contents to.</param>
        /// <param name="cancellationToken"></param>
        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            #if NETSTANDARD2_0 || NETFRAMEWORK
            return _stream.CopyToAsync(target);
            #else
            return _stream.CopyToAsync(target, cancellationToken);
            #endif
        }
        
        #pragma warning disable CS8644
        private class HeaderDictionary : Dictionary<string, StringValues>, IHeaderDictionary
        {
            public long? ContentLength { get; set; }
        }
        #pragma warning restore CS8644
    }
}
