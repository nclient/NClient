using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Core.Mappers;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Generation.MethodBuilders.Providers
{
    internal interface ITimeoutAttributeProvider
    {
        TimeoutAttribute? Get(MethodInfo method, IEnumerable<MethodInfo> overridingMethods);
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

        public TimeoutAttribute? Get(MethodInfo method, IEnumerable<MethodInfo> overridingMethods)
        {
            return Find(method) ?? overridingMethods.Select(Find).FirstOrDefault();
        }

        private TimeoutAttribute? Find(MethodInfo method)
        {
            var attributes = method
                .GetCustomAttributes()
                .Select(x => _attributeMapper.TryMap(x))
                .ToArray();
            if (attributes.Any(x => x is IPathAttribute))
                throw _clientValidationExceptionFactory.MethodAttributeNotSupported(nameof(IPathAttribute));
///WTF
            var timeoutAttributes = method
                .GetCustomAttributes()
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
