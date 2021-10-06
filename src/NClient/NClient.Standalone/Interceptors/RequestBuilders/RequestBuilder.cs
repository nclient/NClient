using System;
using System.Collections.Generic;
using System.Linq;
using NClient.Abstractions.HttpClients;
using NClient.Annotations.Parameters;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors;
using NClient.Standalone.Exceptions.Factories;
using NClient.Standalone.Helpers.ObjectToKeyValueConverters;
using NClient.Standalone.Interceptors.MethodBuilders.Models;
using NClient.Standalone.Interceptors.RequestBuilders.Models;

namespace NClient.Standalone.Interceptors.RequestBuilders
{
    internal interface IRequestBuilder
    {
        IHttpRequest Build(Guid requestId, Uri host, Method method, IEnumerable<object> arguments);
    }

    internal class RequestBuilder : IRequestBuilder
    {
        private readonly IRouteTemplateProvider _routeTemplateProvider;
        private readonly IRouteProvider _routeProvider;
        private readonly IHttpMethodProvider _httpMethodProvider;
        private readonly IObjectToKeyValueConverter _objectToKeyValueConverter;
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public RequestBuilder(
            IRouteTemplateProvider routeTemplateProvider,
            IRouteProvider routeProvider,
            IHttpMethodProvider httpMethodProvider,
            IObjectToKeyValueConverter objectToKeyValueConverter,
            IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _routeTemplateProvider = routeTemplateProvider;
            _routeProvider = routeProvider;
            _httpMethodProvider = httpMethodProvider;
            _objectToKeyValueConverter = objectToKeyValueConverter;
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public IHttpRequest Build(Guid requestId, Uri host, Method method, IEnumerable<object> arguments)
        {
            var httpMethod = _httpMethodProvider.Get(method.Attribute);
            var routeTemplate = _routeTemplateProvider.Get(method);
            var paramValuePairs = method.Params
                .Select((methodParam, index) => new Parameter(
                    methodParam.Name,
                    methodParam.Type,
                    arguments.ElementAtOrDefault(index),
                    methodParam.Attribute))
                .ToArray();
            var route = _routeProvider
                .Build(routeTemplate, method.ClientName, method.Name, paramValuePairs, method.UseVersionAttribute);

            var uri = UriHelper.Combine(host, route);
            var request = new HttpRequest(requestId, uri, httpMethod);

            var headerAttributes = method.HeaderAttributes;
            foreach (var headerAttribute in headerAttributes)
            {
                request.AddHeader(headerAttribute.Name, headerAttribute.Value);
            }

            var urlParams = paramValuePairs
                .Where(x => x.Attribute is QueryParamAttribute && x.Value != null);
            foreach (var uriParam in urlParams)
            {
                var keyValuePairs = _objectToKeyValueConverter
                    .Convert(uriParam.Value, uriParam.Name, new QueryMemberNameSelector());
                foreach (var propertyKeyValue in keyValuePairs)
                {
                    request.AddParameter(propertyKeyValue.Key, propertyKeyValue.Value ?? "");
                }
            }

            var headerParams = paramValuePairs
                .Where(x => x.Attribute is HeaderParamAttribute && x.Value != null);
            foreach (var headerParam in headerParams)
            {
                if (!headerParam.Type.IsPrimitive())
                    throw _clientValidationExceptionFactory.ComplexTypeInHeaderNotSupported(headerParam.Name);
                request.AddHeader(headerParam.Name, headerParam.Value!.ToString());
            }

            var bodyParams = paramValuePairs
                .Where(x => x.Attribute is BodyParamAttribute && x.Value != null)
                .ToArray();
            if (bodyParams.Length > 1)
                throw _clientValidationExceptionFactory.MultipleBodyParametersNotSupported();
            request.Body = bodyParams.SingleOrDefault()?.Value;

            return request;
        }
    }
}
