// ReSharper disable once CheckNamespace

namespace NClient.Providers.Transport
{
    public class ResponseContext<TRequest, TResponse> : IResponseContext<TRequest, TResponse>
    {
        public TRequest Request { get; }
        public TResponse Response { get; }

        public ResponseContext(TRequest request, TResponse response)
        {
            Request = request;
            Response = response;
        }
    }
}
