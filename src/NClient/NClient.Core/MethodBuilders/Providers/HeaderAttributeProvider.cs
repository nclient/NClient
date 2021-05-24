using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.MethodBuilders.Models;

namespace NClient.Core.MethodBuilders.Providers
{
    public interface IHeaderAttributeProvider
    {
        HeaderAttribute[] Get(Type clientType, MethodInfo methodInfo, IEnumerable<MethodParam> methodParams);
    }

    public class HeaderAttributeProvider : IHeaderAttributeProvider
    {
        public HeaderAttribute[] Get(Type clientType, MethodInfo methodInfo, IEnumerable<MethodParam> methodParams)
        {
            var clientHeaders = clientType.GetCustomAttributes<HeaderAttribute>().ToArray();
            var methodHeaders = methodInfo.GetCustomAttributes<HeaderAttribute>().ToArray();
            var headerAttributes = methodHeaders.Reverse().Concat(clientHeaders.Reverse()).DistinctBy(x => x.Name).ToArray();

            var headerAttributeNames = headerAttributes
                .Select(x => x.Name)
                .ToArray();
            var headerParamNames = methodParams
                .Where(x => x.Attribute is HeaderParamAttribute)
                .Select(x => x.Name)
                .ToArray();
            var duplicateHeaderNames = headerAttributeNames.Intersect(headerParamNames).ToArray();
            if (duplicateHeaderNames.Any())
                throw OuterExceptionFactory.HeaderParamDuplicatesStaticHeader(duplicateHeaderNames);

            return headerAttributes;
        }
    }
}