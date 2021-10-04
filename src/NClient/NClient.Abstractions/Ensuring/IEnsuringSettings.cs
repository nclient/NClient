using System;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Ensuring
{
    public interface IEnsuringSettings<TRequest, TResponse>
    {
        Predicate<ResponseContext<TRequest, TResponse>> SuccessCondition { get; }
        Action<ResponseContext<TRequest, TResponse>> OnFailure { get; }
    }
}
