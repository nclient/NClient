using System;
using System.Net;

namespace NClient.Providers.HttpClient
{
    public class HttpResponse<T> : HttpResponse
    {
        public T Value { get; }

        public HttpResponse(HttpStatusCode statusCode, T value) : base(statusCode)
        {
            Value = value;
        }

        public HttpResponse(HttpResponse httpResponse, T value) : base(httpResponse.StatusCode)
        {
            Content = httpResponse.Content;
            ContentEncoding = httpResponse.ContentEncoding;
            ErrorMessage = httpResponse.ErrorMessage;
            ErrorException = httpResponse.ErrorException;
            Value = value;
        }
    }

    public class HttpResponse
    {
        public HttpStatusCode StatusCode { get; init; }
        public string? Content { get; init; }
        public string? ContentEncoding { get; init; }
        public string? ErrorMessage { get; init; }
        public Exception? ErrorException { get; init; }

        public bool IsSuccessful => (int)StatusCode >= 200 && (int)StatusCode <= 299;

        public HttpResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
