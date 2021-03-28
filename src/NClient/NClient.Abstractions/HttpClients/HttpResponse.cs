using System;
using System.Net;

namespace NClient.Abstractions.HttpClients
{
    public class HttpResponse<T> : HttpResponse
    {
        public T Value { get; }

        public HttpResponse(T value)
        {
            Value = value;
        }

        public HttpResponse(HttpResponse httpResponse, T value)
        {
            Value = value;
            ContentType = httpResponse.ContentType;
            ContentLength = httpResponse.ContentLength;
            ContentEncoding = httpResponse.ContentEncoding;
            Content = httpResponse.Content;
            StatusCode = httpResponse.StatusCode;
            StatusDescription = httpResponse.StatusDescription;
            RawBytes = httpResponse.RawBytes;
            ResponseUri = httpResponse.ResponseUri;
            Server = httpResponse.Server;
            Headers = httpResponse.Headers;
            ErrorMessage = httpResponse.ErrorMessage;
            ErrorException = httpResponse.ErrorException;
            ProtocolVersion = httpResponse.ProtocolVersion;
        }
    }

    public class HttpResponse
    {
        public string? ContentType { get; set; }
        public long ContentLength { get; set; }
        public string? ContentEncoding { get; set; }
        public string? Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? StatusDescription { get; set; }
        public byte[]? RawBytes { get; set; }
        public Uri? ResponseUri { get; set; }
        public string? Server { get; set; }
        public HttpHeader[]? Headers { get; set; }
        public string? ErrorMessage { get; set; }
        public Exception? ErrorException { get; set; }
        public Version? ProtocolVersion { get; set; }

        public bool IsSuccessful => (int)StatusCode >= 200 && (int)StatusCode <= 299;
    }
}
