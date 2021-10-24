// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    public interface IResponseWithError<TValue, TError> : IResponseWithError<TError>, IResponse<TValue>
    {
    }
    
    public interface IResponseWithError<TError> : IResponse
    {
        /// <summary>
        /// The object obtained as a result of deserialization of the body if the IsSuccessful property for the HTTP response is false.
        /// </summary>
        TError? Error { get; }
    }
}
