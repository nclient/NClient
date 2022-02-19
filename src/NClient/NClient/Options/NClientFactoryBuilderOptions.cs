using System;
using System.Collections.Generic;
using System.Net.Http;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public class NClientFactoryBuilderOptions<TRequest, TResult>
    {
        /// <summary>
        /// Gets a list of operations used to configure an <see cref="HttpClient"/>.
        /// </summary>
        public IList<Func<INClientFactoryOptionalBuilder<TRequest, TResult>, INClientFactoryOptionalBuilder<TRequest, TResult>>> BuilderActions { get; } 
            = new List<Func<INClientFactoryOptionalBuilder<TRequest, TResult>, INClientFactoryOptionalBuilder<TRequest, TResult>>>();
    }
}
