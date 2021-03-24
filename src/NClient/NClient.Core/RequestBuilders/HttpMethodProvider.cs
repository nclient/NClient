using System.Linq;
using System.Net.Http;
using System.Reflection;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Mappers;

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
            if (methodAttributes.Any(x => x is PathAttribute))
                throw OuterExceptionFactory.MethodAttributeNotSupported(method, nameof(PathAttribute));

            var httpMethodAttributes = methodAttributes
                .Where(x => x is MethodAttribute)
                .ToArray();
            if (httpMethodAttributes.Length > 1)
                throw OuterExceptionFactory.MultipleMethodAttributeNotSupported(method);
            var httpMethodAttribute = httpMethodAttributes.SingleOrDefault() 
                ?? throw OuterExceptionFactory.MethodAttributeNotFound(typeof(MethodAttribute), method);

            return httpMethodAttribute switch
            { 
                GetMethodAttribute => HttpMethod.Get,
                PostMethodAttribute => HttpMethod.Post,
                PutMethodAttribute => HttpMethod.Put,
                DeleteMethodAttribute => HttpMethod.Delete,
                { } => throw OuterExceptionFactory.MethodAttributeNotSupported(method, httpMethodAttribute.GetType().Name),
                _ => throw InnerExceptionFactory.NullReference(nameof(httpMethodAttribute))
            };
        }
    }
}
