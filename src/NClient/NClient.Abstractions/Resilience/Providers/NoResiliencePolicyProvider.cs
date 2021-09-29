namespace NClient.Abstractions.Resilience.Providers
{
    internal class NoResiliencePolicyProvider<TRequest, TResponse> : IResiliencePolicyProvider<TRequest, TResponse>
    {
        public IResiliencePolicy<TRequest, TResponse> Create()
        {
            return new NoResiliencePolicy<TRequest, TResponse>();
        }
    }
}
