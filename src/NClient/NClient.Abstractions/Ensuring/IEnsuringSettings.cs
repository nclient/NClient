using System;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Ensuring
{
    public interface IEnsuringSettings<TRequest, TResponse>
    {
        Predicate<IResponseContext<TRequest, TResponse>> IsSuccess { get; }
        Action<IResponseContext<TRequest, TResponse>> OnFailure { get; }
    }
    
    public interface IOrderedEnsuringSettings
    {
        public int Order { get; }
    }
}
