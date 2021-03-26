namespace NClient.Abstractions.Resilience
{
    public interface IResiliencePolicyProvider
    {
        IResiliencePolicy Create();
    }
}
