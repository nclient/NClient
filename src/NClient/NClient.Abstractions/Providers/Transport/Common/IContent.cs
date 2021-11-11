﻿using System.Text;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    public interface IContent
    {
        /// <summary>
        /// Gets byte representation of response content.
        /// </summary>
        byte[] Bytes { get; }
        
        /// <summary>
        /// Gets response content encoding.
        /// </summary>
        Encoding? Encoding { get; }
        
        /// <summary>
        /// Gets metadata returned by server with the response content.
        /// </summary>
        IMetadataContainer Metadatas { get; }
    }
}
