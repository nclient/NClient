using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Annotations.Parameters;
using NClient.Core.Helpers;
using NClient.Standalone.ClientProxy.Interceptors.MethodBuilders.Models;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Interceptors.MethodBuilders.Providers
{
    internal interface IHeaderAttributeProvider
    {
        HeaderAttribute[] Find(Type clientType, MethodInfo methodInfo, IEnumerable<MethodInfo> overridingMethods, ICollection<MethodParam> methodParams);
    }

    internal class HeaderAttributeProvider : IHeaderAttributeProvider
    {
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public HeaderAttributeProvider(IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public HeaderAttribute[] Find(Type clientType, MethodInfo methodInfo, IEnumerable<MethodInfo> overridingMethods, ICollection<MethodParam> methodParams)
        {
            var returnedHeaderNames = new HashSet<string>();

            return Find(clientType, methodInfo, methodParams)
                .Concat(overridingMethods.SelectMany(x => Find(clientType, x, methodParams)))
                .Where(x =>
                {
                    if (returnedHeaderNames.Contains(x.Name))
                        return false;

                    returnedHeaderNames.Add(x.Name);
                    return true;
                })
                .ToArray();
        }

        private HeaderAttribute[] Find(Type clientType, MethodInfo methodInfo, IEnumerable<MethodParam> methodParams)
        {
            var clientHeaders = (clientType.IsInterface
                    ? clientType.GetInterfaceCustomAttributes(inherit: true)
                    : Array.Empty<HeaderAttribute>())
                .Where(x => x is HeaderAttribute)
                .Cast<HeaderAttribute>()
                .ToArray();
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
                throw _clientValidationExceptionFactory.HeaderParamDuplicatesStaticHeader(duplicateHeaderNames);

            return headerAttributes;
        }
    }
}
