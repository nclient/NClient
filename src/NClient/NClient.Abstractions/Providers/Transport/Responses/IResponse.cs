using System;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    /// <summary>The container for response data with deserialized body.</summary>
    public interface IResponse<TValue> : IResponse
    {
        /// <summary>The object obtained as a result of deserialization of the body.</summary>
        TValue? Data { get; }
        
        /// <summary>Deconstructs response.</summary>
        /// <param name="data">The object obtained as a result of deserialization of the body. If the request was unsuccessful, the value will be null.</param>
        /// <param name="response">The response containing a response context.</param>
        void Deconstruct(out TValue? data, out IResponse response);
    }
    
    /// <summary>The container for response data.</summary>
    public interface IResponse
    {
        /// <summary>The request that the response belongs to.</summary>
        IRequest Request { get; }
        
        /// <summary>Gets string representation of response content.</summary>
        IContent Content { get; }
        
        /// <summary>Gets response status code.</summary>
        int StatusCode { get; }
        
        /// <summary>Gets description of status returned.</summary>
        string? StatusDescription { get; }
        
        /// <summary>Gets the URL that actually responded to the content (different from request if redirected).</summary>
        string? Endpoint { get; }
        
        /// <summary>Gets metadata returned by server with the response.</summary>
        IMetadataContainer Metadatas { get; }
        
        /// <summary>Gets error generated while attempting request.</summary>
        string? ErrorMessage { get; }
        
        /// <summary>Gets the exception thrown when error is encountered.</summary>
        Exception? ErrorException { get; }
        
        /// <summary>Gets the protocol version (1.0, 1.1, etc).</summary>
        Version? ProtocolVersion { get; }
        
        /// <summary>Gets information about the success of the request.</summary>
        bool IsSuccessful { get; }
        
        /// <summary>Throws an exception if the IsSuccessful property for the response is false.</summary>
        IResponse EnsureSuccess();
    }
}
