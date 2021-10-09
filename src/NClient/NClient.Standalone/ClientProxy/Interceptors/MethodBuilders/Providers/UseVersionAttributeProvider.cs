using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Annotations.Versioning;
using NClient.Core.Helpers;
using NClient.Core.Mappers;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Interceptors.MethodBuilders.Providers
{
    internal interface IUseVersionAttributeProvider
    {
        UseVersionAttribute? Find(Type clientType, MethodInfo methodInfo, IEnumerable<MethodInfo> overridingMethods);
    }

    internal class UseVersionAttributeProvider : IUseVersionAttributeProvider
    {
        private readonly IAttributeMapper _attributeMapper;
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public UseVersionAttributeProvider(
            IAttributeMapper attributeMapper,
            IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _attributeMapper = attributeMapper;
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public UseVersionAttribute? Find(Type clientType, MethodInfo methodInfo, IEnumerable<MethodInfo> overridingMethods)
        {
            return Find(clientType, methodInfo) ?? overridingMethods.Select(x => Find(clientType, x)).FirstOrDefault();
        }

        private UseVersionAttribute? Find(Type clientType, MethodInfo methodInfo)
        {
            var useVersionAttributes = (clientType.IsInterface
                    ? clientType.GetInterfaceCustomAttributes(inherit: true)
                    : clientType.GetCustomAttributes(inherit: true).Cast<Attribute>())
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is UseVersionAttribute)
                .Cast<UseVersionAttribute>()
                .ToArray();
            if (useVersionAttributes.Length > 1)
                throw _clientValidationExceptionFactory.MultipleAttributeForClientNotSupported(nameof(UseVersionAttribute));
            var useVersionAttribute = useVersionAttributes.SingleOrDefault();

            var methodUseVersionAttribute = methodInfo.GetCustomAttribute<UseVersionAttribute>();

            return methodUseVersionAttribute ?? useVersionAttribute;
        }
    }
}
