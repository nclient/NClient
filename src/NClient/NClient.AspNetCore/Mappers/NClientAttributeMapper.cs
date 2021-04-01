using System;
using Microsoft.AspNetCore.Mvc;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Mappers;

namespace NClient.AspNetCore.Mappers
{
    public class NClientAttributeMapper : IAttributeMapper
    {
        public Attribute? TryMap(Attribute attribute)
        { 
            return attribute switch
            {
                ApiAttribute => new ApiControllerAttribute(),

                PathAttribute x => new RouteAttribute(x.Template) { Order = x.Order, Name = x.Name },

                GetMethodAttribute x => x.Template is null 
                    ? new HttpGetAttribute { Order = x.Order, Name = x.Name }
                    : new HttpGetAttribute(x.Template) { Order = x.Order, Name = x.Name },
                PostMethodAttribute x => x.Template is null
                    ? new HttpPostAttribute { Order = x.Order, Name = x.Name }
                    : new HttpPostAttribute(x.Template) { Order = x.Order, Name = x.Name },
                PutMethodAttribute x => x.Template is null
                    ? new HttpPutAttribute { Order = x.Order, Name = x.Name }
                    : new HttpPutAttribute(x.Template) { Order = x.Order, Name = x.Name },
                DeleteMethodAttribute x => x.Template is null
                    ? new HttpDeleteAttribute { Order = x.Order, Name = x.Name }
                    : new HttpDeleteAttribute(x.Template) { Order = x.Order, Name = x.Name },

                RouteParamAttribute x => new FromRouteAttribute { Name = x.Name },
                QueryParamAttribute x => new FromQueryAttribute { Name = x.Name },
                BodyParamAttribute => new FromBodyAttribute(),
                HeaderParamAttribute x => new FromHeaderAttribute { Name = x.Name },

                {} => null,
                _ => throw InnerExceptionFactory.NullArgument(nameof(attribute))
            };
        }
    }
}
