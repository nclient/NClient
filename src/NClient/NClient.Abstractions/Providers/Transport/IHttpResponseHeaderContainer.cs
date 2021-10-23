using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace NClient.Providers.Transport
{
    public interface IHttpResponseHeaderContainer : IEnumerable<KeyValuePair<string, IEnumerable<string>>>
    {
        ICollection<string> AcceptRanges { get; }
        TimeSpan? Age { get; }
        EntityTagHeaderValue ETag { get; }
        Uri Location { get; }
        ICollection<AuthenticationHeaderValue> ProxyAuthenticate { get; }
        RetryConditionHeaderValue RetryAfter { get; }
        ICollection<ProductInfoHeaderValue> Server { get; }
        ICollection<string> Vary { get; }
        ICollection<AuthenticationHeaderValue> WwwAuthenticate { get; }
        CacheControlHeaderValue CacheControl { get; }
        ICollection<string> Connection { get; }
        bool? ConnectionClose { get; }
        DateTimeOffset? Date { get; }
        ICollection<NameValueHeaderValue> Pragma { get; }
        ICollection<string> Trailer { get; }
        ICollection<TransferCodingHeaderValue> TransferEncoding { get; }
        bool? TransferEncodingChunked { get; }
        ICollection<ProductHeaderValue> Upgrade { get; }
        ICollection<ViaHeaderValue> Via { get; }
        ICollection<WarningHeaderValue> Warning { get; }
    }
}
