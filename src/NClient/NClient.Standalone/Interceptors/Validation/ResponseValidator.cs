using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Resilience;

namespace NClient.Standalone.Interceptors.Validation
{
    internal interface IResponseValidator<TRequest, TResponse>
    {
        ResponseContext<TRequest, TResponse> Ensure(ResponseContext<TRequest, TResponse> responseContext);
    }
    
    internal class ResponseValidator<TRequest, TResponse> : IResponseValidator<TRequest, TResponse>
    {
        private readonly IEnsuringSettings<TRequest, TResponse> _ensuringSettings;
        
        public ResponseValidator(IEnsuringSettings<TRequest, TResponse> ensuringSettings)
        {
            _ensuringSettings = ensuringSettings;
        }

        public ResponseContext<TRequest, TResponse> Ensure(ResponseContext<TRequest, TResponse> responseContext)
        {
            if (_ensuringSettings.IsSuccess(responseContext))
                return responseContext;
            
            _ensuringSettings.OnFailure(responseContext);
            return responseContext;
        }
    }
}
