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
                RouteAttribute x => new ClientAttribute(x.Template ?? "") { Order = x.Order },
                
                HttpGetAttribute x => new AsHttpGetAttribute(x.Template ?? "") { Order = x.Order },
                HttpPostAttribute x => new AsHttpPostAttribute(x.Template ?? "") { Order = x.Order },
                HttpPutAttribute x => new AsHttpPutAttribute(x.Template ?? "") { Order = x.Order },
                HttpDeleteAttribute x => new AsHttpDeleteAttribute(x.Template ?? "") { Order = x.Order },

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
