using System;
using System.Collections.Generic;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    /// <summary>The container for data used to make requests.</summary>
    public class Request : IRequest
    {
        private bool _disposed;
        private readonly IEnumerable<IDisposable> _disposeWith;
        private readonly List<IParameter> _parameters = new();

        /// <summary>Gets the request id.</summary>
        public Guid Id { get; }
        
        /// <summary>Gets the endpoint used for the request.</summary>
        public string Endpoint { get; }
        
        /// <summary>Gets request type.</summary>
        public RequestType Type { get; }
        
        /// <summary>Gets string representation of request body.</summary>
        public IContent? Content { get; set; }
        
        /// <summary>Gets collection of URI parameters.</summary>
        public IReadOnlyCollection<IParameter> Parameters => _parameters;
        
        /// <summary>Gets collection of metadata.</summary>
        public IMetadataContainer Metadatas { get; }

        /// <summary>Initializes container for request data.</summary>
        /// <param name="id">The request id.</param>
        /// <param name="endpoint">The request URI (without parameters).</param>
        /// <param name="requestType">The request type.</param>
        /// <param name="disposeWith">Objects to be disposed along with the request.</param>
        public Request(Guid id, string endpoint, RequestType requestType, IEnumerable<IDisposable>? disposeWith = null)
        {
            Ensure.IsNotNull(endpoint, nameof(endpoint));
            Ensure.IsNotNull(requestType, nameof(requestType));

            Id = id;
            Endpoint = endpoint;
            Type = requestType;
            Metadatas = new MetadataContainer();
            
            _disposeWith = disposeWith ?? Array.Empty<IDisposable>();
        }

        /// <summary>Adds URI parameter.</summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        public void AddParameter(string name, object value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));
            Ensure.IsNotNull(value, nameof(value));

            _parameters.Add(new Parameter(name, value));
        }

        /// <summary>Adds header.</summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        public void AddMetadata(string name, string value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));
            Ensure.IsNotNull(value, nameof(value));

            Metadatas.Add(new Metadata(name, value));
        }
        
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                Content?.Stream.Dispose();
                foreach (var disposable in _disposeWith)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
