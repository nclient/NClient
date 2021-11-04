using System.Collections.Generic;
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
        
        public INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(IResponseValidatorSettings<TRequest, TResponse> settings, params IResponseValidatorSettings<TRequest, TResponse>[] extraSettings)
        {
            return WithCustomResponseValidation(extraSettings.Concat(new[] { settings }));
        }
        
        public INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(IEnumerable<IResponseValidatorSettings<TRequest, TResponse>> settings)
        {
            return WithCustomResponseValidation(settings
                .Select(x => new ResponseValidator<TRequest, TResponse>(x))
                .Cast<IResponseValidator<TRequest, TResponse>>()
                .ToArray());
        }
        
        public INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(IResponseValidator<TRequest, TResponse> validator, params IResponseValidator<TRequest, TResponse>[] extraValidators)
        {
            return WithCustomResponseValidation(extraValidators.Concat(new[] { validator }));
        }
        
        public INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(IEnumerable<IResponseValidator<TRequest, TResponse>> validators)
        {
            return WithCustomResponseValidation(validators
                .Select(x => new ResponseValidatorProvider<TRequest, TResponse>(x))
                .Cast<IResponseValidatorProvider<TRequest, TResponse>>()
                .ToArray());
        }
        
        public INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(IResponseValidatorProvider<TRequest, TResponse> provider, params IResponseValidatorProvider<TRequest, TResponse>[] extraProviders)
        {
            return WithCustomResponseValidation(extraProviders.Concat(new[] { provider }));
        }
        
        public INClientAdvancedResponseValidationSetter<TRequest, TResponse> WithCustomResponseValidation(IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> providers)
        {
            var providerCollection = providers as ICollection<IResponseValidatorProvider<TRequest, TResponse>> ?? providers.ToArray();
            
            Ensure.AreNotNullItems(providerCollection, nameof(providers));
            
            _builderContextModifier.Add(x => x.WithResponseValidation(providerCollection));
            return new NClientAdvancedResponseValidationSetter<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
