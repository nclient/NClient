using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    public interface IContentHeaderContainer : IEnumerable<KeyValuePair<string, IEnumerable<string>>>
    {
        ICollection<string> Allow { get; }
        ContentDispositionHeaderValue ContentDisposition { get; }
        ICollection<string> ContentEncoding { get; }
        ICollection<string> ContentLanguage { get; }
        long? ContentLength { get; }
        Uri ContentLocation { get; }
        byte[] ContentMD5 { get; }
        ContentRangeHeaderValue ContentRange { get; }
        MediaTypeHeaderValue ContentType { get; }
        DateTimeOffset? Expires { get; }
        DateTimeOffset? LastModified { get; }
    }
}
