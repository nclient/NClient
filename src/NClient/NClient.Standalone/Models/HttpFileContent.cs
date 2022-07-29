using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

// ReSharper disable once CheckNamespace
namespace NClient.Models
{
    public class HttpFileContent : IFormFile
    {
        private readonly Stream _stream;
        
        public string Name { get; }
        public string FileName { get; }
        
        public string ContentType { get; }
        public string ContentDisposition { get; }
        public IHeaderDictionary Headers { get; }
        
        public long Length => _stream.Length;

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
        
        public Stream OpenReadStream()
        {
            return _stream;
        }
        
        public void CopyTo(Stream target)
        {
            _stream.CopyTo(target);
        }
        
        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            return _stream.CopyToAsync(target);
        }
        
        private class HeaderDictionary : Dictionary<string, StringValues>, IHeaderDictionary
        {
            public long? ContentLength { get; set; }
        }
    }
}
