using System;
using NClient.Abstractions.Resilience;

namespace NClient.Abstractions.Validation
{
    // TODO: folder for providers
    // TODO: HttpRequest support
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
