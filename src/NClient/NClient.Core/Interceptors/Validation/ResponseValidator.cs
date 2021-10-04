using NClient.Abstractions.Resilience;

namespace NClient.Core.Interceptors.Validation
{
    public interface IResponseValidator<TRequest, TResponse>
    {
        ResponseContext<TRequest, TResponse> Ensure(ResponseContext<TRequest, TResponse> responseContext);
    }
    
    public class ResponseValidator<TRequest, TResponse> : IResponseValidator<TRequest, TResponse>
    {
        private readonly IResponseValidatorSettings<TRequest, TResponse> _settings;
        
        public ResponseValidator(IResponseValidatorSettings<TRequest, TResponse> settings)
        {
            _settings = settings;
        }

        public ResponseContext<TRequest, TResponse> Ensure(ResponseContext<TRequest, TResponse> responseContext)
        {
            if (_settings.SuccessCondition(responseContext))
                return responseContext;
            
            _settings.OnFailure(responseContext);
            return responseContext;
        }
    }
}
