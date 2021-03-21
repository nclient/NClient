using System;
using NClient.Core.Attributes.Clients;
using NClient.Core.Attributes.Clients.Methods;
using NClient.Core.Attributes.Clients.Parameters;
using NClient.Core.Attributes.Services;
using NClient.Core.Attributes.Services.Methods;
using NClient.Core.Attributes.Services.Parameters;
using NClient.Core.Exceptions.Factories;

namespace NClient.Core.Attributes
{
    public interface IAttributeMapper
    {
        Attribute? TryMap(Attribute attribute);
    }

    public class AttributeMapper : IAttributeMapper
    {
        public Attribute? TryMap(Attribute attribute)
        {
            return attribute switch
            {
                ServiceAttribute x => new ClientAttribute(x.Template ?? "") { Order = x.Order },

                ForHttpGetAttribute x => new AsHttpGetAttribute(x.Template ?? "") { Order = x.Order },
                ForHttpPostAttribute x => new AsHttpPostAttribute(x.Template ?? "") { Order = x.Order },
                ForHttpPutAttribute x => new AsHttpPutAttribute(x.Template ?? "") { Order = x.Order },
                ForHttpDeleteAttribute x => new AsHttpDeleteAttribute(x.Template ?? "") { Order = x.Order },

                OutOfRouteAttribute => new ToRouteAttribute(),
                OutOfQueryAttribute => new ToQueryAttribute(),
                OutOfHeaderAttribute => new ToHeaderAttribute(),
                OutOfBodyAttribute => new ToBodyAttribute(),

                { } => attribute,
                _ => throw InnerExceptionFactory.NullArgument(nameof(attribute))
            };
        }
    }
}
