using System;
using NClient.Abstractions.Customization.Resilience;

namespace NClient.Abstractions.Customization
{
    public interface INClientBuilderCustomizer<TInterface, TRequest, TResponse>
        : INClientCommonCustomizer<INClientBuilderCustomizer<TInterface, TRequest, TResponse>, TInterface, TRequest, TResponse>
        where TInterface : class
    {
        // TODO: doc
        INClientBuilderCustomizer<TInterface, TRequest, TResponse> WithCustomResilience(Action<IResiliencePolicyMethodSelector<TInterface, TRequest, TResponse>> customizer);
    }
}
