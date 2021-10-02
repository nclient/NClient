using System;
using NClient.Abstractions.Customization.Resilience;

namespace NClient.Abstractions.Customization
{
    public interface INClientBuilderCustomizer<TClient, TRequest, TResponse>
        : INClientCommonCustomizer<INClientBuilderCustomizer<TClient, TRequest, TResponse>, TClient, TRequest, TResponse>
        where TClient : class
    {
        // TODO: doc
        INClientBuilderCustomizer<TClient, TRequest, TResponse> WithCustomResilience(Action<IResiliencePolicyMethodSelector<TClient, TRequest, TResponse>> customizer);
    }
}
