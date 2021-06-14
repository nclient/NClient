using System;
using System.Net;
using NClient.Annotations;
using NClient.Annotations.Auth;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Mappers;

namespace NClient.Mappers
{
    internal class AspNetAttributeMapper : IAttributeMapper
    {
        public Attribute? TryMap(Attribute attribute)
        {
            return attribute.GetType() switch
            {
                { Name: "ApiControllerAttribute" } => new ApiAttribute(),

                { Name: "RouteAttribute" } => new PathAttribute(GetTemplate(attribute)) { Order = GetOrder(attribute) },

                { Name: "HttpGetAttribute" } => new GetMethodAttribute(GetTemplate(attribute)) { Order = GetOrder(attribute) },
                { Name: "HttpHeadAttribute" } => new HeadMethodAttribute(GetTemplate(attribute)) { Order = GetOrder(attribute) },
                { Name: "HttpPostAttribute" } => new PostMethodAttribute(GetTemplate(attribute)) { Order = GetOrder(attribute) },
                { Name: "HttpPutAttribute" } => new PutMethodAttribute(GetTemplate(attribute)) { Order = GetOrder(attribute) },
                { Name: "HttpDeleteAttribute" } => new DeleteMethodAttribute(GetTemplate(attribute)) { Order = GetOrder(attribute) },
                { Name: "HttpOptionsAttribute" } => new OptionsMethodAttribute(GetTemplate(attribute)) { Order = GetOrder(attribute) },
#if !NETSTANDARD2_0
                { Name: "HttpPatchAttribute" } => new PatchMethodAttribute(GetTemplate(attribute)) { Order = GetOrder(attribute) },
#endif

                { Name: "ProducesResponseTypeAttribute" } x => new ResponseAttribute(
                    type: GetProperty<Type>(attribute, "Type"),
                    statusCode: GetProperty<HttpStatusCode>(attribute, "StatusCode")),

                { Name: "AllowAnonymousAttribute" } => new AnonymousAttribute(),
                { Name: "AuthorizeAttribute" } => new AuthorizedAttribute(GetProperty<string>(attribute, name: "Policy"))
                {
                    Roles = GetProperty<string>(attribute, name: "Roles"),
                    AuthenticationSchemes = GetProperty<string>(attribute, name: "AuthenticationSchemes")
                },

                { Name: "FromRouteAttribute" } => new RouteParamAttribute(),
                { Name: "FromQueryAttribute" } => new QueryParamAttribute(),
                { Name: "FromHeaderAttribute" } => new HeaderParamAttribute(),
                { Name: "FromBodyAttribute" } => new BodyParamAttribute(),

                { } => null,
                _ => throw new ArgumentNullException(nameof(attribute))
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
                ?? throw new ArgumentException($"Property '{name}' not found.", nameof(name));
            return (T)property.GetValue(attribute);
        }
    }
}
