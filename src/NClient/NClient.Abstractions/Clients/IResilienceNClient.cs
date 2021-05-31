using System;
using System.Linq.Expressions;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Clients
{
    public interface IResilienceNClient<T>
    {
        TResult Invoke<TResult>(Expression<Func<T, TResult>> methodCall, IResiliencePolicyProvider resiliencePolicyProvider);
    }
}
