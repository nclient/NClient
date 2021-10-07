﻿using System;
using System.Net.Http;
using NClient.Annotations.Methods;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.Interceptors.RequestBuilders
{
    internal interface IHttpMethodProvider
    {
        HttpMethod Get(MethodAttribute methodAttribute);
    }

    internal class HttpMethodProvider : IHttpMethodProvider
    {
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public HttpMethodProvider(IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public HttpMethod Get(MethodAttribute methodAttribute)
        {
            return methodAttribute switch
            {
                GetMethodAttribute => HttpMethod.Get,
                HeadMethodAttribute => HttpMethod.Head,
                PostMethodAttribute => HttpMethod.Post,
                PutMethodAttribute => HttpMethod.Put,
                DeleteMethodAttribute => HttpMethod.Delete,
                OptionsMethodAttribute => HttpMethod.Options,
                #if !NETSTANDARD2_0
                PatchMethodAttribute => HttpMethod.Patch,
                #endif
                { } => throw _clientValidationExceptionFactory.MethodAttributeNotSupported(methodAttribute.GetType().Name),
                _ => throw new ArgumentNullException(nameof(methodAttribute))
            };
        }
    }
}
