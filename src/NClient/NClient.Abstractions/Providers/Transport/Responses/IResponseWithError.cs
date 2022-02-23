// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    /// <summary>The response containing a response context with a deserialized response body including error.</summary>
    /// <typeparam name="TData">The type of the response body.</typeparam>
    /// <typeparam name="TError">The type of the response body in case of an error.</typeparam>
    public interface IResponseWithError<TData, TError> : IResponseWithError<TError>, IResponse<TData>
    {
        /// <summary>Deconstructs response.</summary>
        /// <param name="data">The object obtained as a result of deserialization of the body. If the request was unsuccessful, the value will be null.</param>
        /// <param name="error">The object obtained as a result of deserialization of the body in case of an error. If the request was successful, the value will be null.</param>
        /// <param name="response">The response containing a response context.</param>
        void Deconstruct(out TData? data, out TError? error, out IResponse response);
    }
    
    /// <summary>The response containing an HTTP context with a deserialized response body in case of an error.</summary>
    /// <typeparam name="TError">The type of the HTTP response body in case of an error.</typeparam>
    public interface IResponseWithError<TError> : IResponse
    {
        /// <summary>The object obtained as a result of deserialization of the body if the IsSuccessful property for the response is false.</summary>
        TError? Error { get; }
        
        /// <summary>Deconstructs response.</summary>
        /// <param name="error">The object obtained as a result of deserialization of the body in case of an error. If the request was successful, the value will be null.</param>
        /// <param name="response">The response containing a response context.</param>
        void Deconstruct(out TError? error, out IResponse response);
    }
}
