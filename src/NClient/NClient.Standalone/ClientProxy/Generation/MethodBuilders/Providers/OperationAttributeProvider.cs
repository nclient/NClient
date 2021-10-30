using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Core.Mappers;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Generation.MethodBuilders.Providers
{
    internal interface IOperationAttributeProvider
    {
        IOperationAttribute Get(MethodInfo method, IEnumerable<MethodInfo> overridingMethods);
    }

    internal class OperationAttributeProvider : IOperationAttributeProvider
    {
        private readonly IAttributeMapper _attributeMapper;
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public OperationAttributeProvider(
            IAttributeMapper attributeMapper,
            IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _attributeMapper = attributeMapper;
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public IOperationAttribute Get(MethodInfo method, IEnumerable<MethodInfo> overridingMethods)
        {
            return Find(method) ?? overridingMethods.Select(Find).FirstOrDefault()
                ?? throw _clientValidationExceptionFactory.MethodAttributeNotFound(nameof(IOperationAttribute));
        }

        private IOperationAttribute? Find(MethodInfo method)
        {
            var attributes = method
                .GetCustomAttributes()
                .Select(x => _attributeMapper.TryMap(x))
                .ToArray();
            if (attributes.Any(x => x is IPathAttribute))
                throw _clientValidationExceptionFactory.MethodAttributeNotSupported(nameof(IPathAttribute));

            var operationAttributes = method
                .GetCustomAttributes()
                .Select(x => _attributeMapper.TryMap(x))
                .Where(x => x is IOperationAttribute)
                .Cast<IOperationAttribute>()
                .ToArray();
            if (operationAttributes.Length > 1)
                throw _clientValidationExceptionFactory.MultipleMethodAttributeNotSupported();

            return operationAttributes.SingleOrDefault();
        }
    }
}
