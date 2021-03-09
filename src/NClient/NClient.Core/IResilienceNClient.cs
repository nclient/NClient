using System;
using System.Linq.Expressions;
using NClient.Providers.Resilience.Abstractions;

namespace NClient.Core
{
    public interface IResilienceNClient<T> where T : INClient
    {
        TResult InvokeResiliently<TResult>(Expression<Func<T, TResult>> apiMethodCall, IResiliencePolicyProvider resiliencePolicyProvider);
    }
}
