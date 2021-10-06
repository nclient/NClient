using System;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Ensuring
{
    public interface IEnsuringSettings<TRequest, TResponse>
    {
        Predicate<ResponseContext<TRequest, TResponse>> IsSuccess { get; }
        Action<ResponseContext<TRequest, TResponse>> OnFailure { get; }
    }
}
