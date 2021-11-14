﻿using System;
using System.Linq.Expressions;
using NClient.Common.Helpers;
using NClient.Core.Helpers.EqualityComparers;
using NClient.Invocation;
using NClient.Providers.Transport;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Resilience
{
    internal class NClientResilienceMethodSelector<TClient, TRequest, TResponse> : INClientResilienceMethodSelector<TClient, TRequest, TResponse>
    {
        private readonly MethodInfoEqualityComparer _methodInfoEqualityComparer = new();
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientResilienceMethodSelector(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }
        
        public INClientResilienceSetter<TClient, TRequest, TResponse> ForAllMethods()
        {
            return new NClientResilienceSetter<TClient, TRequest, TResponse>(
                _builderContextModifier, 
                methodPredicate: (_, _) => true);
        }
        
        public INClientResilienceSetter<TClient, TRequest, TResponse> ForMethod(Expression<Func<TClient, Delegate>> selector)
        {
            Ensure.IsNotNull(selector, nameof(selector));
            
            var func = selector.Compile();
            var selectedMethod = func.Invoke(default!).Method;
            return new NClientResilienceSetter<TClient, TRequest, TResponse>(
                _builderContextModifier, 
                methodPredicate: (method, _) => _methodInfoEqualityComparer.Equals(method.Info, selectedMethod));
        }
        
        public INClientResilienceSetter<TClient, TRequest, TResponse> ForMethodsThat(Func<IMethod, IRequest, bool> condition)
        {
            Ensure.IsNotNull(condition, nameof(condition));
            
            return new NClientResilienceSetter<TClient, TRequest, TResponse>(
                _builderContextModifier, 
                condition);
        }
    }
}
