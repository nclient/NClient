using System;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Ensuring
{
    public class EnsuringSettings<TRequest, TResponse> : IEnsuringSettings<TRequest, TResponse>
    {
        public virtual Predicate<IResponseContext<TRequest, TResponse>> IsSuccess { get; }
        public virtual Action<IResponseContext<TRequest, TResponse>> OnFailure { get; }
        
        public EnsuringSettings(
            Predicate<IResponseContext<TRequest, TResponse>> isSuccess, 
            Action<IResponseContext<TRequest, TResponse>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
