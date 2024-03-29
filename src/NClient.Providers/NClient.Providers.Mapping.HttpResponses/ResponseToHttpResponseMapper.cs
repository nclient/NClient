﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Results.HttpResults;
using NClient.Providers.Transport;

namespace NClient.Providers.Mapping.HttpResponses
{
    internal class ResponseToHttpResponseMapper : IResponseMapper<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly IToolset _toolset;
        
        public ResponseToHttpResponseMapper(IToolset toolset)
        {
            _toolset = toolset;
        }
        
        public bool CanMap(Type resultType, IResponseContext<HttpRequestMessage, HttpResponseMessage> responseContext)
        {
            if (resultType.IsGenericType)
                return resultType.GetGenericTypeDefinition() == typeof(HttpResponse<>)
                    || resultType.GetGenericTypeDefinition() == typeof(IHttpResponse<>)
                    || resultType.GetGenericTypeDefinition() == typeof(HttpResponseWithError<>)
                    || resultType.GetGenericTypeDefinition() == typeof(IHttpResponseWithError<>)
                    || resultType.GetGenericTypeDefinition() == typeof(HttpResponseWithError<,>)
                    || resultType.GetGenericTypeDefinition() == typeof(IHttpResponseWithError<,>);
            
            return resultType == typeof(HttpResponse) 
                || resultType == typeof(IHttpResponse);
        }
        
        public async Task<object?> MapAsync(Type resultType, IResponseContext<HttpRequestMessage, HttpResponseMessage> responseContext, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var httpRequest = new HttpRequest(responseContext.Response.RequestMessage);
            var httpResponse = new HttpResponse(httpRequest, responseContext.Response);
            var content = await responseContext.Response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (resultType == typeof(HttpResponse) || resultType == typeof(IHttpResponse))
                return httpResponse;
            
            if (resultType.GetGenericTypeDefinition() == typeof(HttpResponse<>) || resultType.GetGenericTypeDefinition() == typeof(IHttpResponse<>))
            { 
                var data = httpResponse.IsSuccessful
                    ? _toolset.Serializer.Deserialize(content, resultType.GetGenericArguments()[0])
                    : null;
                var httpResponseType = typeof(HttpResponse<>).MakeGenericType(resultType.GetGenericArguments()[0]);
                return Activator.CreateInstance(httpResponseType, httpResponse, data);
            }
            
            if (resultType.GetGenericTypeDefinition() == typeof(HttpResponseWithError<>) || resultType.GetGenericTypeDefinition() == typeof(IHttpResponseWithError<>))
            {
                var error = httpResponse.IsSuccessful 
                    ? null
                    : _toolset.Serializer.Deserialize(content, resultType.GetGenericArguments()[0]);
                var httpResponseType = typeof(HttpResponseWithError<>).MakeGenericType(resultType.GetGenericArguments()[0]);
                return Activator.CreateInstance(httpResponseType, httpResponse, error);
            }
            
            if (resultType.GetGenericTypeDefinition() == typeof(HttpResponseWithError<,>) || resultType.GetGenericTypeDefinition() == typeof(IHttpResponseWithError<,>))
            {
                var data = httpResponse.IsSuccessful
                    ? _toolset.Serializer.Deserialize(content, resultType.GetGenericArguments()[0])
                    : null;
                var error = httpResponse.IsSuccessful 
                    ? null
                    : _toolset.Serializer.Deserialize(content, resultType.GetGenericArguments()[1]);
                var httpResponseType = typeof(HttpResponseWithError<,>).MakeGenericType(resultType.GetGenericArguments()[0], resultType.GetGenericArguments()[1]);
                return Activator.CreateInstance(httpResponseType, httpResponse, data, error);
            }

            throw new ArgumentException($"Result type '{resultType.Name}' is not supported.", nameof(resultType));
        }
    }
}
