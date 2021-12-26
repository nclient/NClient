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
    internal interface ITimeoutAttributeProvider
    {
        ITimeoutAttribute? Find(Type clientType, MethodInfo method, IEnumerable<MethodInfo> overridingMethods);
    }

    internal class TimeoutAttributeProvider : ITimeoutAttributeProvider
    {
        private readonly IAttributeMapper _attributeMapper;
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public TimeoutAttributeProvider(
            IAttributeMapper attributeMapper,
            IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _attributeMapper = attributeMapper;
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public ITimeoutAttribute? Find(Type clientType, MethodInfo method, IEnumerable<MethodInfo> overridingMethods)
        {
            return Find(clientType, method) ?? overridingMethods.Select(x => Find(clientType, x)).FirstOrDefault();
        }

        private TimeoutAttribute? Find(Type clientType, MethodInfo method)
        {
            var typeTimeoutAttribute = FindInType(clientType);
            var methodTimeoutAttribute = FindInMethod(method);

            return methodTimeoutAttribute ?? typeTimeoutAttribute;
        }

        private TimeoutAttribute? FindInType(Type clientType)
        {
            var timeoutAttributes = (clientType.IsInterface
                    ? clientType.GetInterfaceCustomAttributes(inherit: true)
                    : clientType.GetCustomAttributes(inherit: true).Cast<Attribute>())
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is TimeoutAttribute)
                .Cast<TimeoutAttribute>()
                .ToArray();
            
            if (timeoutAttributes.Length > 1)
                throw _clientValidationExceptionFactory.MultipleMethodAttributeNotSupported();
            
            return timeoutAttributes.SingleOrDefault();
        }
        
        private TimeoutAttribute? FindInMethod(MethodInfo method)
        {
            var timeoutAttributes = method.GetCustomAttributes()
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is TimeoutAttribute)
                .Cast<TimeoutAttribute>()
                .ToArray();
            
            if (timeoutAttributes.Length > 1)
                throw _clientValidationExceptionFactory.MultipleMethodAttributeNotSupported();
            
            return timeoutAttributes.SingleOrDefault();
        }
    }
}
