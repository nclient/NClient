using System.Threading.Tasks;
using NClient.Providers.Transport;
using NClient.Providers.Validation;

namespace NClient.Standalone.Client.Validation
{
    internal class ResponseValidator<TRequest, TResponse> : IResponseValidator<TRequest, TResponse>
    {
        private readonly IResponseValidatorSettings<TRequest, TResponse> _responseValidatorSettings;
        
        public ResponseValidator(IResponseValidatorSettings<TRequest, TResponse> responseValidatorSettings)
        {
            _responseValidatorSettings = responseValidatorSettings;
        }
        
        public bool IsSuccess(IResponseContext<TRequest, TResponse> responseContext)
        {
            return _responseValidatorSettings.IsSuccess(responseContext);
        }

        public Task OnFailureAsync(IResponseContext<TRequest, TResponse> responseContext)
        {
            _responseValidatorSettings.OnFailure(responseContext);
            return Task.CompletedTask;
        }
    }
}
