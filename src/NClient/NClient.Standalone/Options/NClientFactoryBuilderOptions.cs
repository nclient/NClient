using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>An options class for configuring a client factory.</summary>
    public class NClientFactoryBuilderOptions<TRequest, TResult>
    {
        /// <summary>Gets a list of operations used to configure a client factory.</summary>
        public IList<Func<INClientFactoryOptionalBuilder<TRequest, TResult>, INClientFactoryOptionalBuilder<TRequest, TResult>>> BuilderActions { get; } 
            = new List<Func<INClientFactoryOptionalBuilder<TRequest, TResult>, INClientFactoryOptionalBuilder<TRequest, TResult>>>();
    }
}
