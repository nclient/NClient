using System;
using System.Linq.Expressions;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Clients
{
    public interface IResilienceNClient<T>
    {
        TResult InvokeResiliently<TResult>(Expression<Func<T, TResult>> apiMethodCall, IResiliencePolicyProvider resiliencePolicyProvider);
    }
}
