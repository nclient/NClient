using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>An options class for configuring a client.</summary>
    public class NClientBuilderOptions<TClient, TRequest, TResult> where TClient : class
    {
        /// <summary>Gets a list of operations used to configure a client.</summary>
        public IList<Func<INClientOptionalBuilder<TClient, TRequest, TResult>, INClientOptionalBuilder<TClient, TRequest, TResult>>> BuilderActions { get; } 
            = new List<Func<INClientOptionalBuilder<TClient, TRequest, TResult>, INClientOptionalBuilder<TClient, TRequest, TResult>>>();
    }
}
