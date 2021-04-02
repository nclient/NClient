using System;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Mappers;

namespace NClient.InterfaceProxy.Mappers
{
    public class AspNetAttributeMapper : IAttributeMapper
    {
        public Attribute? TryMap(Attribute attribute)
        {
            return attribute.GetType() switch
            {
                { Name: "RouteAttribute" } x => new PathAttribute((string)x.GetProperty("Template")!.GetValue(attribute))
                {
                    Order = (int)x.GetProperty("Order")!.GetValue(attribute)
                },

                { Name: "HttpGetAttribute" } x => new GetMethodAttribute((string)x.GetProperty("Template")!.GetValue(attribute))
                {
                    Order = (int)x.GetProperty("Order")!.GetValue(attribute)
                },
                { Name: "HttpPostAttribute" } x => new PostMethodAttribute((string)x.GetProperty("Template")!.GetValue(attribute))
                {
                    Order = (int)x.GetProperty("Order")!.GetValue(attribute)
                },
                { Name: "HttpPutAttribute" } x => new PutMethodAttribute((string)x.GetProperty("Template")!.GetValue(attribute))
                {
                    Order = (int)x.GetProperty("Order")!.GetValue(attribute)
                },
                { Name: "HttpDeleteAttribute" } x => new DeleteMethodAttribute((string)x.GetProperty("Template")!.GetValue(attribute))
                {
                    Order = (int)x.GetProperty("Order")!.GetValue(attribute)
                },

                { Name: "FromRouteAttribute" } => new RouteParamAttribute(),
                { Name: "FromQueryAttribute" } => new QueryParamAttribute(),
                { Name: "FromHeaderAttribute" } => new HeaderParamAttribute(),
                { Name: "FromBodyAttribute" } => new BodyParamAttribute(),

                { } => null,
                _ => throw InnerExceptionFactory.NullArgument(nameof(attribute))
            };
        }
    }
}
