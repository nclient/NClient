using System;
using System.Linq.Expressions;

namespace NClient.Abstractions.Customization.Resilience
{
    // TODO: doc
    public interface INClientFactoryResilienceMethodSelector<TRequest, TResponse>
    {
        INClientFactoryResilienceSetter<TRequest, TResponse> ForAllMethods();
        INClientFactoryResilienceSetter<TRequest, TResponse> ForAllMethodsOf<TClient>();
        INClientFactoryResilienceSetter<TRequest, TResponse> ForMethodOf<TClient>(Expression<Func<TClient, Delegate>> methodSelector);
    }
}
