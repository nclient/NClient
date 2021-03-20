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
                ApiAttribute x => new RouteAttribute(x.Template ?? "") { Order = x.Order },

                AsHttpGetAttribute x => new HttpGetAttribute(x.Template ?? "") { Order = x.Order },
                AsHttpPostAttribute x => new HttpPostAttribute(x.Template ?? "") { Order = x.Order },
                AsHttpPutAttribute x => new HttpPutAttribute(x.Template ?? "") { Order = x.Order },
                AsHttpDeleteAttribute x => new HttpDeleteAttribute(x.Template ?? "") { Order = x.Order },

                ToRouteAttribute => new FromRouteAttribute(),
                ToQueryAttribute => new FromQueryAttribute(),
                ToBodyAttribute => new FromBodyAttribute(),
                ToHeaderAttribute => new FromHeaderAttribute(),

                {} => null,
                _ => throw InnerExceptionFactory.NullArgument(nameof(attribute))
            };
        }
    }
}
