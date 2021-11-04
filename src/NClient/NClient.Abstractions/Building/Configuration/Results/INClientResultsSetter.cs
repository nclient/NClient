using System.Collections.Generic;
using NClient.Providers.Results;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientResultsSetter<TRequest, TResponse>
    {
        INClientResultsSelector<TRequest, TResponse> Use(IResultBuilder<IRequest, IResponse> builder, params IResultBuilder<IRequest, IResponse>[] extraBuilders);
        
        INClientResultsSelector<TRequest, TResponse> Use(IEnumerable<IResultBuilder<IRequest, IResponse>> builders);
        
        INClientResultsSelector<TRequest, TResponse> Use(IResultBuilderProvider<IRequest, IResponse> provider, params IResultBuilderProvider<IRequest, IResponse>[] extraProviders);
        
        INClientResultsSelector<TRequest, TResponse> Use(IEnumerable<IResultBuilderProvider<IRequest, IResponse>> providers);
    }
}
