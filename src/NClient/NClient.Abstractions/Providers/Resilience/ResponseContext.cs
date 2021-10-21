namespace NClient.Abstractions.Providers.Resilience
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
