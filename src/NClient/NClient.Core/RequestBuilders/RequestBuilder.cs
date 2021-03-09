using System;
using System.Linq;
using System.Reflection;
using NClient.Core.Attributes;
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
        private readonly IRouteBuilder _routeBuilder;
        private readonly IHttpMethodProvider _httpMethodProvider;
        private readonly IParameterProvider _parameterProvider;
        private readonly IObjectToKeyValueConverter _objectToKeyValueConverter;
        private readonly IAttributeHelper _attributeHelper;

        public RequestBuilder(
            Uri host,
            IRouteBuilder routeBuilder,
            IHttpMethodProvider httpMethodProvider, 
            IParameterProvider parameterProvider,
            IObjectToKeyValueConverter objectToKeyValueConverter,
            IAttributeHelper attributeHelper)
        {
            _host = host;
            _routeBuilder = routeBuilder;
            _httpMethodProvider = httpMethodProvider;
            _parameterProvider = parameterProvider;
            _objectToKeyValueConverter = objectToKeyValueConverter;
            _attributeHelper = attributeHelper;
        }

        public HttpRequest Build(Type clientType, MethodInfo method, object[] arguments)
        {
            var route = _routeBuilder.Build(clientType, method, arguments);
            var httpMethod = _httpMethodProvider.Get(method);
            var methodParams = _parameterProvider.Get(method, arguments);
            var uri = new Uri(_host, route);

            var request = new HttpRequest(uri, httpMethod);

            var urlParams = methodParams
                .Where(x => _attributeHelper.IsFromUriAttribute(x.Attribute) && x.Value != null);
            foreach (var uriParam in urlParams)
            {
                foreach (var propertyKeyValue in _objectToKeyValueConverter.Convert(uriParam.Value, uriParam.Name))
                {
                    request.AddParameter(propertyKeyValue.Key, propertyKeyValue.Value);
                }
            }

            var headerParams = methodParams
                .Where(x => _attributeHelper.IsFromHeaderAttribute(x.Attribute) && x.Value != null);
            foreach (var headerParam in headerParams)
            {
                if (!headerParam.Value.GetType().IsSimple())
                    throw OuterExceptionFactory.ComplexTypeInHeaderNotSupported(method, headerParam.Name);
                request.AddHeader(headerParam.Name, headerParam.Value!.ToString());
            }

            var bodyParams = methodParams
                .Where(x => _attributeHelper.IsFromBodyAttributeType(x.Attribute) && x.Value != null)
                .ToArray();
            if (bodyParams.Length > 1)
                throw OuterExceptionFactory.MultipleBodyParametersNotSupported(method);
            request.Body = bodyParams.SingleOrDefault()?.Value;


            return request;
        }
    }
}
