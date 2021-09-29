using System;
using System.Linq.Expressions;

namespace NClient.Abstractions.Customization.Resilience
{
    // TODO: doc
    public interface IResiliencePolicyMethodSelector<TInterface, TRequest, TResponse>
    {
        IResiliencePolicyProviderSetter<TInterface, TRequest, TResponse> ForAllMethods();
        IResiliencePolicyProviderSetter<TInterface, TRequest, TResponse> ForMethod(Expression<Func<TInterface, Delegate>> methodSelector);
    }
    
    // TODO: doc
    public interface IResiliencePolicyMethodSelector<TRequest, TResponse>
    {
        IResiliencePolicyProviderSetter<TRequest, TResponse> ForAllMethods();
        IResiliencePolicyProviderSetter<TRequest, TResponse> ForAllMethodsOf<TInterface>();
        IResiliencePolicyProviderSetter<TRequest, TResponse> ForMethodOf<TInterface>(Expression<Func<TInterface, Delegate>> methodSelector);
    }
}
