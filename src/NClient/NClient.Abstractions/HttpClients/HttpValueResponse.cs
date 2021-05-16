namespace NClient.Abstractions.HttpClients
{
    public class HttpValueResponse<TValue, TError> : HttpValueResponse<TValue>
    {
        public TError? Error { get; }

        public HttpValueResponse(HttpResponse httpResponse, HttpRequest httpRequest,  TValue? value, TError? error) 
            : base(httpResponse, httpRequest, value)
        {
            Error = error;
        }
    }
    
    public class HttpValueResponse<TValue> : HttpResponse
    {
        public TValue? Value { get; }

        public HttpValueResponse(HttpResponse httpResponse, HttpRequest httpRequest, TValue? value) 
            : base(httpResponse, httpRequest)
        {
            Value = value;
        }
    }
}
