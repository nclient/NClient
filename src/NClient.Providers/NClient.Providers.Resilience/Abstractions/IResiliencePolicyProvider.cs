namespace NClient.Providers.Resilience.Abstractions
{
    public interface IResiliencePolicyProvider
    {
        IResiliencePolicy Create();
    }
}
