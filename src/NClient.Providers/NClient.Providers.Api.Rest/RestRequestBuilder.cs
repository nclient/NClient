using System;
using System.Linq;
using System.Text;
using NClient.Annotations;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors;
using NClient.Core.Helpers.ObjectToKeyValueConverters;
using NClient.Providers.Api.Rest.Exceptions.Factories;
using NClient.Providers.Api.Rest.Models;
using NClient.Providers.Api.Rest.Providers;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;

namespace NClient.Providers.Api.Rest
{
    internal class RestRequestBuilder : IRequestBuilder
    {
        private readonly ISerializer _serializer;
        private readonly IRouteTemplateProvider _routeTemplateProvider;
        private readonly IRouteProvider _routeProvider;
        private readonly ITransportMethodProvider _transportMethodProvider;
        private readonly IObjectToKeyValueConverter _objectToKeyValueConverter;
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public RestRequestBuilder(
            ISerializer serializer,
            IRouteTemplateProvider routeTemplateProvider,
            IRouteProvider routeProvider,
            ITransportMethodProvider transportMethodProvider,
            IObjectToKeyValueConverter objectToKeyValueConverter,
            IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _serializer = serializer;
            _routeTemplateProvider = routeTemplateProvider;
            _routeProvider = routeProvider;
            _transportMethodProvider = transportMethodProvider;
            _objectToKeyValueConverter = objectToKeyValueConverter;
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public IRequest Build(Guid requestId, string resource, IMethodInvocation methodInvocation)
        {
            var requestType = _transportMethodProvider.Get(methodInvocation.Method.Operation);
            var routeTemplate = _routeTemplateProvider.Get(methodInvocation.Method);
            var methodParameters = methodInvocation.Method.Params
                .Select((methodParam, index) => new MethodParameter(
                    methodParam.Name,
                    methodParam.Type,
                    methodInvocation.Arguments.ElementAtOrDefault(index),
                    methodParam.Attribute))
                .ToArray();
            var route = _routeProvider
                .Build(routeTemplate, methodInvocation.Method.ClientName, methodInvocation.Method.Name, methodParameters, methodInvocation.Method.UseVersionAttribute);

            var endpoint = PathHelper.Combine(resource, route);
            var request = new Request(requestId, endpoint, requestType);

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
            request.AddMetadata("Accept", _serializer.ContentType);

            var bodyParams = methodParameters
                .Where(x => x.Attribute is IContentParamAttribute && x.Value != null)
                .ToArray();
            if (bodyParams.Length > 1)
                throw _clientValidationExceptionFactory.MultipleBodyParametersNotSupported();
            if (bodyParams.Length == 1)
            {
                var bodyJson = _serializer.Serialize(bodyParams.SingleOrDefault()?.Value);
                var bodyBytes = Encoding.UTF8.GetBytes(bodyJson);
                request.Content = new Content(bodyBytes, Encoding.UTF8.WebName, new MetadataContainer
                {
                    new Metadata("Content-Encoding", Encoding.UTF8.WebName),
                    new Metadata("Content-Type", _serializer.ContentType),
                    new Metadata("Content-Length", bodyBytes.Length.ToString())
                });
            }

            return request;
        }
    }
}
