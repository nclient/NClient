using System.Collections.Generic;
using System.Linq;
using NClient.Common.Helpers;
using NClient.Providers.Mapping;
using NClient.Standalone.Client.Mapping;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Mapping
{
    internal class NClientTransportResponseMappingSetter<TRequest, TResponse> : INClientTransportResponseMappingSetter<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientTransportResponseMappingSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }
        
        public INClientResponseMappingSelector<TRequest, TResponse> Use(IResponseMapper<TRequest, TResponse> builder, params IResponseMapper<TRequest, TResponse>[] extraBuilders)
        {
            return Use(extraBuilders.Concat(new[] { builder }));
        }
        
        public INClientResponseMappingSelector<TRequest, TResponse> Use(IEnumerable<IResponseMapper<TRequest, TResponse>> mappers)
        {
            var mapperCollection = mappers as ICollection<IResponseMapper<TRequest, TResponse>> ?? mappers.ToArray();

            Ensure.AreNotNullItems(mapperCollection, nameof(mappers));
            
            return Use(mapperCollection
                .Select(x => new ResponseMapperProvider<TRequest, TResponse>(x))
                .Cast<IResponseMapperProvider<TRequest, TResponse>>()
                .ToArray());
        }
        
        public INClientResponseMappingSelector<TRequest, TResponse> Use(IResponseMapperProvider<TRequest, TResponse> provider, params IResponseMapperProvider<TRequest, TResponse>[] extraProviders)
        {
            return Use(extraProviders.Concat(new[] { provider }));
        }
        
        public INClientResponseMappingSelector<TRequest, TResponse> Use(IEnumerable<IResponseMapperProvider<TRequest, TResponse>> providers)
        {
            var providerCollection = providers as ICollection<IResponseMapperProvider<TRequest, TResponse>> ?? providers.ToArray();
            
            Ensure.AreNotNullItems(providerCollection, nameof(providers));
            
            _builderContextModifier.Add(x => x.WithResultBuilders(providerCollection));
            return new NClientResponseMappingSelector<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
