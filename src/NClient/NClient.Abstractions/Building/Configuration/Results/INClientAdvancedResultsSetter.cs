using System.Collections.Generic;
using NClient.Providers.Results;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace

namespace NClient
{
    public interface INClientAdvancedResultsSetter<TRequest, TResponse>
    {
        // TODO: doc
        INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomTransportResults(IResultBuilder<TRequest, TResponse> builder, params IResultBuilder<TRequest, TResponse>[] extraBuilders);
        
        INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomTransportResults(IEnumerable<IResultBuilder<TRequest, TResponse>> builders);
        
        INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomTransportResults(IResultBuilderProvider<TRequest, TResponse> provider, params IResultBuilderProvider<TRequest, TResponse>[] extraProviders);
        
        INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomTransportResults(IEnumerable<IResultBuilderProvider<TRequest, TResponse>> providers);
        
        INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomResults(IResultBuilder<IRequest, IResponse> builder, params IResultBuilder<IRequest, IResponse>[] extraBuilders);
        
        INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomResults(IEnumerable<IResultBuilder<IRequest, IResponse>> builders);
        
        INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomResults(IResultBuilderProvider<IRequest, IResponse> provider, params IResultBuilderProvider<IRequest, IResponse>[] extraProviders);
        
        INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomResults(IEnumerable<IResultBuilderProvider<IRequest, IResponse>> providers);
    }
}
