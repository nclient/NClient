using System.Collections.Generic;
using System.Linq;
using NClient.Common.Helpers;
using NClient.Providers.Mapping;
using NClient.Providers.Transport;
using NClient.Standalone.Client.Mapping;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Configuration.Mapping
{
    internal class NClientResponseMappingSetter<TRequest, TResponse> : INClientResponseMappingSetter<TRequest, TResponse>
    {
        private readonly BuilderContextModifier<TRequest, TResponse> _builderContextModifier;
        
        public NClientResponseMappingSetter(BuilderContextModifier<TRequest, TResponse> builderContextModifier)
        {
            _builderContextModifier = builderContextModifier;
        }

        public INClientResponseMappingSelector<TRequest, TResponse> Use(IResponseMapper<IRequest, IResponse> builder, params IResponseMapper<IRequest, IResponse>[] extraBuilders)
        {
            return Use(extraBuilders.Concat(new[] { builder }));
        }
        
        public INClientResponseMappingSelector<TRequest, TResponse> Use(IEnumerable<IResponseMapper<IRequest, IResponse>> mappers)
        {
            var mapperCollection = mappers as ICollection<IResponseMapper<IRequest, IResponse>> ?? mappers.ToArray();

            Ensure.AreNotNullItems(mapperCollection, nameof(mappers));
            
            return Use(mapperCollection
                .Select(x => new ResponseMapperProvider<IRequest, IResponse>(x))
                .Cast<IResponseMapperProvider<IRequest, IResponse>>()
                .ToArray());
        }
        
        public INClientResponseMappingSelector<TRequest, TResponse> Use(IResponseMapperProvider<IRequest, IResponse> provider, params IResponseMapperProvider<IRequest, IResponse>[] extraProviders)
        {
            return Use(extraProviders.Concat(new[] { provider }));
        }
        
        public INClientResponseMappingSelector<TRequest, TResponse> Use(IEnumerable<IResponseMapperProvider<IRequest, IResponse>> providers)
        {
            var providerCollection = providers as ICollection<IResponseMapperProvider<IRequest, IResponse>> ?? providers.ToArray();
            
            Ensure.AreNotNullItems(providerCollection, nameof(providers));
            
            _builderContextModifier.Add(x => x.WithResultBuilders(providerCollection));
            return new NClientResponseMappingSelector<TRequest, TResponse>(_builderContextModifier);
        }
    }
}
