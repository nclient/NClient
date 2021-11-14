﻿using System;
using NClient.Exceptions;
using NClient.Providers.Validation;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport.Http.RestSharp
{
    public class DefaultRestSharpResponseValidatorSettings : IResponseValidatorSettings<IRestRequest, IRestResponse>
    {
        public Predicate<IResponseContext<IRestRequest, IRestResponse>> IsSuccess { get; }
        public Action<IResponseContext<IRestRequest, IRestResponse>> OnFailure { get; }
        
        public DefaultRestSharpResponseValidatorSettings() : this(
            isSuccess: responseContext => 
                responseContext.Response.IsSuccessful,
            onFailure: responseContext => 
                throw new TransportException<IRestRequest, IRestResponse>(responseContext.Request, responseContext.Response, responseContext.Response.ErrorMessage, responseContext.Response.ErrorException!))
        {
        }
        
        public DefaultRestSharpResponseValidatorSettings(
            Predicate<IResponseContext<IRestRequest, IRestResponse>> isSuccess, 
            Action<IResponseContext<IRestRequest, IRestResponse>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
