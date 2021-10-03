using System;
using System.Linq.Expressions;

namespace NClient.Abstractions.Customization.Resilience
{
    // TODO: doc
    public interface INClientResilienceMethodSelector<TClient, TRequest, TResponse>
    {
        INClientResilienceSetter<TClient, TRequest, TResponse> ForAllMethods();
        INClientResilienceSetter<TClient, TRequest, TResponse> ForMethod(Expression<Func<TClient, Delegate>> methodSelector);
    }
}
