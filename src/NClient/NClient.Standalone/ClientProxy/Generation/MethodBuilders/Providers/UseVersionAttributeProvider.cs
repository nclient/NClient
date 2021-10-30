using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Core.Helpers;
using NClient.Core.Mappers;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Generation.MethodBuilders.Providers
{
    internal interface IUseVersionAttributeProvider
    {
        IUseVersionAttribute? Find(Type clientType, MethodInfo methodInfo, IEnumerable<MethodInfo> overridingMethods);
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

        public IUseVersionAttribute? Find(Type clientType, MethodInfo methodInfo, IEnumerable<MethodInfo> overridingMethods)
        {
            return Find(clientType, methodInfo) ?? overridingMethods.Select(x => Find(clientType, x)).FirstOrDefault();
        }

        private IUseVersionAttribute? Find(Type clientType, MethodInfo methodInfo)
        {
            var useVersionAttributes = (clientType.IsInterface
                    ? clientType.GetInterfaceCustomAttributes(inherit: true)
                    : clientType.GetCustomAttributes(inherit: true).Cast<Attribute>())
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is IUseVersionAttribute)
                .Cast<IUseVersionAttribute>()
                .ToArray();
            if (useVersionAttributes.Length > 1)
                throw _clientValidationExceptionFactory.MultipleAttributeForClientNotSupported(nameof(IUseVersionAttribute));
            var useVersionAttribute = useVersionAttributes.SingleOrDefault();

            var methodUseVersionAttribute = methodInfo
                    .GetCustomAttributes()
                    .SingleOrDefault(x => x is IUseVersionAttribute)
                as IUseVersionAttribute;

            return methodUseVersionAttribute ?? useVersionAttribute;
        }
    }
}
