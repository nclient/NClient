// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    /// <summary>
    /// The container for response data with deserialized body including error.
    /// </summary>
    public class ResponseWithError<TData, TError> : Response<TData>, IResponseWithError<TData, TError>
    {
        /// <summary>
        /// The object obtained as a result of deserialization of the body if the IsSuccessful property for the response is false.
        /// </summary>
        public TError? Error { get; }

        /// <summary>
        /// Creates the container for response data.
        /// </summary>
        /// <param name="response">The response used as base response.</param>
        /// <param name="request">The request that the response belongs to.</param>
        /// <param name="data">The object obtained as a result of deserialization of the body.</param>
        /// <param name="error">The object obtained as a result of deserialization of the body if the IsSuccessful property for the response is false.</param>
        public ResponseWithError(IResponse response, IRequest request, TData? data, TError? error)
            : base(response, request, data)
        {
            Error = error;
        }

        /// <summary>
        /// Throws an exception if the IsSuccessful property for the response is false.
        /// </summary>
        public new ResponseWithError<TData, TError> EnsureSuccess()
        {
            base.EnsureSuccess();
            return this;
        }
    }

    /// <summary>
    /// The container for response data with deserialized body error.
    /// </summary>
    public class ResponseWithError<TError> : Response, IResponseWithError<TError>
    {
        /// <summary>
        /// The object obtained as a result of deserialization of the body if the IsSuccessful property for the response is false.
        /// </summary>
        public TError? Error { get; }

        /// <summary>
        /// Creates the container for response data.
        /// </summary>
        /// <param name="response">The response used as base response.</param>
        /// <param name="request">The request that the response belongs to.</param>
        /// <param name="error">The object obtained as a result of deserialization of the body if the IsSuccessful property for the response is false.</param>
        public ResponseWithError(IResponse response, IRequest request, TError? error)
            : base(response, request)
        {
            Error = error;
        }

        /// <summary>
        /// Throws an exception if the IsSuccessful property for the response is false.
        /// </summary>
        public new ResponseWithError<TError> EnsureSuccess()
        {
            base.EnsureSuccess();
            return this;
        }
    }
}
