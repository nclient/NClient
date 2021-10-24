﻿using System;
using System.Collections.Generic;

namespace NClient.Providers.Transport
{
    public interface IRequest
    {
        /// <summary>
        /// Gets the request id.
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// Gets the <see cref="T:System.Uri" /> used for the request. Should not include query.
        /// </summary>
        Uri Resource { get; }
        /// <summary>
        /// Gets HTTP method type.
        /// </summary>
        RequestType? Method { get; }
        /// <summary>
        /// Gets object used for request body.
        /// </summary>
        object? Data { get; set; }
        /// <summary>
        /// Gets string representation of request body.
        /// </summary>
        string? Content { get; set; }
        /// <summary>
        /// Gets collection of URI parameters.
        /// </summary>
        IReadOnlyCollection<IParameter> Parameters { get; }
        /// <summary>
        /// Gets collection of HTTP headers.
        /// </summary>
        IReadOnlyCollection<IHeader> Headers { get; }
        /// <summary>
        /// Adds URI parameter.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        void AddParameter(string name, object value);
        /// <summary>
        /// Adds HTTP header.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        void AddHeader(string name, string value);
    }
}