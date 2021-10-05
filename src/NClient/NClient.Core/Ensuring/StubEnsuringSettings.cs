using System;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Resilience;

namespace NClient.Core.Ensuring
{
    internal class StubEnsuringSettings<TRequest, TResponse> : IEnsuringSettings<TRequest, TResponse>
    {
        public Predicate<ResponseContext<TRequest, TResponse>> IsSuccess { get; }
        public Action<ResponseContext<TRequest, TResponse>> OnFailure { get; }

        public StubEnsuringSettings()
        {
            IsSuccess = _ => true;
            OnFailure = _ => { };
        }
    }
}
