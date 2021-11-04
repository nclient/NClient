using System.Collections.Generic;
using NClient.Providers.Results;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientTransportResultsSetter<TRequest, TResponse>
    {
        // TODO: doc
        INClientResultsSelector<TRequest, TResponse> Use(IResultBuilder<TRequest, TResponse> builder, params IResultBuilder<TRequest, TResponse>[] extraBuilders);
        
        INClientResultsSelector<TRequest, TResponse> Use(IEnumerable<IResultBuilder<TRequest, TResponse>> builders);
        
        INClientResultsSelector<TRequest, TResponse> Use(IResultBuilderProvider<TRequest, TResponse> provider, params IResultBuilderProvider<TRequest, TResponse>[] extraProviders);
        
        INClientResultsSelector<TRequest, TResponse> Use(IEnumerable<IResultBuilderProvider<TRequest, TResponse>> providers);
    }
}
