using System.Collections.Generic;
using System.Linq;
using NClient.Providers;
using NClient.Providers.Validation;

namespace NClient.Standalone.Client.Validation
{
    internal class CompositeResponseValidatorProvider<TRequest, TResponse> : IResponseValidatorProvider<TRequest, TResponse>
    {
        private readonly IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> _responseValidatorProviders;
        
        public CompositeResponseValidatorProvider(IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> responseValidatorProviders)
        {
            _responseValidatorProviders = responseValidatorProviders;
        }
        
        public IResponseValidator<TRequest, TResponse> Create(IToolset toolset)
        {
            return new CompositeResponseValidator<TRequest, TResponse>(_responseValidatorProviders
                .Select(x => x.Create(toolset)));
        }
    }
}
