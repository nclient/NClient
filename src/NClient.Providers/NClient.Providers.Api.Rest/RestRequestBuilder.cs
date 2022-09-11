using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Core.AspNetRouting;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors;
using NClient.Core.Helpers.ObjectToKeyValueConverters;
using NClient.Models;
using NClient.Providers.Api.Rest.Exceptions.Factories;
using NClient.Providers.Api.Rest.Helpers;
using NClient.Providers.Api.Rest.Models;
using NClient.Providers.Api.Rest.Providers;
using NClient.Providers.Authorization;
using NClient.Providers.Host;
using NClient.Providers.Transport;

namespace NClient.Providers.Api.Rest
{
    internal class RestRequestBuilder : IRequestBuilder
    {
        private readonly ConcurrentDictionary<MethodInfo, RouteTemplate> _routeTemplatesCache;
        
        private readonly IRouteTemplateProvider _routeTemplateProvider;
        private readonly IRouteProvider _routeProvider;
        private readonly IRequestTypeProvider _requestTypeProvider;
        private readonly IFormUrlEncoder _formUrlEncoder;
        private readonly IObjectToKeyValueConverter _objectToKeyValueConverter;
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;
        private readonly IToolset _toolset;

        public RestRequestBuilder(
            IRouteTemplateProvider routeTemplateProvider,
            IRouteProvider routeProvider,
            IRequestTypeProvider requestTypeProvider,
            IFormUrlEncoder formUrlEncoder,
            IObjectToKeyValueConverter objectToKeyValueConverter,
            IClientValidationExceptionFactory clientValidationExceptionFactory,
            IToolset toolset)
        {
            _routeTemplatesCache = new ConcurrentDictionary<MethodInfo, RouteTemplate>();

            _routeTemplateProvider = routeTemplateProvider;
            _routeProvider = routeProvider;
            _requestTypeProvider = requestTypeProvider;
            _formUrlEncoder = formUrlEncoder;
            _objectToKeyValueConverter = objectToKeyValueConverter;
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
            _toolset = toolset;
        }
        
        public async Task<IRequest> BuildAsync(Guid requestId, IHost host, IAuthorization authorization,
            IMethodInvocation methodInvocation, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var requestType = _requestTypeProvider.Get(methodInvocation.Method.Operation);
            if (!_routeTemplatesCache.TryGetValue(methodInvocation.Method.Info, out var routeTemplate))
            {
                routeTemplate = _routeTemplateProvider.Get(methodInvocation.Method);
                _routeTemplatesCache.TryAdd(methodInvocation.Method.Info, routeTemplate);
            }
            var methodParameters = methodInvocation.Method.Params
                .Select((methodParam, index) => new MethodParameter(
                    methodParam.Name,
                    methodParam.Type,
                    methodInvocation.Arguments.ElementAtOrDefault(index),
                    methodParam.Attribute))
                .ToArray();
            var route = _routeProvider
                .Build(routeTemplate, methodInvocation.Method.ClientName, methodInvocation.Method.Name, methodParameters, methodInvocation.Method.UseVersionAttribute);

            var uri = await host.TryGetUriAsync(cancellationToken).ConfigureAwait(false);
            if (string.IsNullOrEmpty(uri?.Host))
                throw _clientValidationExceptionFactory.HostUriNotDefined();
            
            var resource = new Uri(Path.Combine(uri.ToString(), route));
            var request = new Request(requestId, resource, requestType);

            var authorizationTokens = await authorization.TryGetAccessTokensAsync(cancellationToken).ConfigureAwait(false);
            var authorizationToken = authorizationTokens?.TryGet(uri);
            if (authorizationToken is not null)
                request.AddMetadata(
                    name: HttpRequestHeader.Authorization.ToString(), 
                    value: $"{authorizationToken.Scheme} {authorizationToken.Value}");

            var headerAttributes = methodInvocation.Method.MetadataAttributes;
            foreach (var headerAttribute in headerAttributes)
            {
                request.AddMetadata(headerAttribute.Name, headerAttribute.Value);
            }

            var urlParams = methodParameters
                .Where(x => x.Attribute is IPropertyParamAttribute && x.Value != null);
            foreach (var uriParam in urlParams)
            {
                var keyValuePairs = _objectToKeyValueConverter
                    .Convert(uriParam.Value, uriParam.Name, new QueryMemberNameSelector());
                foreach (var propertyKeyValue in keyValuePairs)
                {
                    request.AddParameter(propertyKeyValue.Key, propertyKeyValue.Value ?? "");
                }
            }

            var headerParams = methodParameters
                .Where(x => x.Attribute is IMetadataParamAttribute && x.Value != null);
            foreach (var headerParam in headerParams)
            {
                if (!headerParam.Type.IsPrimitive())
                    throw _clientValidationExceptionFactory.ComplexTypeInHeaderNotSupported(headerParam.Name);
                request.AddMetadata(headerParam.Name, headerParam.Value!.ToString());
            }
            request.AddMetadata("Accept", _toolset.Serializer.ContentType);

            var bodyParams = methodParameters
                .Where(x => x.Attribute is IContentParamAttribute && x.Value != null)
                .ToArray();
            if (bodyParams.Length > 1)
                throw _clientValidationExceptionFactory.MultipleBodyParametersNotSupported();
            if (bodyParams.Length == 0)
                return request;
            
            var bodyParam = bodyParams.SingleOrDefault();

            switch (bodyParam?.Value)
            {
                case IStreamContent streamContent:
                {
                    var metadata = new MetadataContainer(streamContent.Metadatas)
                    {
                        new Metadata("Content-Encoding", streamContent.Encoding.WebName),
                        new Metadata("Content-Type", streamContent.ContentType),
                        new Metadata("Content-Disposition", $"attachment; name=\"{streamContent.Name}\"")
                    };
                    
                    request.Content = new Content(streamContent.Value, streamContent.Encoding.WebName, metadata);
                    break;
                }
                case IFormFile formFile:
                {
                    var formFileHeaders = formFile.Headers
                        .SelectMany(header => header.Value
                            .Select(value => new Metadata(header.Key, value)));
                    
                    var metadata = new MetadataContainer(formFileHeaders)
                    {
                        new Metadata("Content-Type", formFile.ContentType),
                        new Metadata("Content-Disposition", $"attachment; name=\"{formFile.Name}\"; filename=\"{formFile.FileName}\"")
                    };
                    
                    request.Content = new Content(formFile.OpenReadStream(), encoding: null, metadata);
                    break;
                }
                case { } customObject when bodyParam.Attribute is IFormParamAttribute:
                {
                    var properties = _objectToKeyValueConverter
                        .Convert(customObject, bodyParam.Name, new BodyMemberNameSelector(), useRootNameAsPrefix: false)
                        .Select(x => new KeyValuePair<string, string?>(x.Key, x.Value?.ToString()));
                    var formUrlEncodedContent = _formUrlEncoder.GetContentByteArray(properties!);
                    var metadata = new MetadataContainer
                    {
                        new Metadata("Content-Encoding", Encoding.UTF8.WebName),
                        new Metadata("Content-Type", "application/x-www-form-urlencoded"),
                        new Metadata("Content-Length", formUrlEncodedContent.Length.ToString())
                    };
                    
                    request.Content = new Content(new MemoryStream(formUrlEncodedContent), Encoding.UTF8.WebName, metadata);
                    break;
                }
                case { } customObject:
                {
                    var bodyJson = _toolset.Serializer.Serialize(customObject);
                    var bodyBytes = Encoding.UTF8.GetBytes(bodyJson);
                    var metadata = new MetadataContainer
                    {
                        new Metadata("Content-Encoding", Encoding.UTF8.WebName),
                        new Metadata("Content-Type", _toolset.Serializer.ContentType),
                        new Metadata("Content-Length", bodyBytes.Length.ToString())
                    };
                    
                    request.Content = new Content(new MemoryStream(bodyBytes), Encoding.UTF8.WebName, metadata);
                    break;
                }
            }

            return request;
        }
    }
}
