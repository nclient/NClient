using System.Collections.Generic;
using NClient.Providers.Results;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientTransportResultsSetter<TRequest, TResponse>
    {
        // TODO: doc
        INClientResultsSelector<TRequest, TResponse> Use(IEnumerable<IResultBuilder<TRequest, TResponse>> builders);
        
        INClientResultsSelector<TRequest, TResponse> Use(IEnumerable<IResultBuilderProvider<TRequest, TResponse>> providers);
    }
}
