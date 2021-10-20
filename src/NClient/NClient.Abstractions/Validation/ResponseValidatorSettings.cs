using System;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Validation
{
    public class ResponseValidatorSettings<TRequest, TResponse> : IResponseValidatorSettings<TRequest, TResponse>
    {
        public virtual Predicate<IResponseContext<TRequest, TResponse>> IsSuccess { get; }
        public virtual Action<IResponseContext<TRequest, TResponse>> OnFailure { get; }
        
        public ResponseValidatorSettings(
            Predicate<IResponseContext<TRequest, TResponse>> isSuccess, 
            Action<IResponseContext<TRequest, TResponse>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
