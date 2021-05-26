using System;
using System.Linq;
using System.Reflection;
using NClient.Annotations.Versioning;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.Mappers;

namespace NClient.Core.MethodBuilders.Providers
{
    internal interface IVersionAttributeProvider
    {
        VersionAttribute? Find(Type clientType, MethodInfo methodInfo);
    }

    internal class VersionAttributeProvider : IVersionAttributeProvider
    {
        private readonly IAttributeMapper _attributeMapper;

        public VersionAttributeProvider(IAttributeMapper attributeMapper)
        {
            _attributeMapper = attributeMapper;
        }

        public VersionAttribute? Find(Type clientType, MethodInfo methodInfo)
        {
            var versionAttributes = (clientType.IsInterface
                    ? clientType.GetInterfaceCustomAttributes(inherit: true)
                    : clientType.GetCustomAttributes(inherit: true).Cast<Attribute>())
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is VersionAttribute)
                .Cast<VersionAttribute>()
                .ToArray();
            if (versionAttributes.Length > 1)
                throw OuterExceptionFactory.MultipleAttributeForClientNotSupported(nameof(VersionAttribute));
            var versionAttribute = versionAttributes.SingleOrDefault();
            
            var methodVersion = methodInfo.GetCustomAttribute<VersionAttribute>();

            return methodVersion ?? versionAttribute;
        }
    }
}