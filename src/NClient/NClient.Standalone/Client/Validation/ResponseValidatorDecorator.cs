using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NClient.Providers.Resilience;
using NClient.Providers.Validation;

namespace NClient.Standalone.Client.Validation
{
    internal class ResponseValidatorDecorator<TRequest, TResponse> : IResponseValidator<TRequest, TResponse>
    {
        private readonly IReadOnlyCollection<IResponseValidator<TRequest, TResponse>> _responseValidators;
        
        public ResponseValidatorDecorator(IEnumerable<IResponseValidator<TRequest, TResponse>> responseValidators)
        {
            _responseValidators = responseValidators
                .OrderByDescending(x => x is IOrderedResponseValidationSettings)
                .ThenBy(x => (x as IOrderedResponseValidationSettings)?.Order)
                .ToArray();
        }

        public bool IsSuccess(IResponseContext<TRequest, TResponse> responseContext)
        {
            return _responseValidators.All(x => x.IsSuccess(responseContext));
        }
        
        public Task OnFailureAsync(IResponseContext<TRequest, TResponse> responseContext)
        {
            foreach (var responseValidator in _responseValidators)
            {
                if (responseValidator.IsSuccess(responseContext))
                    continue;
                
                responseValidator.OnFailureAsync(responseContext);
                return Task.CompletedTask;
            }
            
            return Task.CompletedTask;
        }
    }
}
