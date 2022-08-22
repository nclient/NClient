using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Core.Helpers;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Generation.MethodBuilders.Providers
{
    internal interface IMetadataAttributeProvider
    {
        IMetadataAttribute[] Find(Type clientType, MethodInfo methodInfo, IEnumerable<MethodInfo> overridingMethods);
    }

    internal class MetadataAttributeProvider : IMetadataAttributeProvider
    {
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public MetadataAttributeProvider(IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public IMetadataAttribute[] Find(Type clientType, MethodInfo methodInfo, IEnumerable<MethodInfo> overridingMethods)
        {
            return Find(clientType, methodInfo)
                .Concat(overridingMethods.SelectMany(x => Find(clientType, x)))
                .ToArray();
        }

        private IMetadataAttribute[] Find(Type clientType, MethodInfo methodInfo)
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

            return methodMetadatas.Reverse().Concat(clientMetadatas.Reverse()).ToArray();
        }
    }
}
