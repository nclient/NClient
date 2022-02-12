using System;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    /// <summary>
    /// The container for response data with deserialized body.
    /// </summary>
    public class Response<TData> : Response, IResponse<TData>
    {
        /// <summary>
        /// The object obtained as a result of deserialization of the body.
        /// </summary>
        public TData? Data { get; }

        /// <summary>
        /// Creates the container for response data.
        /// </summary>
        /// <param name="response">The response used as base response.</param>
        /// <param name="request">The request that the response belongs to.</param>
        /// <param name="data">The object obtained as a result of deserialization of the body.</param>
        public Response(IResponse response, IRequest request, TData? data)
            : base(response, request)
        {
            Data = data;
        }

        /// <summary>
        /// Throws an exception if the IsSuccessful property for the response is false.
        /// </summary>
        public new Response<TData> EnsureSuccess()
        {
            base.EnsureSuccess();
            return this;
        }

        public void Deconstruct(out TData? data, out IResponse response)
        {
            data = Data;
            response = this;
        }
    }

    /// <summary>
    /// The container for response data.
    /// </summary>
    public class Response : IResponse
    {
        /// <summary>
        /// The request that the response belongs to.
        /// </summary>
        public IRequest Request { get; }
        /// <summary>
        /// Gets string representation of response content.
        /// </summary>
        public IContent Content { get; set; }
        /// <summary>
        /// Gets response status code.
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Gets description of status returned.
        /// </summary>
        public string? StatusDescription { get; set; }
        /// <summary>
        /// Gets the endpoint that actually responded to the content (different from request if redirected).
        /// </summary>
        public string? Endpoint { get; set; }
        /// <summary>
        /// Gets metadata returned by server with the response.
        /// </summary>
        public IMetadataContainer Metadatas { get; set; }
        /// <summary>
        /// Gets error generated while attempting request.
        /// </summary>
        public string? ErrorMessage { get; set; }
        /// <summary>
        /// Gets the exception thrown when error is encountered.
        /// </summary>
        public Exception? ErrorException { get; set; }
        /// <summary>
        /// Gets the protocol version (1.0, 1.1, etc).
        /// </summary>
        public Version? ProtocolVersion { get; set; }

        /// <summary>
        /// Gets information about the success of the request.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Creates the container for response data.
        /// </summary>
        /// <param name="transportRequest">The request that the response belongs to.</param>
        public Response(IRequest transportRequest)
        {
            Ensure.IsNotNull(transportRequest, nameof(transportRequest));
            
            Request = transportRequest;
            Content = new Content();
            Metadatas = new MetadataContainer(Array.Empty<IMetadata>());
        }

        internal Response(IResponse response, IRequest request) : this(request)
        {
            Ensure.IsNotNull(response, nameof(response));
            
            Content = response.Content;
            StatusCode = response.StatusCode;
            StatusDescription = response.StatusDescription;
            Endpoint = response.Endpoint;
            Metadatas = response.Metadatas;
            ErrorMessage = response.ErrorMessage;
            ErrorException = response.ErrorException;
            ProtocolVersion = response.ProtocolVersion;
            IsSuccessful = response.IsSuccessful;
        }

        /// <summary>
        /// Throws an exception if the IsSuccessful property for the response is false.
        /// </summary>
        public IResponse EnsureSuccess()
        {
            if (!IsSuccessful)
                throw ErrorException!;
            return this;
        }
    }
}
