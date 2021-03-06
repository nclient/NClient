using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Mappers;

namespace NClient.Core.Interceptors.MethodBuilders.Providers
{
    internal interface IMethodAttributeProvider
    {
        MethodAttribute Get(MethodInfo method, IEnumerable<MethodInfo> overridingMethods);
    }

    internal class MethodAttributeProvider : IMethodAttributeProvider
    {
        private readonly IAttributeMapper _attributeMapper;
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public MethodAttributeProvider(
            IAttributeMapper attributeMapper,
            IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _attributeMapper = attributeMapper;
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public MethodAttribute Get(MethodInfo method, IEnumerable<MethodInfo> overridingMethods)
        {
            return Find(method) ?? overridingMethods.Select(Find).FirstOrDefault()
                ?? throw _clientValidationExceptionFactory.MethodAttributeNotFound(nameof(MethodAttribute));
        }

        private MethodAttribute? Find(MethodInfo method)
        {
            var attributes = method
                .GetCustomAttributes()
                .Select(x => _attributeMapper.TryMap(x))
                .ToArray();
            if (attributes.Any(x => x is PathAttribute))
                throw _clientValidationExceptionFactory.MethodAttributeNotSupported(nameof(PathAttribute));

            var methodAttributes = method
                .GetCustomAttributes()
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is MethodAttribute)
                .Cast<MethodAttribute>()
                .ToArray();
            if (methodAttributes.Length > 1)
                throw _clientValidationExceptionFactory.MultipleMethodAttributeNotSupported();

            return methodAttributes.SingleOrDefault();
        }
    }
}