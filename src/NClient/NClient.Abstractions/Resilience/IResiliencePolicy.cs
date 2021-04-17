using System;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;

namespace NClient.Abstractions.Resilience
{
    public interface IResiliencePolicy
    {
        Task<HttpResponse> ExecuteAsync(Func<Task<HttpResponse>> action);
    }
}
