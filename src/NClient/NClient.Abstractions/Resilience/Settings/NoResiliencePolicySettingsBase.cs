namespace NClient.Abstractions.Resilience.Settings
{
    public abstract class NoResiliencePolicySettingsBase<TRequest, TResponse> : DefaultResiliencePolicySettingsBase<TRequest, TResponse>
    {
        public override int RetryCount { get; set; } = 0;
    }
}
