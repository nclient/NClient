using System.Collections.Generic;
using System.Linq;
using NClient.Common.Helpers;
using NClient.Providers.Validation;
using NClient.Standalone.Client.Validation;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Validation
{
    internal class NClientTransportResponseValidationSetter<TRequest, TResponse> : INClientTransportResponseValidationSetter<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientTransportResponseValidationSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }
        
        public INClientResponseValidationSelector<TRequest, TResponse> Use(IResponseValidatorSettings<TRequest, TResponse> settings, params IResponseValidatorSettings<TRequest, TResponse>[] extraSettings)
        {
            return Use(extraSettings.Concat(new[] { settings }));
        }
        
        public INClientResponseValidationSelector<TRequest, TResponse> Use(IEnumerable<IResponseValidatorSettings<TRequest, TResponse>> settings)
        {
            return Use(settings
                .Select(x => new ResponseValidator<TRequest, TResponse>(x))
                .Cast<IResponseValidator<TRequest, TResponse>>()
                .ToArray());
        }
        
        public INClientResponseValidationSelector<TRequest, TResponse> Use(IResponseValidator<TRequest, TResponse> validator, params IResponseValidator<TRequest, TResponse>[] extraValidators)
        {
            return Use(extraValidators.Concat(new[] { validator }));
        }
        
        public INClientResponseValidationSelector<TRequest, TResponse> Use(IEnumerable<IResponseValidator<TRequest, TResponse>> validators)
        {
            return Use(validators
                .Select(x => new ResponseValidatorProvider<TRequest, TResponse>(x))
                .Cast<IResponseValidatorProvider<TRequest, TResponse>>()
                .ToArray());
        }
        
        public INClientResponseValidationSelector<TRequest, TResponse> Use(IResponseValidatorProvider<TRequest, TResponse> provider, params IResponseValidatorProvider<TRequest, TResponse>[] extraProviders)
        {
            return Use(extraProviders.Concat(new[] { provider }));
        }
        
        public INClientResponseValidationSelector<TRequest, TResponse> Use(IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> providers)
        {
            var providerCollection = providers as ICollection<IResponseValidatorProvider<TRequest, TResponse>> ?? providers.ToArray();
            
            Ensure.AreNotNullItems(providerCollection, nameof(providers));
            
            _builderContextModifier.Add(x => x.WithResponseValidation(providerCollection));
            return new NClientResponseValidationSelector<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
