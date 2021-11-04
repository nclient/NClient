using System.Collections.Generic;
using System.Linq;
using NClient.Common.Helpers;
using NClient.Providers.Results;
using NClient.Providers.Transport;
using NClient.Standalone.Client.Results;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Results
{
    internal class NClientAdvancedResultsSetter<TRequest, TResponse> : INClientAdvancedResultsSetter<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientAdvancedResultsSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }
        
        public INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomTransportResults(IResultBuilder<TRequest, TResponse> builder, params IResultBuilder<TRequest, TResponse>[] extraBuilders)
        {
            return WithCustomTransportResults(extraBuilders.Concat(new[] { builder }));
        }
        
        public INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomTransportResults(IEnumerable<IResultBuilder<TRequest, TResponse>> builders)
        {
            return WithCustomTransportResults(builders
                .Select(x => new ResultBuilderProvider<TRequest, TResponse>(x))
                .Cast<IResultBuilderProvider<TRequest, TResponse>>()
                .ToArray());
        }
        
        public INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomTransportResults(IResultBuilderProvider<TRequest, TResponse> provider, params IResultBuilderProvider<TRequest, TResponse>[] extraProviders)
        {
            return WithCustomTransportResults(extraProviders.Concat(new[] { provider }));
        }
        
        public INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomTransportResults(IEnumerable<IResultBuilderProvider<TRequest, TResponse>> providers)
        {
            var providerCollection = providers as ICollection<IResultBuilderProvider<TRequest, TResponse>> ?? providers.ToArray();
            
            Ensure.AreNotNullItems(providerCollection, nameof(providers));
            
            _builderContextModifier.Add(x => x.WithResultBuilders(providerCollection));
            return new NClientAdvancedResultsSetter<TRequest, TResponse>(_builderContextModifier);
        }
        
        public INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomResults(IResultBuilder<IRequest, IResponse> builder, params IResultBuilder<IRequest, IResponse>[] extraBuilders)
        {
            return WithCustomResults(extraBuilders.Concat(new[] { builder }));
        }
        
        public INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomResults(IEnumerable<IResultBuilder<IRequest, IResponse>> builders)
        {
            return WithCustomTransportResults(builders
                .Select(x => new ResultBuilderProvider<IRequest, IResponse>(x))
                .Cast<IResultBuilderProvider<TRequest, TResponse>>()
                .ToArray());
        }
        
        public INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomResults(IResultBuilderProvider<IRequest, IResponse> provider, params IResultBuilderProvider<IRequest, IResponse>[] extraProviders)
        {
            return WithCustomResults(extraProviders.Concat(new[] { provider }));
        }
        
        public INClientAdvancedResultsSetter<TRequest, TResponse> WithCustomResults(IEnumerable<IResultBuilderProvider<IRequest, IResponse>> providers)
        {
            var providerCollection = providers as ICollection<IResultBuilderProvider<IRequest, IResponse>> ?? providers.ToArray();
            
            Ensure.AreNotNullItems(providerCollection, nameof(providers));
            
            _builderContextModifier.Add(x => x.WithResultBuilders(providerCollection));
            return new NClientAdvancedResultsSetter<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
