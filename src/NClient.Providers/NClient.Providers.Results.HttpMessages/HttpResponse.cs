using NClient.Abstractions.HttpClients;

namespace NClient.Providers.Results.HttpMessages
{
    /// <summary>
    /// The container for HTTP response data with deserialized body.
    /// </summary>
    public class HttpResponse<TData> : HttpResponse, IHttpResponse<TData>
    {
        /// <summary>
        /// The object obtained as a result of deserialization of the body.
        /// </summary>
        public TData? Data { get; }

        /// <summary>
        /// Creates the container for HTTP response data.
        /// </summary>
        /// <param name="httpResponse">The HTTP response used as base HTTP response.</param>
        /// <param name="httpRequest">The HTTP request that the response belongs to.</param>
        /// <param name="data">The object obtained as a result of deserialization of the body.</param>
        public HttpResponse(IHttpResponse httpResponse, IHttpRequest httpRequest, TData? data)
            : base(httpResponse, httpRequest)
        {
            Data = data;
        }

        /// <summary>
        /// Throws an exception if the IsSuccessful property for the HTTP response is false.
        /// </summary>
        public new HttpResponse<TData> EnsureSuccess()
        {
            base.EnsureSuccess();
            return this;
        }
    }
}
