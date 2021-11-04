using System.Linq;
using NClient.Common.Helpers;
using NClient.Providers.Validation;
using NClient.Standalone.Client.Validation;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Validation
{
    internal class NClientAdvancedResponseValidationSetter<TRequest, TResponse> : INClientAdvancedResponseValidationSetter<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientAdvancedResponseValidationSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }

        public INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(params IResponseValidatorSettings<TRequest, TResponse>[] responseValidatorSettings)
        {
            return WithCustomResponseValidation(responseValidatorSettings
                .Select(x => new ResponseValidator<TRequest, TResponse>(x))
                .Cast<IResponseValidator<TRequest, TResponse>>()
                .ToArray());
        }

        public INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(params IResponseValidator<TRequest, TResponse>[] responseValidators)
        {
            return WithCustomResponseValidation(responseValidators
                .Select(x => new ResponseValidatorProvider<TRequest, TResponse>(x))
                .Cast<IResponseValidatorProvider<TRequest, TResponse>>()
                .ToArray());
        }

        public INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(params IResponseValidatorProvider<TRequest, TResponse>[] responseValidatorProvider)
        {
            Ensure.IsNotNull(responseValidatorProvider, nameof(responseValidatorProvider));
            
            _builderContextModifier.Add(x => x.WithResponseValidation(responseValidatorProvider));
            return new NClientAdvancedResponseValidationSetter<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
