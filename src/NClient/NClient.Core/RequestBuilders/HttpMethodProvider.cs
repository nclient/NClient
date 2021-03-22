using System.Linq;
using System.Net.Http;
using System.Reflection;
using NClient.Core.Attributes;
using NClient.Core.Attributes.Clients;
using NClient.Core.Attributes.Clients.Methods;
using NClient.Core.Exceptions.Factories;

namespace NClient.Core.RequestBuilders
{
    internal interface IHttpMethodProvider
    {
        HttpMethod Get(MethodInfo method);
    }

    internal class HttpMethodProvider : IHttpMethodProvider
    {
        private readonly IAttributeMapper _attributeMapper;

        public HttpMethodProvider(IAttributeMapper attributeMapper)
        {
            _attributeMapper = attributeMapper;
        }

        public HttpMethod Get(MethodInfo method)
        {
            var methodAttributes = method
                .GetCustomAttributes()
                .Select(x => _attributeMapper.TryMap(x))
                .ToArray();
            if (methodAttributes.Any(x => x is ClientAttribute))
                throw OuterExceptionFactory.MethodAttributeNotSupported(method, nameof(ClientAttribute));

            var httpMethodAttributes = methodAttributes
                .Where(x => x is AsHttpMethodAttribute)
                .ToArray();
            if (httpMethodAttributes.Length > 1)
                throw OuterExceptionFactory.MultipleMethodAttributeNotSupported(method);
            var httpMethodAttribute = httpMethodAttributes.SingleOrDefault() 
                ?? throw OuterExceptionFactory.MethodAttributeNotFound(typeof(AsHttpMethodAttribute), method);

            return httpMethodAttribute switch
            { 
                AsHttpGetAttribute => HttpMethod.Get,
                AsHttpPostAttribute => HttpMethod.Post,
                AsHttpPutAttribute => HttpMethod.Put,
                AsHttpDeleteAttribute => HttpMethod.Delete,
                { } => throw OuterExceptionFactory.MethodAttributeNotSupported(method, httpMethodAttribute.GetType().Name),
                _ => throw InnerExceptionFactory.NullReference(nameof(httpMethodAttribute))
            };
        }
    }
}
