using System;
using System.Collections.Generic;
using System.Net.Http;
using NClient.Common.Helpers;

namespace NClient.Providers.Transport
{
    /// <summary>
    /// The container for data used to make requests.
    /// </summary>
    public class HttpRequest : IHttpRequest
    {
        private readonly List<IHttpParameter> _parameters = new();
        private readonly List<IHttpHeader> _headers = new();

        /// <summary>
        /// Gets the request id.
        /// </summary>
        public Guid Id { get; }
        /// <summary>
        /// Gets the <see cref="T:System.Uri" /> used for the HTTP request. Should not include query.
        /// </summary>
        public Uri Resource { get; }
        /// <summary>
        /// Gets HTTP method type.
        /// </summary>
        public HttpMethod Method { get; }
        /// <summary>
        /// Gets object used for request body.
        /// </summary>
        public object? Data { get; set; }
        /// <summary>
        /// Gets string representation of request body.
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// Gets collection of URI parameters.
        /// </summary>
        public IReadOnlyCollection<IHttpParameter> Parameters => _parameters;
        /// <summary>
        /// Gets collection of HTTP headers.
        /// </summary>
        public IReadOnlyCollection<IHttpHeader> Headers => _headers;

        /// <summary>
        /// Creates container for HTTP request data.
        /// </summary>
        /// <param name="id">The request id.</param>
        /// <param name="resource">The request URI (without parameters).</param>
        /// <param name="method">The request HTTP method type.</param>
        public HttpRequest(Guid id, Uri resource, HttpMethod method)
        {
            Ensure.IsNotNull(resource, nameof(resource));
            Ensure.IsNotNull(method, nameof(method));

            Id = id;
            Resource = resource;
            Method = method;
        }

        /// <summary>
        /// Adds URI parameter.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        public void AddParameter(string name, object value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));
            Ensure.IsNotNull(value, nameof(value));

            _parameters.Add(new HttpParameter(name, value));
        }

        /// <summary>
        /// Adds HTTP header.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        public void AddHeader(string name, string value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));
            Ensure.IsNotNull(value, nameof(value));

            _headers.Add(new HttpHeader(name, value));
        }
    }
}
