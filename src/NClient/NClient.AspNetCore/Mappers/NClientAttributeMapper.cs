using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NClient.Annotations;
using NClient.Annotations.Auth;
using NClient.Annotations.Http;
using NClient.Core.Mappers;

namespace NClient.AspNetCore.Mappers
{
    internal class NClientAttributeMapper : IAttributeMapper
    {
        public Attribute? TryMap(Attribute attribute)
        {
            return attribute switch
            {
                HttpFacadeAttribute => new ApiControllerAttribute(),

                ToVersionAttribute x => new MapToApiVersionAttribute(x.Version),
                VersionAttribute x => new ApiVersionAttribute(x.Version) { Deprecated = x.Deprecated },

                PathAttribute x => new RouteAttribute(x.Template) { Order = x.Order, Name = x.Name },

                GetMethodAttribute x => x.Path is null
                    ? new HttpGetAttribute { Order = x.Order, Name = x.Name }
                    : new HttpGetAttribute(x.Path) { Order = x.Order, Name = x.Name },
                HeadMethodAttribute x => x.Path is null
                    ? new HttpHeadAttribute { Order = x.Order, Name = x.Name }
                    : new HttpHeadAttribute(x.Path) { Order = x.Order, Name = x.Name },
                PostMethodAttribute x => x.Path is null
                    ? new HttpPostAttribute { Order = x.Order, Name = x.Name }
                    : new HttpPostAttribute(x.Path) { Order = x.Order, Name = x.Name },
                PutMethodAttribute x => x.Path is null
                    ? new HttpPutAttribute { Order = x.Order, Name = x.Name }
                    : new HttpPutAttribute(x.Path) { Order = x.Order, Name = x.Name },
                DeleteMethodAttribute x => x.Path is null
                    ? new HttpDeleteAttribute { Order = x.Order, Name = x.Name }
                    : new HttpDeleteAttribute(x.Path) { Order = x.Order, Name = x.Name },
                OptionsMethodAttribute x => x.Path is null
                    ? new HttpOptionsAttribute { Order = x.Order, Name = x.Name }
                    : new HttpOptionsAttribute(x.Path) { Order = x.Order, Name = x.Name },
                #if !NETSTANDARD2_0
                PatchMethodAttribute x => x.Path is null
                    ? new HttpPatchAttribute { Order = x.Order, Name = x.Name }
                    : new HttpPatchAttribute(x.Path) { Order = x.Order, Name = x.Name },
                #endif

                ResponseAttribute x => new ProducesResponseTypeAttribute(x.Type, (int)x.StatusCode),

                AnonymousAttribute => new AllowAnonymousAttribute(),
                AuthorizedAttribute x => new AuthorizeAttribute(x.Policy!) { Roles = x.Roles, AuthenticationSchemes = x.AuthenticationSchemes },

                RouteParamAttribute x => new FromRouteAttribute { Name = x.Name },
                QueryParamAttribute x => new FromQueryAttribute { Name = x.Name },
                BodyParamAttribute => new FromBodyAttribute(),
                HeaderParamAttribute x => new FromHeaderAttribute { Name = x.Name },

                { } => attribute,
                _ => throw new ArgumentNullException(nameof(attribute))
            };
        }
    }
}
