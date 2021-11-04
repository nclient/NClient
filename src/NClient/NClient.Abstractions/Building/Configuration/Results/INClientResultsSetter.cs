using System.Collections.Generic;
using NClient.Providers.Results;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientResultsSetter<TRequest, TResponse>
    {
        INClientResultsSelector<TRequest, TResponse> Use(IEnumerable<IResultBuilder<IRequest, IResponse>> builders);
        
        INClientResultsSelector<TRequest, TResponse> Use(IEnumerable<IResultBuilderProvider<IRequest, IResponse>> providers);
    }
}
