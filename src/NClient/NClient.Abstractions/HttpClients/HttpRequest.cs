using System;
using System.Collections.Generic;
using System.Net.Http;

namespace NClient.Abstractions.HttpClients
{
    public class HttpRequest
    {
        private readonly List<HttpParameter> _parameters = new();
        private readonly List<HttpHeader> _headers = new();

        public Guid Id { get; }
        public Uri Uri { get; }
        public HttpMethod Method { get; }
        public object? Body { get; set; }
        public IReadOnlyCollection<HttpParameter> Parameters => _parameters;
        public IReadOnlyCollection<HttpHeader> Headers => _headers;

        public HttpRequest(Guid id, Uri uri, HttpMethod method)
        {
            Id = id;
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
