﻿namespace NClient.Abstractions.Resilience
{
    /// <summary>
    /// A provider abstraction for a component that can create <see cref="IResiliencePolicy"/> instances.
    /// </summary>
    public interface IResiliencePolicyProvider
    {
        /// <summary>
        /// Creates and configures an instance of <see cref="IResiliencePolicy"/> instance.
        /// </summary>
        IResiliencePolicy Create();
    }
}
