namespace NClient.Abstractions.HttpClients
{
    public interface IHttpResponseContent
    {
        /// <summary>
        /// Gets byte representation of response content.
        /// </summary>
        byte[] Bytes { get; }
        /// <summary>
        /// Gets headers returned by server with the response content.
        /// </summary>
        HttpResponseContentHeaderContainer Headers { get; }
    }
}
