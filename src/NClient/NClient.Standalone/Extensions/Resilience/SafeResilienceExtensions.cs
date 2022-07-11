using NClient.Common.Helpers;
using NClient.Core.Extensions;
using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{ 
    public static class SafeResilienceExtensions
    {
        /// <summary>Sets a custom resilience policy for safe methods (Info/Options, Check/Head, Read/Get).</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="safeMethodProvider">The custom resilience policy for safe methods (Info/Options, Check/Head, Read/Get).</param>
        /// <param name="otherMethodProvider">The custom resilience policy for unsafe methods (all except Info/Options, Check/Head, Read/Get).</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSafeResilience<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IResiliencePolicyProvider<TRequest, TResponse> safeMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider) 
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(safeMethodProvider, nameof(safeMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return optionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(otherMethodProvider)
                    .ForMethodsThat((_, request) => request.Type.IsSafe())
                    .Use(safeMethodProvider));
        }
        
        /// <summary>Sets a custom resilience policy for safe methods (Info/Options, Check/Head, Read/Get).</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="safeMethodProvider">The custom resilience policy for safe methods (Info/Options, Check/Head, Read/Get).</param>
        /// <param name="otherMethodProvider">The custom resilience policy for unsafe methods (all except Info/Options, Check/Head, Read/Get).</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSafeResilience<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            IResiliencePolicyProvider<TRequest, TResponse> safeMethodProvider, IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(safeMethodProvider, nameof(safeMethodProvider));
            Ensure.IsNotNull(otherMethodProvider, nameof(otherMethodProvider));
            
            return optionalBuilder
                .WithResilience(x => x
                    .ForAllMethods()
                    .Use(otherMethodProvider)
                    .ForMethodsThat((_, request) => request.Type.IsSafe())
                    .Use(safeMethodProvider));
        }
    }
}
