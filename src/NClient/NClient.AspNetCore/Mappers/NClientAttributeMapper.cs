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
                PathAttribute x => new RouteAttribute(x.Template) { Order = x.Order, Name = x.Name },

                GetMethodAttribute x => new HttpGetAttribute(x.Template ?? "") { Order = x.Order, Name = x.Name },
                PostMethodAttribute x => new HttpPostAttribute(x.Template ?? "") { Order = x.Order, Name = x.Name },
                PutMethodAttribute x => new HttpPutAttribute(x.Template ?? "") { Order = x.Order, Name = x.Name },
                DeleteMethodAttribute x => new HttpDeleteAttribute(x.Template ?? "") { Order = x.Order, Name = x.Name },

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
