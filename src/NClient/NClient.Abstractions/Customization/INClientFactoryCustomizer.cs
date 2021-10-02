using System;
using NClient.Abstractions.Customization.Resilience;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Customization
{
    public interface INClientFactoryCustomizer<TRequest, TResponse>
        : INClientCommonCustomizer<INClientFactoryCustomizer<TRequest, TResponse>, INClientFactory, TRequest, TResponse>
    {
        /// <summary>
        /// Sets custom <see cref="IResiliencePolicyProvider{TRequest,TResponse}"/> used to create instances of <see cref="IResiliencePolicy"/> for specific method.
        /// </summary>
        /// <param name="customizer"></param>
        // TODO: doc
        INClientFactoryCustomizer<TRequest, TResponse> WithCustomResilience(Action<IResiliencePolicyMethodSelector<TRequest, TResponse>> customizer);
    }
}
