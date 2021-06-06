using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Mappers;

namespace NClient.Core.MethodBuilders.Providers
{
    internal interface IMethodAttributeProvider
    {
        MethodAttribute Get(MethodInfo method);
    }

    internal class MethodAttributeProvider : IMethodAttributeProvider
    {
        private readonly IAttributeMapper _attributeMapper;

        public MethodAttributeProvider(IAttributeMapper attributeMapper)
        {
            _attributeMapper = attributeMapper;
        }

        public MethodAttribute Get(MethodInfo method)
        {
            var attributes = method
                .GetCustomAttributes()
                .Select(x => _attributeMapper.TryMap(x))
                .ToArray();
            if (attributes.Any(x => x is PathAttribute))
                throw ClientValidationExceptionFactory.MethodAttributeNotSupported(nameof(PathAttribute));

            var methodAttributes = method
                .GetCustomAttributes()
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is MethodAttribute)
                .Cast<MethodAttribute>()
                .ToArray();
            if (methodAttributes.Length > 1)
                throw ClientValidationExceptionFactory.MultipleMethodAttributeNotSupported();
            var methodAttribute = methodAttributes.SingleOrDefault()
                ?? throw ClientValidationExceptionFactory.MethodAttributeNotFound(nameof(MethodAttribute));

            return methodAttribute;
        }
    }
}