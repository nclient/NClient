﻿using System;
using System.Net.Http;
using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport.Http.System
{
    public class SystemResiliencePolicySettings : IResiliencePolicySettings<HttpRequestMessage, HttpResponseMessage>
    {
        public int MaxRetries { get; }
        public Func<int, TimeSpan> GetDelay { get; }
        public Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool> ShouldRetry { get; }
        
        public SystemResiliencePolicySettings(
            int maxRetries, 
            Func<int, TimeSpan> getDelay, 
            Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool> shouldRetry)
        {
            MaxRetries = maxRetries;
            GetDelay = getDelay;
            ShouldRetry = shouldRetry;
        }
    }
}