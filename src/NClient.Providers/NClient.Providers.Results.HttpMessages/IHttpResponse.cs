namespace NClient.Providers.Results.HttpMessages
{
    public interface IHttpResponse<TValue> : IHttpResponse
    {
        /// <summary>
        /// The object obtained as a result of deserialization of the body.
        /// </summary>
        TValue? Data { get; }
    }
}
