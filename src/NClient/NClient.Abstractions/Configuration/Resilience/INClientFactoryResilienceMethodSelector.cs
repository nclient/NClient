using System;
using System.Linq.Expressions;
using System.Reflection;
using NClient.Abstractions.HttpClients;

namespace NClient.Abstractions.Configuration.Resilience
{
    // TODO: doc
    public interface INClientFactoryResilienceMethodSelector<TRequest, TResponse>
    {
        INClientFactoryResilienceSetter<TRequest, TResponse> ForAllMethods();
        INClientFactoryResilienceSetter<TRequest, TResponse> ForAllMethodsOf<TClient>();
        INClientFactoryResilienceSetter<TRequest, TResponse> ForMethodOf<TClient>(Expression<Func<TClient, Delegate>> methodSelector);
        INClientFactoryResilienceSetter<TRequest, TResponse> ForMethodsThat(Func<MethodInfo, IHttpRequest, bool> predicate);
    }
}
