using System;
using Microsoft.AspNetCore.Mvc;
using NClient.Core.Attributes;
using NClient.Core.Attributes.Methods;
using NClient.Core.Attributes.Parameters;
using NClient.Core.Exceptions.Factories;

namespace NClient.AspNetProxy.Attributes
{
    public class AspNetAttributeMapper : IAttributeMapper
    {
        public Attribute? TryMap(Attribute attribute)
        {
            return attribute switch
            {
                RouteAttribute x => new PathAttribute(x.Template) { Order = x.Order },

                HttpGetAttribute x => new GetMethodAttribute(x.Template ?? "") { Order = x.Order },
                HttpPostAttribute x => new PostMethodAttribute(x.Template ?? "") { Order = x.Order },
                HttpPutAttribute x => new PutMethodAttribute(x.Template ?? "") { Order = x.Order },
                HttpDeleteAttribute x => new DeleteMethodAttribute(x.Template ?? "") { Order = x.Order },

                FromRouteAttribute => new RouteParamAttribute(),
                FromQueryAttribute => new QueryParamAttribute(),
                FromHeaderAttribute => new HeaderParamAttribute(),
                FromBodyAttribute => new BodyParamAttribute(),

                {} => null,
                _ => throw InnerExceptionFactory.NullArgument(nameof(attribute))
            };
        }
    }
}
