﻿using System;
using System.Reflection;
using NClient.Core.MethodBuilders.Models;
using NClient.Core.MethodBuilders.Providers;

namespace NClient.Core.MethodBuilders
{
    internal interface IMethodBuilder
    {
        Method Build(Type clientType, MethodInfo methodInfo);
    }

    internal class MethodBuilder : IMethodBuilder
    {
        private readonly IMethodAttributeProvider _methodAttributeProvider;
        private readonly IPathAttributeProvider _pathAttributeProvider;
        private readonly IHeaderAttributeProvider _headerAttributeProvider;
        private readonly IMethodParamBuilder _methodParamBuilder;

        public MethodBuilder(
            IMethodAttributeProvider methodAttributeProvider,
            IPathAttributeProvider pathAttributeProvider,
            IHeaderAttributeProvider headerAttributeProvider,
            IMethodParamBuilder methodParamBuilder)
        {
            _methodAttributeProvider = methodAttributeProvider;
            _pathAttributeProvider = pathAttributeProvider;
            _headerAttributeProvider = headerAttributeProvider;
            _methodParamBuilder = methodParamBuilder;
        }

        public Method Build(Type clientType, MethodInfo methodInfo)
        {
            var methodAttribute = _methodAttributeProvider.Get(methodInfo);
            var methodParams = _methodParamBuilder.Build(methodInfo);

            return new Method(methodInfo.Name, clientType.Name, methodAttribute, methodParams)
            {
                PathAttribute = _pathAttributeProvider.Find(clientType),
                HeaderAttributes = _headerAttributeProvider.Get(clientType, methodInfo, methodParams)
            };
        }
    }
}