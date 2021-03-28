using System;
using System.Threading.Tasks;

namespace NClient.Abstractions.Resilience
{
    public interface IResiliencePolicy
    {
        Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action);
    }
}
