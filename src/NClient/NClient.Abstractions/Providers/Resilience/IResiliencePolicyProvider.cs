// ReSharper disable once CheckNamespace

namespace NClient.Providers.Resilience
{
    /// <summary>A provider abstraction that can create a transient exception handling policies.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    public interface IResiliencePolicyProvider<TRequest, TResponse>
    {
        /// <summary>Creates and configures an instance of transient exception handling policy.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        IResiliencePolicy<TRequest, TResponse> Create(IToolset toolset);
    }
}
