using System;
using Microsoft.AspNetCore.Mvc;
using NClient.Core.Attributes;
using NClient.Core.Attributes.Clients;
using NClient.Core.Attributes.Clients.Methods;
using NClient.Core.Attributes.Clients.Parameters;
using NClient.Core.Exceptions.Factories;

namespace NClient.AspNetProxy.Attributes
{
    public class NClientAttributeMapper : IAttributeMapper
    {
        public Attribute? TryMap(Attribute attribute)
        { 
            return attribute switch
            {
                ApiAttribute x => new RouteAttribute(x.Template),

                AsHttpGetAttribute x => x.Template is null ? new HttpGetAttribute() : new HttpGetAttribute(x.Template),
                AsHttpPostAttribute x => x.Template is null ? new HttpPostAttribute() : new HttpPostAttribute(x.Template),
                AsHttpPutAttribute x => x.Template is null ? new HttpPutAttribute() : new HttpPutAttribute(x.Template),
                AsHttpDeleteAttribute x => x.Template is null ? new HttpDeleteAttribute() : new HttpDeleteAttribute(x.Template),

                FromRouteAttribute => new ToRouteAttribute(),
                FromQueryAttribute => new ToQueryAttribute(),
                FromBodyAttribute => new ToBodyAttribute(),
                FromHeaderAttribute => new ToHeaderAttribute(),

                {} => null,
                _ => throw InnerExceptionFactory.NullArgument(nameof(attribute))
            };
        }
    }
}
