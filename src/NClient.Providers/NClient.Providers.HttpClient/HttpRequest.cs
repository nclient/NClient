using System;
using System.Collections.Generic;
using System.Net.Http;

namespace NClient.Providers.HttpClient
{
    public record HttpParameter(string Name, object Value);
    public record HttpHeader(string Name, string? Value);

    public class HttpRequest
    {
        private readonly List<HttpParameter> _parameters = new();
        private readonly List<HttpHeader> _headers = new();

        public Uri Uri { get; }
        public HttpMethod Method { get; }
        public object? Body { get; set; }
        public IReadOnlyCollection<HttpParameter> Parameters => _parameters;
        public IReadOnlyCollection<HttpHeader> Headers => _headers;

        public HttpRequest(Uri uri, HttpMethod method)
        {
            Uri = uri;
            Method = method;
        }

        public void AddParameter(string name, object value)
        {
            _parameters.Add(new HttpParameter(name, value));
        }

        public void AddHeader(string name, string value)
        {
            _headers.Add(new HttpHeader(name, value));
        }
    }
}
