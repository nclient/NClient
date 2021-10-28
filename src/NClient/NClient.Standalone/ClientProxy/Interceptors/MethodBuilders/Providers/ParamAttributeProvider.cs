using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Core.Helpers;
using NClient.Core.Mappers;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Interceptors.MethodBuilders.Providers
{
    internal interface IParamAttributeProvider
    {
        IParamAttribute Get(ParameterInfo paramInfo, IEnumerable<ParameterInfo> overridingParams);
    }

    internal class ParamAttributeProvider : IParamAttributeProvider
    {
        private readonly IAttributeMapper _attributeMapper;
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public ParamAttributeProvider(
            IAttributeMapper attributeMapper,
            IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _attributeMapper = attributeMapper;
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public IParamAttribute Get(ParameterInfo paramInfo, IEnumerable<ParameterInfo> overridingParams)
        {
            return Find(paramInfo) ?? overridingParams.Select(Find).FirstOrDefault()
                ?? GetAttributeForImplicitParameter(paramInfo);
        }

        private IParamAttribute? Find(ParameterInfo paramInfo)
        {
            var paramAttributes = paramInfo
                .GetCustomAttributes()
                .Select(attribute => _attributeMapper.TryMap(attribute))
                .Where(x => x is IParamAttribute)
                .Cast<IParamAttribute>()
                .ToArray();
            if (paramAttributes.Length > 1)
                throw _clientValidationExceptionFactory.MultipleParameterAttributeNotSupported(paramInfo.Name);
            return paramAttributes.SingleOrDefault();
        }

        private static IParamAttribute GetAttributeForImplicitParameter(ParameterInfo paramInfo)
        {
            return paramInfo.ParameterType.IsPrimitive()
                ? new PropertyParamAttribute()
                : new ContentParamAttribute();
        }
    }
}
