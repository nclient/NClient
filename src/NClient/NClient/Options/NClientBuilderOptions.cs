using System;
using System.Collections.Generic;
using System.Net.Http;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public class NClientBuilderOptions<TClient, TRequest, TResult> where TClient : class
    {
        /// <summary>
        /// Gets a list of operations used to configure an <see cref="HttpClient"/>.
        /// </summary>
        public IList<Func<INClientOptionalBuilder<TClient, TRequest, TResult>, INClientOptionalBuilder<TClient, TRequest, TResult>>> BuilderActions { get; } 
            = new List<Func<INClientOptionalBuilder<TClient, TRequest, TResult>, INClientOptionalBuilder<TClient, TRequest, TResult>>>();
    }
}
