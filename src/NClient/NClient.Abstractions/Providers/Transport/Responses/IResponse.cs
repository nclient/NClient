using System;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    public interface IResponse<TValue> : IResponse
    {
        /// <summary>
        /// The object obtained as a result of deserialization of the body.
        /// </summary>
        TValue? Data { get; }
    }
    
    public interface IResponse
    {
        /// <summary>
        /// The request that the response belongs to.
        /// </summary>
        IRequest Request { get; }
        /// <summary>
        /// Gets string representation of response content.
        /// </summary>
        IContent Content { get; set; }
        /// <summary>
        /// Gets response status code.
        /// </summary>
        int StatusCode { get; set; }
        /// <summary>
        /// Gets description of status returned.
        /// </summary>
        string? StatusDescription { get; set; }
        /// <summary>
        /// Gets the URL that actually responded to the content (different from request if redirected).
        /// </summary>
        string? Endpoint { get; set; }
        /// <summary>
        /// Gets metadata returned by server with the response.
        /// </summary>
        IMetadataContainer Metadatas { get; set; }
        /// <summary>
        /// Gets error generated while attempting request.
        /// </summary>
        string? ErrorMessage { get; set; }
        /// <summary>
        /// Gets the exception thrown when error is encountered.
        /// </summary>
        Exception? ErrorException { get; set; }
        /// <summary>
        /// Gets the protocol version (1.0, 1.1, etc).
        /// </summary>
        Version? ProtocolVersion { get; set; }
        /// <summary>
        /// Gets information about the success of the request.
        /// </summary>
        bool IsSuccessful { get; set; }
        /// <summary>
        /// Throws an exception if the IsSuccessful property for the response is false.
        /// </summary>
        IResponse EnsureSuccess();
    }
}
