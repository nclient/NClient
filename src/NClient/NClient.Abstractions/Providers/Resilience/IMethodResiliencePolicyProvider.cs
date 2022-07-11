using NClient.Invocation;
using NClient.Providers.Transport;

namespace NClient.Providers.Resilience
{
    /// <summary>The provider that can create resilience policy for transient exception handling.</summary>
    /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
    /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
    internal interface IMethodResiliencePolicyProvider<TRequest, TResponse>
    {
        /// <summary>Creates the provider that can create resilience policy for transient exception handling.</summary>
        /// <param name="method">The information about the client's executable method.</param>
        /// <param name="request">The container for data used to make requests.</param>
        /// <param name="toolset">Tools that help implement providers.</param>
        IResiliencePolicy<TRequest, TResponse> Create(IMethod method, IRequest request, IToolset toolset);
    }
}
