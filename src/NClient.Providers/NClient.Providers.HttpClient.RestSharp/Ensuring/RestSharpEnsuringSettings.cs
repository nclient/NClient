﻿using System;
using NClient.Abstractions.Ensuring;
using NClient.Abstractions.Resilience;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public class RestSharpEnsuringSettings : IEnsuringSettings<IRestRequest, IRestResponse>
    {
        public Predicate<ResponseContext<IRestRequest, IRestResponse>> IsSuccess { get; }
        public Action<ResponseContext<IRestRequest, IRestResponse>> OnFailure { get; }
        
        public RestSharpEnsuringSettings(
            Predicate<ResponseContext<IRestRequest, IRestResponse>> isSuccess, 
            Action<ResponseContext<IRestRequest, IRestResponse>> onFailure)
        {
            IsSuccess = isSuccess;
            OnFailure = onFailure;
        }
    }
}
