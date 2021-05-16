namespace NClient.Abstractions.HttpClients
{
    public class HttpResponseWithError<TValue, TError> : HttpResponse<TValue>
    {
        public TError? Error { get; }

        public HttpResponseWithError(HttpResponse httpResponse, HttpRequest httpRequest, TValue? value, TError? error)
            : base(httpResponse, httpRequest, value)
        {
            Error = error;
        }
    }

    public class HttpResponseWithError<TError> : HttpResponse
    {
        public TError? Error { get; }

        public HttpResponseWithError(HttpResponse httpResponse, HttpRequest httpRequest, TError? error)
            : base(httpResponse, httpRequest)
        {
            Error = error;
        }
    }
}
