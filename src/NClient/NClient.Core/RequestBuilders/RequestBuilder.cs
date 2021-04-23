using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Abstractions.HttpClients;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors;
using NClient.Core.Helpers.ObjectToKeyValueConverters;
using NClient.Core.MethodBuilders.Models;
using NClient.Core.RequestBuilders.Models;

namespace NClient.Core.RequestBuilders
{
    internal interface IRequestBuilder
    {
        HttpRequest Build(Guid requestId, Method method, IEnumerable<object> arguments);
    }

    internal class RequestBuilder : IRequestBuilder
    {
        private readonly Uri _host;
        private readonly IRouteTemplateProvider _routeTemplateProvider;
        private readonly IRouteProvider _routeProvider;
        private readonly IHttpMethodProvider _httpMethodProvider;
        private readonly IObjectToKeyValueConverter _objectToKeyValueConverter;

        public RequestBuilder(
            Uri host,
            IRouteTemplateProvider routeTemplateProvider,
            IRouteProvider routeProvider,
            IHttpMethodProvider httpMethodProvider,
            IObjectToKeyValueConverter objectToKeyValueConverter)
        {
            _host = host;
            _routeTemplateProvider = routeTemplateProvider;
            _routeProvider = routeProvider;
            _httpMethodProvider = httpMethodProvider;
            _objectToKeyValueConverter = objectToKeyValueConverter;
        }

        public HttpRequest Build(Guid requestId, Method method, IEnumerable<object> arguments)
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
            var route = _routeProvider.Build(routeTemplate, method.ClientName, method.Name, paramValuePairs);
            
            var uri = new Uri(_host, route);
            var request = new HttpRequest(requestId, uri, httpMethod);

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
                    throw OuterExceptionFactory.ComplexTypeInHeaderNotSupported(headerParam.Name);
                request.AddHeader(headerParam.Name, headerParam.Value!.ToString());
            }

            var bodyParams = paramValuePairs
                .Where(x => x.Attribute is BodyParamAttribute && x.Value != null)
                .ToArray();
            if (bodyParams.Length > 1)
                throw OuterExceptionFactory.MultipleBodyParametersNotSupported();
            request.Body = bodyParams.SingleOrDefault()?.Value;

            return request;
        }
    }
}
