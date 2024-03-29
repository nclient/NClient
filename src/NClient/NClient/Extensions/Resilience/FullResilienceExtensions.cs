﻿using System;
using System.Net.Http;
using NClient.Common.Helpers;
using NClient.Providers.Transport;
using NClient.Providers.Transport.SystemNetHttp;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class FullResilienceExtensions
    {
        /// <summary>Sets a resilience policy for all types of methods.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="getDelay">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="shouldRetry">The predicate to filter the results this policy will handle.</param>
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithFullResilience<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> optionalBuilder,
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            
            return optionalBuilder.WithPollyFullResilience(
                new DefaultSystemNetHttpResiliencePolicySettings(maxRetries, getDelay, shouldRetry));
        }

        /// <summary>Sets a resilience policy for all types of methods.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="getDelay">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="shouldRetry">The predicate to filter the results this policy will handle.</param>
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithFullResilience(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> optionalBuilder,
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            
            return optionalBuilder.WithPollyFullResilience(
                new DefaultSystemNetHttpResiliencePolicySettings(maxRetries, getDelay, shouldRetry));
        }
    }
}
