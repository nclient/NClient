using System;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Resilience;

namespace NClient.Standalone.Ensuring
{
    internal class StubEnsuringSettings<TRequest, TResponse> : IEnsuringSettings<TRequest, TResponse>
    {
        public Predicate<IResponseContext<TRequest, TResponse>> IsSuccess { get; }
        public Action<IResponseContext<TRequest, TResponse>> OnFailure { get; }

        public StubEnsuringSettings()
        {
            IsSuccess = _ => true;
            OnFailure = _ =>
            {
            };
        }
    }
}
