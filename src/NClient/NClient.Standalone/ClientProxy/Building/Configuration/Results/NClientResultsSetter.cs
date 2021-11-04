using System.Collections.Generic;
using System.Linq;
using NClient.Common.Helpers;
using NClient.Providers.Results;
using NClient.Providers.Transport;
using NClient.Standalone.Client.Results;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Results
{
    internal class NClientResultsSetter<TRequest, TResponse> : INClientResultsSetter<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientResultsSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }

        public INClientResultsSelector<TRequest, TResponse> Use(IResultBuilder<IRequest, IResponse> builder, params IResultBuilder<IRequest, IResponse>[] extraBuilders)
        {
            return Use(extraBuilders.Concat(new[] { builder }));
        }
        
        public INClientResultsSelector<TRequest, TResponse> Use(IEnumerable<IResultBuilder<IRequest, IResponse>> builders)
        {
            return Use(builders
                .Select(x => new ResultBuilderProvider<IRequest, IResponse>(x))
                .Cast<IResultBuilderProvider<IRequest, IResponse>>()
                .ToArray());
        }
        
        public INClientResultsSelector<TRequest, TResponse> Use(IResultBuilderProvider<IRequest, IResponse> provider, params IResultBuilderProvider<IRequest, IResponse>[] extraProviders)
        {
            return Use(extraProviders.Concat(new[] { provider }));
        }
        
        public INClientResultsSelector<TRequest, TResponse> Use(IEnumerable<IResultBuilderProvider<IRequest, IResponse>> providers)
        {
            var providerCollection = providers as ICollection<IResultBuilderProvider<IRequest, IResponse>> ?? providers.ToArray();
            
            Ensure.AreNotNullItems(providerCollection, nameof(providers));
            
            _builderContextModifier.Add(x => x.WithResultBuilders(providerCollection));
            return new NClientResultsSelector<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
