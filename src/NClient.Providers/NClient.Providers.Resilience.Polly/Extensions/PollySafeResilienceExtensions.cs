﻿using NClient.Common.Helpers;
using NClient.Core.Extensions;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Transport;
using Polly;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class PollySafeResilienceExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithPollySafeResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> safeMethodPolicy, IAsyncPolicy<IResponseContext<TRequest, TResponse>> otherMethodPolicy)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(safeMethodPolicy, nameof(safeMethodPolicy));
            Ensure.IsNotNull(otherMethodPolicy, nameof(otherMethodPolicy));
            
            return optionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(otherMethodPolicy))
                    .ForMethodsThat((_, request) => request.Type.IsSafe())
                    .Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(safeMethodPolicy)));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithPollySafeResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            IAsyncPolicy<IResponseContext<TRequest, TResponse>> safeMethodPolicy, IAsyncPolicy<IResponseContext<TRequest, TResponse>> otherMethodPolicy)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(safeMethodPolicy, nameof(safeMethodPolicy));
            Ensure.IsNotNull(otherMethodPolicy, nameof(otherMethodPolicy));
            
            return optionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(otherMethodPolicy))
                    .ForMethodsThat((_, request) => request.Type.IsSafe())
                    .Use(new PollyResiliencePolicyProvider<TRequest, TResponse>(safeMethodPolicy)));
        }
    }
}
