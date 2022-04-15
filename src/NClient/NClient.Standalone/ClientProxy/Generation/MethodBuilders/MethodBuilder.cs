using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Core.Helpers;
using NClient.Core.Helpers.EqualityComparers;
using NClient.Exceptions;
using NClient.Invocation;
using NClient.Standalone.ClientProxy.Generation.MethodBuilders.Providers;

namespace NClient.Standalone.ClientProxy.Generation.MethodBuilders
{
    internal interface IMethodBuilder
    {
        IMethod Build(Type clientType, MethodInfo methodInfo, Type returnType);
    }

    internal class MethodBuilder : IMethodBuilder
    {
        private readonly ConcurrentDictionary<MethodInfo, Method> _cache;
        private readonly IOperationAttributeProvider _operationAttributeProvider;
        private readonly IUseVersionAttributeProvider _useVersionAttributeProvider;
        private readonly IPathAttributeProvider _pathAttributeProvider;
        private readonly IHeaderAttributeProvider _headerAttributeProvider;
        private readonly ITimeoutAttributeProvider _timeoutAttributeProvider;
        private readonly IMethodParamBuilder _methodParamBuilder;

        public MethodBuilder(
            IOperationAttributeProvider operationAttributeProvider,
            IUseVersionAttributeProvider useVersionAttributeProvider,
            IPathAttributeProvider pathAttributeProvider,
            IHeaderAttributeProvider headerAttributeProvider,
            ITimeoutAttributeProvider timeoutAttributeProvider,
            IMethodParamBuilder methodParamBuilder)
        {
            _cache = new ConcurrentDictionary<MethodInfo, Method>();
            _operationAttributeProvider = operationAttributeProvider;
            _useVersionAttributeProvider = useVersionAttributeProvider;
            _pathAttributeProvider = pathAttributeProvider;
            _headerAttributeProvider = headerAttributeProvider;
            _timeoutAttributeProvider = timeoutAttributeProvider;
            _methodParamBuilder = methodParamBuilder;
        }

        public IMethod Build(Type clientType, MethodInfo methodInfo, Type returnType)
        {
            if (_cache.TryGetValue(methodInfo, out var cachedMethod))
                return cachedMethod;
            
            var overridingMethods = new List<MethodInfo>();
            var isOverridingMethod = methodInfo.GetCustomAttribute<OverrideAttribute>() is not null;
            if (isOverridingMethod)
            {
                if (clientType.HasMultipleInheritance())
                    throw new ClientValidationException($"The {nameof(OverrideAttribute)} cannot be used if there is multiple inheritance.");

                var overridingMethodInfoEqualityComparer = new OverridingMethodInfoEqualityComparer();
                var allOverridingMethods = clientType.GetInterfaceMethods(inherit: true)
                    .Where(x => overridingMethodInfoEqualityComparer.Equals(x, methodInfo))
                    .Except(new[] { methodInfo })
                    .ToArray();
                overridingMethods.Add(allOverridingMethods.First());
                overridingMethods.AddRange(allOverridingMethods.Skip(1)
                    .TakeWhile(x => x.GetCustomAttribute<OverrideAttribute>() is not null)
                    .ToArray());
            }

            var operationAttribute = _operationAttributeProvider.Get(methodInfo, overridingMethods);
            var methodParams = _methodParamBuilder.Build(methodInfo, overridingMethods);

            var method = new Method(methodInfo.Name, methodInfo, clientType.Name, clientType, 
                operationAttribute, methodParams, returnType)
            {
                PathAttribute = _pathAttributeProvider.Find(clientType),
                UseVersionAttribute = _useVersionAttributeProvider.Find(clientType, methodInfo, overridingMethods),
                MetadataAttributes = _headerAttributeProvider.Find(clientType, methodInfo, overridingMethods, methodParams),
                TimeoutAttribute = _timeoutAttributeProvider.Find(clientType, methodInfo, overridingMethods)
            };

            _cache.TryAdd(methodInfo, method);
            return method;
        }
    }
}
