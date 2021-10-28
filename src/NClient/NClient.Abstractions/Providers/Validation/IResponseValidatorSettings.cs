using System;
using NClient.Providers.Resilience;

namespace NClient.Providers.Validation
{
    public interface IResponseValidatorSettings<TRequest, TResponse>
    {
        Predicate<IResponseContext<TRequest, TResponse>> IsSuccess { get; }
        Action<IResponseContext<TRequest, TResponse>> OnFailure { get; }
    }
    
    public interface IOrderedResponseValidationSettings
    {
        public int Order { get; }
    }
}
