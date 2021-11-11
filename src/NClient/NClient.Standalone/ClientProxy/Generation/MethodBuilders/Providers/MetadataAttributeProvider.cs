using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Core.Helpers;
using NClient.Invocation;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Generation.MethodBuilders.Providers
{
    internal interface IHeaderAttributeProvider
    {
        IMetadataAttribute[] Find(Type clientType, MethodInfo methodInfo, IEnumerable<MethodInfo> overridingMethods, ICollection<IMethodParam> methodParams);
    }

    internal class MetadataAttributeProvider : IHeaderAttributeProvider
    {
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public MetadataAttributeProvider(IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public IMetadataAttribute[] Find(Type clientType, MethodInfo methodInfo, IEnumerable<MethodInfo> overridingMethods, ICollection<IMethodParam> methodParams)
        {
            var returnedMetadataNames = new HashSet<string>();

            return Find(clientType, methodInfo, methodParams)
                .Concat(overridingMethods.SelectMany(x => Find(clientType, x, methodParams)))
                .Where(x =>
                {
                    if (returnedMetadataNames.Contains(x.Name))
                        return false;

                    returnedMetadataNames.Add(x.Name);
                    return true;
                })
                .ToArray();
        }

        private IMetadataAttribute[] Find(Type clientType, MethodInfo methodInfo, IEnumerable<IMethodParam> methodParams)
        {
            var clientMetadatas = (clientType.IsInterface
                    ? clientType.GetInterfaceCustomAttributes(inherit: true)
                    : Array.Empty<Attribute>())
                .Where(x => x is IMetadataAttribute)
                .Cast<IMetadataAttribute>()
                .ToArray();
            var methodMetadatas = methodInfo
                .GetCustomAttributes()
                .Where(x => x is IMetadataAttribute)
                .Cast<IMetadataAttribute>()
                .ToArray();
            var metadataAttributes = methodMetadatas.Reverse().Concat(clientMetadatas.Reverse()).DistinctBy(x => x.Name).ToArray();

            var metadataAttributeNames = metadataAttributes
                .Select(x => x.Name)
                .ToArray();
            var metadataParamNames = methodParams
                .Where(x => x.Attribute is HeaderParamAttribute)
                .Select(x => x.Name)
                .ToArray();
            var duplicateMetadataNames = metadataAttributeNames.Intersect(metadataParamNames).ToArray();
            if (duplicateMetadataNames.Any())
                throw _clientValidationExceptionFactory.HeaderParamDuplicatesStaticHeader(duplicateMetadataNames);

            return metadataAttributes;
        }
    }
}
