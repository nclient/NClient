using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    /// <summary>Provides a collection of response content.</summary>
    public class MultipartContent : IContent, IEnumerable<IContent>
    {
        private readonly ICollection<IContent> _nestedContent;

        /// <summary>Gets stream representation of response content.</summary>
        public Stream Stream => throw new NotImplementedException("Use an enumeration instead.");

        /// <summary>Gets response content encoding.</summary>
        public Encoding? Encoding => throw new NotImplementedException("Use an enumeration instead.");
        
        /// <summary>Gets metadata returned by server with the response content.</summary>
        public IMetadataContainer Metadatas => throw new NotImplementedException("Use an enumeration instead.");
        
        public MultipartContent(IEnumerable<IContent> contents)
        {
            _nestedContent = contents.ToArray();
        }
        
        public IEnumerator<IContent> GetEnumerator()
        {
            return _nestedContent.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
