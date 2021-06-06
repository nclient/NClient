using System.Linq;
using System.Reflection;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.Mappers;

namespace NClient.Core.MethodBuilders.Providers
{
    internal interface IParamAttributeProvider
    {
        ParamAttribute Get(ParameterInfo paramInfo);
    }

    internal class ParamAttributeProvider : IParamAttributeProvider
    {
        private readonly IAttributeMapper _attributeMapper;

        public ParamAttributeProvider(IAttributeMapper attributeMapper)
        {
            _attributeMapper = attributeMapper;
        }

        public ParamAttribute Get(ParameterInfo paramInfo)
        {
            var paramAttributes = paramInfo
                .GetCustomAttributes()
                .Select(attribute => _attributeMapper.TryMap(attribute))
                .Where(x => x is ParamAttribute)
                .Cast<ParamAttribute>()
                .ToArray();
            if (paramAttributes.Length > 1)
                throw ClientValidationExceptionFactory.MultipleParameterAttributeNotSupported(paramInfo.Name);
            var paramAttribute = paramAttributes.SingleOrDefault() ?? GetAttributeForImplicitParameter(paramInfo);

            return paramAttribute;
        }

        private static ParamAttribute GetAttributeForImplicitParameter(ParameterInfo paramInfo)
        {
            return paramInfo.ParameterType.IsPrimitive()
                ? new QueryParamAttribute()
                : new BodyParamAttribute();
        }
    }
}