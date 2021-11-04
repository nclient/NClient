using System.Collections.Generic;
using System.Linq;
using NClient.Common.Helpers;
using NClient.Providers.Results;
using NClient.Standalone.Client.Results;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Results
{
    internal class NClientTransportResultsSetter<TRequest, TResponse> : INClientTransportResultsSetter<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientTransportResultsSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }
        
        public INClientResultsSelector<TRequest, TResponse> Use(IResultBuilder<TRequest, TResponse> builder, params IResultBuilder<TRequest, TResponse>[] extraBuilders)
        {
            return Use(extraBuilders.Concat(new[] { builder }));
        }
        
        public INClientResultsSelector<TRequest, TResponse> Use(IEnumerable<IResultBuilder<TRequest, TResponse>> builders)
        {
            return Use(builders
                .Select(x => new ResultBuilderProvider<TRequest, TResponse>(x))
                .Cast<IResultBuilderProvider<TRequest, TResponse>>()
                .ToArray());
        }
        
        public INClientResultsSelector<TRequest, TResponse> Use(IResultBuilderProvider<TRequest, TResponse> provider, params IResultBuilderProvider<TRequest, TResponse>[] extraProviders)
        {
            return Use(extraProviders.Concat(new[] { provider }));
        }
        
        public INClientResultsSelector<TRequest, TResponse> Use(IEnumerable<IResultBuilderProvider<TRequest, TResponse>> providers)
        {
            var providerCollection = providers as ICollection<IResultBuilderProvider<TRequest, TResponse>> ?? providers.ToArray();
            
            Ensure.AreNotNullItems(providerCollection, nameof(providers));
            
            _builderContextModifier.Add(x => x.WithResultBuilders(providerCollection));
            return new NClientResultsSelector<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
