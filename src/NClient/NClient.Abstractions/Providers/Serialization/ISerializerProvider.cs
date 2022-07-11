﻿using Microsoft.Extensions.Logging;

namespace NClient.Providers.Serialization
{
    /// <summary>A provider abstraction for a component that can create <see cref="ISerializer"/> instances.</summary>
    public interface ISerializerProvider
    {
        /// <summary>Creates and configures an instance of <see cref="ISerializer"/> instance.</summary>
        /// <param name="logger">Optional logger. If it is not passed, then logs will not be written.</param>
        ISerializer Create(ILogger? logger);
    }
}
