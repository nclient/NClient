// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    public interface IResponseWithError<TValue, TError> : IResponseWithError<TError>, IResponse<TValue>
    {
        void Deconstruct(out TValue? data, out TError? error);
    }
    
    public interface IResponseWithError<TError> : IResponse
    {
        /// <summary>The object obtained as a result of deserialization of the body if the IsSuccessful property for the response is false.</summary>
        TError? Error { get; }
    }
}
