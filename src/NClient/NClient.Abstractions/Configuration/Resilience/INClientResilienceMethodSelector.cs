﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using NClient.Abstractions.HttpClients;

namespace NClient.Abstractions.Configuration.Resilience
{
    // TODO: doc
    public interface INClientResilienceMethodSelector<TClient, TRequest, TResponse>
    {
        INClientResilienceSetter<TClient, TRequest, TResponse> ForAllMethods();
        INClientResilienceSetter<TClient, TRequest, TResponse> ForMethod(Expression<Func<TClient, Delegate>> methodSelector);
        INClientResilienceSetter<TClient, TRequest, TResponse> ForMethodsThat(Func<MethodInfo, IHttpRequest, bool> predicate);
    }
}
