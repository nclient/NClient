using System;
using System.Threading.Tasks;

namespace NClient.Providers.Resilience.Abstractions
{
    public interface IResiliencePolicy
    {
        Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action);
    }
}
