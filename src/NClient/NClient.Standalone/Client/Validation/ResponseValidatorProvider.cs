using NClient.Providers;
using NClient.Providers.Validation;

namespace NClient.Standalone.Client.Validation
{
    public class ResponseValidatorProvider<TRequest, TResponse> : IResponseValidatorProvider<TRequest, TResponse>
    {
        private readonly IResponseValidator<TRequest, TResponse> _responseValidator;
        
        public ResponseValidatorProvider(IResponseValidator<TRequest, TResponse> responseValidator)
        {
            _responseValidator = responseValidator;
        }
        
        public IResponseValidator<TRequest, TResponse> Create(IToolset toolset)
        {
            return _responseValidator;
        }
    }
}
