using System;
using System.Linq;
using NClient.Annotations;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.Mappers;

namespace NClient.Core.MethodBuilders.Providers
{
    internal interface IPathAttributeProvider
    {
        PathAttribute? Find(Type clientType);
    }

    internal class PathAttributeProvider : IPathAttributeProvider
    {
        private readonly IAttributeMapper _attributeMapper;
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public PathAttributeProvider(
            IAttributeMapper attributeMapper,
            IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _attributeMapper = attributeMapper;
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public PathAttribute? Find(Type clientType)
        {
            var pathAttributes = (clientType.IsInterface
                    ? clientType.GetInterfaceCustomAttributes(inherit: true)
                    : clientType.GetCustomAttributes(inherit: true).Cast<Attribute>())
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is PathAttribute)
                .Cast<PathAttribute>()
                .ToArray();
            if (pathAttributes.Length > 1)
                throw _clientValidationExceptionFactory.MultipleAttributeForClientNotSupported(nameof(PathAttribute));
            var pathAttribute = pathAttributes.SingleOrDefault();

            return pathAttribute;
        }
    }
}