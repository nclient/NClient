using System;
using System.Collections.Generic;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    /// <summary>
    /// The container for data used to make requests.
    /// </summary>
    public class Request : IRequest
    {
        private readonly List<IParameter> _parameters = new();
        private readonly List<IHeader> _headers = new();

        /// <summary>
        /// Gets the request id.
        /// </summary>
        public Guid Id { get; }
        /// <summary>
        /// Gets the <see cref="T:System.Uri" /> used for the request. Should not include query.
        /// </summary>
        public Uri Resource { get; }
        /// <summary>
        /// Gets request type.
        /// </summary>
        public RequestType Type { get; }
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
        public IReadOnlyCollection<IParameter> Parameters => _parameters;
        /// <summary>
        /// Gets collection of headers.
        /// </summary>
        public IReadOnlyCollection<IHeader> Headers => _headers;

        /// <summary>
        /// Creates container for request data.
        /// </summary>
        /// <param name="id">The request id.</param>
        /// <param name="resource">The request URI (without parameters).</param>
        /// <param name="requestType">The request type.</param>
        public Request(Guid id, Uri resource, RequestType requestType)
        {
            Ensure.IsNotNull(resource, nameof(resource));
            Ensure.IsNotNull(requestType, nameof(requestType));

            Id = id;
            Resource = resource;
            Type = requestType;
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

            _parameters.Add(new Parameter(name, value));
        }

        /// <summary>
        /// Adds header.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        public void AddHeader(string name, string value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));
            Ensure.IsNotNull(value, nameof(value));

            _headers.Add(new Header(name, value));
        }
    }
}
