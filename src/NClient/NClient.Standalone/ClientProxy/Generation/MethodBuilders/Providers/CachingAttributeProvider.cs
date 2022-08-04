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
    internal interface ICachingAttributeProvider
    {
        ICachingAttribute? Find(Type clientType, MethodInfo method, IEnumerable<MethodInfo> overridingMethods);
    }

    internal class CachingAttributeProvider : ICachingAttributeProvider
    {
        private readonly IAttributeMapper _attributeMapper;
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public CachingAttributeProvider(
            IAttributeMapper attributeMapper,
            IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _attributeMapper = attributeMapper;
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public ICachingAttribute? Find(Type clientType, MethodInfo method, IEnumerable<MethodInfo> overridingMethods)
        {
            return Find(clientType, method) ?? overridingMethods.Select(x => Find(clientType, x)).FirstOrDefault();
        }

        private CachingAttribute? Find(Type clientType, MethodInfo method)
        {
            var typeTimeoutAttribute = FindInType(clientType);
            var methodTimeoutAttribute = FindInMethod(method);

            return methodTimeoutAttribute ?? typeTimeoutAttribute;
        }

        private CachingAttribute? FindInType(Type clientType)
        {
            var timeoutAttributes = (clientType.IsInterface
                    ? clientType.GetInterfaceCustomAttributes(inherit: true)
                    : clientType.GetCustomAttributes(inherit: true).Cast<Attribute>())
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is CachingAttribute)
                .Cast<CachingAttribute>()
                .ToArray();
            
            if (timeoutAttributes.Length > 1)
                throw _clientValidationExceptionFactory.MultipleMethodAttributeNotSupported();
            
            return timeoutAttributes.SingleOrDefault();
        }
        
        private CachingAttribute? FindInMethod(MethodInfo method)
        {
            var timeoutAttributes = method.GetCustomAttributes()
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is CachingAttribute)
                .Cast<CachingAttribute>()
                .ToArray();
            
            if (timeoutAttributes.Length > 1)
                throw _clientValidationExceptionFactory.MultipleMethodAttributeNotSupported();
            
            return timeoutAttributes.SingleOrDefault();
        }
    }
}
