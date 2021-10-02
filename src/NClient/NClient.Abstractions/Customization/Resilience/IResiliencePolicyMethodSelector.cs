using System;
using System.Linq.Expressions;

namespace NClient.Abstractions.Customization.Resilience
{
    // TODO: doc
    public interface IResiliencePolicyMethodSelector<TClient, TRequest, TResponse>
    {
        IResiliencePolicyProviderSetter<TClient, TRequest, TResponse> ForAllMethods();
        IResiliencePolicyProviderSetter<TClient, TRequest, TResponse> ForMethod(Expression<Func<TClient, Delegate>> methodSelector);
    }
    
    // TODO: doc
    public interface IResiliencePolicyMethodSelector<TRequest, TResponse>
    {
        IResiliencePolicyProviderSetter<TRequest, TResponse> ForAllMethods();
        IResiliencePolicyProviderSetter<TRequest, TResponse> ForAllMethodsOf<TClient>();
        IResiliencePolicyProviderSetter<TRequest, TResponse> ForMethodOf<TClient>(Expression<Func<TClient, Delegate>> methodSelector);
    }
}
