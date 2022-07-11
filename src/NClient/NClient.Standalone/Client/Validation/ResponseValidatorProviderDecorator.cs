﻿using System.Collections.Generic;
using System.Linq;
using NClient.Providers;
using NClient.Providers.Validation;

namespace NClient.Standalone.Client.Validation
{
    internal class ResponseValidatorProviderDecorator<TRequest, TResponse> : IResponseValidatorProvider<TRequest, TResponse>
    {
        private readonly IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> _responseValidatorProviders;
        
        public ResponseValidatorProviderDecorator(IEnumerable<IResponseValidatorProvider<TRequest, TResponse>> responseValidatorProviders)
        {
            _responseValidatorProviders = responseValidatorProviders;
        }
        
        public IResponseValidator<TRequest, TResponse> Create(IToolset toolset)
        {
            return new ResponseValidatorDecorator<TRequest, TResponse>(_responseValidatorProviders
                .Select(x => x.Create(toolset)));
        }
    }
}
