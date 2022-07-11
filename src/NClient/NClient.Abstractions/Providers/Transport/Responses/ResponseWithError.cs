// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    /// <summary>The container for response data with deserialized body including error.</summary>
    public class ResponseWithError<TData, TError> : Response<TData>, IResponseWithError<TData, TError>
    {
        /// <summary>The object obtained as a result of deserialization of the body if the IsSuccessful property for the response is false.</summary>
        public TError? Error { get; }

        /// <summary>Initializes the container for response data.</summary>
        /// <param name="response">The response used as base response.</param>
        /// <param name="request">The request that the response belongs to.</param>
        /// <param name="data">The object obtained as a result of deserialization of the body.</param>
        /// <param name="error">The object obtained as a result of deserialization of the body if the IsSuccessful property for the response is false.</param>
        public ResponseWithError(IResponse response, IRequest request, TData? data, TError? error)
            : base(response, request, data)
        {
            Error = error;
        }

        /// <summary>Throws an exception if the IsSuccessful property for the response is false.</summary>
        public new ResponseWithError<TData, TError> EnsureSuccess()
        {
            base.EnsureSuccess();
            return this;
        }

        /// <summary>Deconstructs response.</summary>
        /// <param name="data">The object obtained as a result of deserialization of the body. If the request was unsuccessful, the value will be null.</param>
        /// <param name="error">The object obtained as a result of deserialization of the body in case of an error. If the request was successful, the value will be null.</param>
        /// <param name="response">The response containing a response context.</param>
        public void Deconstruct(out TData? data, out TError? error, out IResponse response)
        {
            data = Data;
            error = Error;
            response = this;
        }
        
        /// <summary>Deconstructs response.</summary>
        /// <param name="error">The object obtained as a result of deserialization of the body in case of an error. If the request was successful, the value will be null.</param>
        /// <param name="response">The response containing a response context.</param>
        void IResponseWithError<TError>.Deconstruct(out TError? error, out IResponse response)
        {
            error = Error;
            response = this;
        }
    }

    /// <summary>The container for response data with deserialized body error.</summary>
    public class ResponseWithError<TError> : Response, IResponseWithError<TError>
    {
        /// <summary>The object obtained as a result of deserialization of the body if the IsSuccessful property for the response is false.</summary>
        public TError? Error { get; }

        /// <summary>Initializes the container for response data.</summary>
        /// <param name="response">The response used as base response.</param>
        /// <param name="request">The request that the response belongs to.</param>
        /// <param name="error">The object obtained as a result of deserialization of the body if the IsSuccessful property for the response is false.</param>
        public ResponseWithError(IResponse response, IRequest request, TError? error) 
            : base(response, request)
        {
            Error = error;
        }

        /// <summary>Throws an exception if the IsSuccessful property for the response is false.</summary>
        public new ResponseWithError<TError> EnsureSuccess()
        {
            base.EnsureSuccess();
            return this;
        }
        
        /// <summary>Deconstructs response.</summary>
        /// <param name="error">The object obtained as a result of deserialization of the body in case of an error. If the request was successful, the value will be null.</param>
        /// <param name="response">The response containing a response context.</param>
        public void Deconstruct(out TError? error, out IResponse response)
        {
            error = Error;
            response = this;
        }
    }
}
