using System;
using Microsoft.AspNetCore.Mvc;
using NClient.Core.Attributes;
using NClient.Core.Attributes.Clients;
using NClient.Core.Attributes.Clients.Methods;
using NClient.Core.Attributes.Clients.Parameters;
using NClient.Core.Exceptions.Factories;

namespace NClient.AspNetProxy.Attributes
{
    public class AspNetAttributeMapper : IAttributeMapper
    {
        public Attribute? TryMap(Attribute attribute)
        {
            return attribute switch
            {
                RouteAttribute x => new ApiAttribute(x.Template),
                
                HttpGetAttribute x => new AsHttpGetAttribute(x.Template),
                HttpPostAttribute x => new AsHttpPostAttribute(x.Template),
                HttpPutAttribute x => new AsHttpPutAttribute(x.Template),
                HttpDeleteAttribute x => new AsHttpDeleteAttribute(x.Template),

                FromRouteAttribute => new ToRouteAttribute(),
                FromQueryAttribute => new ToQueryAttribute(),
                FromHeaderAttribute => new ToHeaderAttribute(),
                FromBodyAttribute => new ToBodyAttribute(),

                {} => null,
                _ => throw InnerExceptionFactory.NullArgument(nameof(attribute))
            };
        }
    }
}
