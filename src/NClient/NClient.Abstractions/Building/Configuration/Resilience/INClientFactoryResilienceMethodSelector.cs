using System;
using System.Linq.Expressions;
using System.Reflection;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    // TODO: doc
    public interface INClientFactoryResilienceMethodSelector<TRequest, TResponse>
    {
        INClientFactoryResilienceSetter<TRequest, TResponse> ForAllMethods();
        INClientFactoryResilienceSetter<TRequest, TResponse> ForAllMethodsOf<TClient>();
        INClientFactoryResilienceSetter<TRequest, TResponse> ForMethodOf<TClient>(Expression<Func<TClient, Delegate>> methodSelector);
        INClientFactoryResilienceSetter<TRequest, TResponse> ForMethodsThat(Func<MethodInfo, IRequest, bool> predicate);
    }
}
