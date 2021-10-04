using System;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Ensuring
{
    public class EnsuringSettings<TRequest, TResponse> : IEnsuringSettings<TRequest, TResponse>
    {
        public virtual Predicate<ResponseContext<TRequest, TResponse>> SuccessCondition { get; }
        public virtual Action<ResponseContext<TRequest, TResponse>> OnFailure { get; }
        
        public EnsuringSettings(
            Predicate<ResponseContext<TRequest, TResponse>> successCondition, 
            Action<ResponseContext<TRequest, TResponse>> onFailure)
        {
            SuccessCondition = successCondition;
            OnFailure = onFailure;
        }
    }
}
