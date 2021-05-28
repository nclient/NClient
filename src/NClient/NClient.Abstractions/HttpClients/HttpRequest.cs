using System;
using System.Collections.Generic;
using System.Net.Http;
using NClient.Common.Helpers;

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
            Ensure.IsNotNull(uri, nameof(uri));
            Ensure.IsNotNull(method, nameof(method));
            
            Id = id;
            Uri = uri;
            Method = method;
        }

        public void AddParameter(string name, object value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));
            Ensure.IsNotNull(value, nameof(value));
            
            _parameters.Add(new HttpParameter(name, value));
        }

        public void AddHeader(string name, string value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));
            Ensure.IsNotNull(value, nameof(value));
            
            _headers.Add(new HttpHeader(name, value));
        }
    }
}
