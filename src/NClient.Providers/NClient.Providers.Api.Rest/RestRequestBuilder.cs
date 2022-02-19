using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NClient.Annotations;
using NClient.Core.AspNetRouting;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors;
using NClient.Core.Helpers.ObjectToKeyValueConverters;
using NClient.Providers.Api.Rest.Exceptions.Factories;
using NClient.Providers.Api.Rest.Models;
using NClient.Providers.Api.Rest.Providers;
using NClient.Providers.Transport;

namespace NClient.Providers.Api.Rest
{
    internal class RestRequestBuilder : IRequestBuilder
    {
        private readonly ConcurrentDictionary<MethodInfo, RouteTemplate> _routeTemplatesCache;
        
        private readonly IRouteTemplateProvider _routeTemplateProvider;
        private readonly IRouteProvider _routeProvider;
        private readonly IRequestTypeProvider _requestTypeProvider;
        private readonly IObjectToKeyValueConverter _objectToKeyValueConverter;
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;
        private readonly IToolset _toolset;

        public RestRequestBuilder(
            IRouteTemplateProvider routeTemplateProvider,
            IRouteProvider routeProvider,
            IRequestTypeProvider requestTypeProvider,
            IObjectToKeyValueConverter objectToKeyValueConverter,
            IClientValidationExceptionFactory clientValidationExceptionFactory,
            IToolset toolset)
        {
            _routeTemplatesCache = new ConcurrentDictionary<MethodInfo, RouteTemplate>();

            _routeTemplateProvider = routeTemplateProvider;
            _routeProvider = routeProvider;
            _requestTypeProvider = requestTypeProvider;
            _objectToKeyValueConverter = objectToKeyValueConverter;
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
            _toolset = toolset;
        }

        public Task<IRequest> BuildAsync(Guid requestId, Uri baseUri, 
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

            var resource = new Uri(PathHelper.Combine(baseUri.ToString(), route));
            var request = new Request(requestId, resource, requestType);

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
            if (bodyParams.Length == 1)
            {
                var bodyJson = _toolset.Serializer.Serialize(bodyParams.SingleOrDefault()?.Value);
                var bodyBytes = Encoding.UTF8.GetBytes(bodyJson);
                request.Content = new Content(bodyBytes, Encoding.UTF8.WebName, new MetadataContainer
                {
                    new Metadata("Content-Encoding", Encoding.UTF8.WebName),
                    new Metadata("Content-Type", _toolset.Serializer.ContentType),
                    new Metadata("Content-Length", bodyBytes.Length.ToString())
                });
            }

            return Task.FromResult<IRequest>(request);
        }
    }
}
