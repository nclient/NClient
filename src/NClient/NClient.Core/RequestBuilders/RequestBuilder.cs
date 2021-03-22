using System;
using System.Linq;
using System.Reflection;
using NClient.Core.Attributes.Clients.Parameters;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers;
using NClient.Providers.HttpClient;

namespace NClient.Core.RequestBuilders
{
    internal interface IRequestBuilder
    {
        HttpRequest Build(Type clientType, MethodInfo method, object[] arguments);
    }

    internal class RequestBuilder : IRequestBuilder
    {
        private readonly Uri _host;
        private readonly IRouteTemplateProvider _routeTemplateProvider;
        private readonly IRouteProvider _routeProvider;
        private readonly IHttpMethodProvider _httpMethodProvider;
        private readonly IParameterProvider _parameterProvider;
        private readonly IObjectToKeyValueConverter _objectToKeyValueConverter;

        public RequestBuilder(
            Uri host,
            IRouteTemplateProvider routeTemplateProvider,
            IRouteProvider routeProvider,
            IHttpMethodProvider httpMethodProvider, 
            IParameterProvider parameterProvider,
            IObjectToKeyValueConverter objectToKeyValueConverter)
        {
            _host = host;
            _routeTemplateProvider = routeTemplateProvider;
            _routeProvider = routeProvider;
            _httpMethodProvider = httpMethodProvider;
            _parameterProvider = parameterProvider;
            _objectToKeyValueConverter = objectToKeyValueConverter;
        }

        public HttpRequest Build(Type clientType, MethodInfo method, object[] arguments)
        {
            var httpMethod = _httpMethodProvider.Get(method);
            var routeTemplate = _routeTemplateProvider.Get(clientType, method);
            var methodParams = _parameterProvider.Get(routeTemplate, method, arguments);
            var route = _routeProvider.Build(routeTemplate, clientType.Name, method.Name, methodParams);
            var uri = new Uri(_host, route);

            var request = new HttpRequest(uri, httpMethod);

            var urlParams = methodParams
                .Where(x => x.Attribute is ToQueryAttribute && x.Value != null);
            foreach (var uriParam in urlParams)
            {
                foreach (var propertyKeyValue in _objectToKeyValueConverter.Convert(uriParam.Value, uriParam.Name))
                {
                    request.AddParameter(propertyKeyValue.Key, propertyKeyValue.Value ?? "");
                }
            }

            var headerParams = methodParams
                .Where(x => x.Attribute is ToHeaderAttribute && x.Value != null);
            foreach (var headerParam in headerParams)
            {
                if (!headerParam.Type.IsSimple())
                    throw OuterExceptionFactory.ComplexTypeInHeaderNotSupported(method, headerParam.Name);
                request.AddHeader(headerParam.Name, headerParam.Value!.ToString());
            }

            var bodyParams = methodParams
                .Where(x => x.Attribute is ToBodyAttribute && x.Value != null)
                .ToArray();
            if (bodyParams.Length > 1)
                throw OuterExceptionFactory.MultipleBodyParametersNotSupported(method);
            request.Body = bodyParams.SingleOrDefault()?.Value;

            return request;
        }
    }
}
