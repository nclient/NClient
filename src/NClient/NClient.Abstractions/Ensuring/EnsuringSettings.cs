using System;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Ensuring
{
    public class EnsuringSettings<TRequest, TResponse> : IEnsuringSettings<TRequest, TResponse>
    {
        public virtual Predicate<ResponseContext<TRequest, TResponse>> IsSuccess { get; }
        public virtual Action<ResponseContext<TRequest, TResponse>> OnFailure { get; }
        
        public EnsuringSettings(
            Predicate<ResponseContext<TRequest, TResponse>> isSuccess, 
            Action<ResponseContext<TRequest, TResponse>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
