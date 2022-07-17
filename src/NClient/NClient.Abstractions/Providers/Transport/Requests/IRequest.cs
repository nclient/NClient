using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    /// <summary>The container for data used to make requests.</summary>
    public interface IRequest : IDisposable
    {
        /// <summary>Gets the request id.</summary>
        Guid Id { get; }
        
        /// <summary>Gets the resource used for the request. Should not include query.</summary>
        Uri Resource { get; }
        
        /// <summary>Gets request type.</summary>
        RequestType Type { get; }
        
        /// <summary>Gets string representation of request body.</summary>
        IContent? Content { get; }
        
        /// <summary>Gets collection of URI parameters.</summary>
        IReadOnlyCollection<IParameter> Parameters { get; }
        
        /// <summary>Gets collection of metadata.</summary>
        IMetadataContainer Metadatas { get; }
        
        /// <summary>Adds URI parameter.</summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        void AddParameter(string name, object value);
        
        /// <summary>Adds header.</summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        void AddMetadata(string name, string value);
    }
}
