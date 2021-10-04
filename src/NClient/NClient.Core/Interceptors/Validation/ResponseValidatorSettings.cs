using System;
using NClient.Abstractions.Resilience;

namespace NClient.Core.Interceptors.Validation
{
    public interface IResponseValidatorSettings<TRequest, TResponse>
    {
        Predicate<ResponseContext<TRequest, TResponse>> SuccessCondition { get; }
        Action<ResponseContext<TRequest, TResponse>> OnFailure { get; }
    }
    
    public class ResponseValidatorSettings<TRequest, TResponse> : IResponseValidatorSettings<TRequest, TResponse>
    {
        public Predicate<ResponseContext<TRequest, TResponse>> SuccessCondition { get; }
        public Action<ResponseContext<TRequest, TResponse>> OnFailure { get; }
        
        public ResponseValidatorSettings(
            Predicate<ResponseContext<TRequest, TResponse>> successCondition, 
            Action<ResponseContext<TRequest, TResponse>> onFailure)
        {
            SuccessCondition = successCondition;
            OnFailure = onFailure;
        }
    }
}
