using System;
using System.Net;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Mappers;

namespace NClient.Mappers
{
    public class AspNetAttributeMapper : IAttributeMapper
    {
        public Attribute? TryMap(Attribute attribute)
        {
            return attribute.GetType() switch
            {
                { Name: "ApiControllerAttribute" } => new ApiAttribute(),

                { Name: "RouteAttribute" } => new PathAttribute(GetTemplate(attribute)) { Order = GetOrder(attribute) },

                { Name: "HttpGetAttribute" } x => new GetMethodAttribute(GetTemplate(attribute)) { Order = GetOrder(attribute) },
                { Name: "HttpPostAttribute" } x => new PostMethodAttribute(GetTemplate(attribute)) { Order = GetOrder(attribute) },
                { Name: "HttpPutAttribute" } x => new PutMethodAttribute(GetTemplate(attribute)) { Order = GetOrder(attribute) },
                { Name: "HttpDeleteAttribute" } x => new DeleteMethodAttribute(GetTemplate(attribute)) { Order = GetOrder(attribute) },

                { Name: "ProducesResponseTypeAttribute" } x => new ResponseAttribute(
                    type: GetProperty<Type>(attribute, "Type"), 
                    statusCode: GetProperty<HttpStatusCode>(attribute, "StatusCode")),
                
                { Name: "FromRouteAttribute" } => new RouteParamAttribute(),
                { Name: "FromQueryAttribute" } => new QueryParamAttribute(),
                { Name: "FromHeaderAttribute" } => new HeaderParamAttribute(),
                { Name: "FromBodyAttribute" } => new BodyParamAttribute(),

                { } => null,
                _ => throw InnerExceptionFactory.NullArgument(nameof(attribute))
            };
        }

        private static string GetTemplate(Attribute attribute)
        {
            return GetProperty<string>(attribute, "Template");
        }

        private static int GetOrder(Attribute attribute)
        {
            return GetProperty<int>(attribute, "Order");
        }
        
        private static T GetProperty<T>(Attribute attribute, string name)
        {
            var property = attribute.GetType().GetProperty(name)
                ?? throw InnerExceptionFactory.ArgumentException($"Property '{name}' not found", nameof(name));
            return (T)property.GetValue(attribute);
        } 
    }
}
