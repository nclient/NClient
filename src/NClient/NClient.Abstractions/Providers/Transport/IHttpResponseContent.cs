// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
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
        IHttpResponseContentHeaderContainer Headers { get; }
    }
}
