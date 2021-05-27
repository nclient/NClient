using System;
using System.Linq;
using System.Reflection;
using NClient.Annotations.Versioning;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.Mappers;

namespace NClient.Core.MethodBuilders.Providers
{
    internal interface IUseVersionAttributeProvider
    {
        UseVersionAttribute? Find(Type clientType, MethodInfo methodInfo);
    }

    internal class UseVersionAttributeProvider : IUseVersionAttributeProvider
    {
        private readonly IAttributeMapper _attributeMapper;

        public UseVersionAttributeProvider(IAttributeMapper attributeMapper)
        {
            _attributeMapper = attributeMapper;
        }

        public UseVersionAttribute? Find(Type clientType, MethodInfo methodInfo)
        {
            var useVersionAttributes = (clientType.IsInterface
                    ? clientType.GetInterfaceCustomAttributes(inherit: true)
                    : clientType.GetCustomAttributes(inherit: true).Cast<Attribute>())
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is UseVersionAttribute)
                .Cast<UseVersionAttribute>()
                .ToArray();
            if (useVersionAttributes.Length > 1)
                throw OuterExceptionFactory.MultipleAttributeForClientNotSupported(nameof(UseVersionAttribute));
            var useVersionAttribute = useVersionAttributes.SingleOrDefault();

            var methodUseVersionAttribute = methodInfo.GetCustomAttribute<UseVersionAttribute>();

            return methodUseVersionAttribute ?? useVersionAttribute;
        }
    }
}