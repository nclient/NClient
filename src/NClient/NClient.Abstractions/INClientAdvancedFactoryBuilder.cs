﻿namespace NClient
{
    /// <summary>
    /// A builder abstraction used to create the client factory with custom providers.
    /// </summary>
    public interface INClientAdvancedFactoryBuilder
    {
        // TODO: doc
        INClientFactoryApiBuilder For(string factoryName);
    }
}