using System.Linq;
using System.Net.Http;
using System.Reflection;
using NClient.Core.Attributes;
using NClient.Core.Exceptions.Factories;

namespace NClient.Core.RequestBuilders
{
    internal interface IHttpMethodProvider
    {
        HttpMethod Get(MethodInfo method);
    }

    internal class HttpMethodProvider : IHttpMethodProvider
    {
        private readonly IAttributeHelper _attributeHelper;

        public HttpMethodProvider(IAttributeHelper attributeHelper)
        {
            _attributeHelper = attributeHelper;
        }

        public HttpMethod Get(MethodInfo method)
        {
            var methodAttributes = method
                .GetCustomAttributes()
                .Select(x => _attributeHelper.IsNotSupportedMethodAttribute(x)
                    ? throw OuterExceptionFactory.MethodAttributeNotSupported(method, x.GetType().Name) : x)
                .Where(x => x.GetType().IsAssignableTo(_attributeHelper.MethodAttributeType))
                .ToArray();
            if (methodAttributes.Length > 1)
                throw OuterExceptionFactory.MultipleMethodAttributeNotSupported(method);
            var methodAttribute = methodAttributes.SingleOrDefault() 
                ?? throw OuterExceptionFactory.MethodAttributeNotFound(_attributeHelper.MethodAttributeType, method);

            return methodAttribute.GetType() switch
            { 
                { } type when type == _attributeHelper.GetAttributeType => HttpMethod.Get,
                { } type when type == _attributeHelper.PostAttributeType => HttpMethod.Post,
                { } type when type == _attributeHelper.PutAttributeType => HttpMethod.Put,
                { } type when type == _attributeHelper.DeleteAttributeType => HttpMethod.Delete,
                { } => throw OuterExceptionFactory.MethodAttributeNotSupported(method, methodAttribute.GetType().Name)
            };
        }
    }
}
