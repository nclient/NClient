namespace NClient.Abstractions.Providers.HttpClient
{
    public interface IHttpResponseWithError<TValue, TError> : IHttpResponseWithError<TError>, IHttpResponse<TValue>
    {
    }
    
    public interface IHttpResponseWithError<TError> : IHttpResponse
    {
        /// <summary>
        /// The object obtained as a result of deserialization of the body if the IsSuccessful property for the HTTP response is false.
        /// </summary>
        TError? Error { get; }
    }
}
