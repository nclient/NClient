using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NClient.Annotations;
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
                IHttpFacadeAttribute => new ApiControllerAttribute(),

                IToVersionAttribute x => new MapToApiVersionAttribute(x.Version),
                IVersionAttribute x => new ApiVersionAttribute(x.Version) { Deprecated = x.Deprecated },

                IPathAttribute x => new RouteAttribute(x.Template) { Order = x.Order, Name = x.Name },

                IGetMethodAttribute x => x.Path is null
                    ? new HttpGetAttribute { Order = x.Order, Name = x.Name }
                    : new HttpGetAttribute(x.Path) { Order = x.Order, Name = x.Name },
                IHeadMethodAttribute x => x.Path is null
                    ? new HttpHeadAttribute { Order = x.Order, Name = x.Name }
                    : new HttpHeadAttribute(x.Path) { Order = x.Order, Name = x.Name },
                IPostMethodAttribute x => x.Path is null
                    ? new HttpPostAttribute { Order = x.Order, Name = x.Name }
                    : new HttpPostAttribute(x.Path) { Order = x.Order, Name = x.Name },
                IPutMethodAttribute x => x.Path is null
                    ? new HttpPutAttribute { Order = x.Order, Name = x.Name }
                    : new HttpPutAttribute(x.Path) { Order = x.Order, Name = x.Name },
                IDeleteMethodAttribute x => x.Path is null
                    ? new HttpDeleteAttribute { Order = x.Order, Name = x.Name }
                    : new HttpDeleteAttribute(x.Path) { Order = x.Order, Name = x.Name },
                IOptionsMethodAttribute x => x.Path is null
                    ? new HttpOptionsAttribute { Order = x.Order, Name = x.Name }
                    : new HttpOptionsAttribute(x.Path) { Order = x.Order, Name = x.Name },
                #if !NETSTANDARD2_0
                IPatchMethodAttribute x => x.Path is null
                    ? new HttpPatchAttribute { Order = x.Order, Name = x.Name }
                    : new HttpPatchAttribute(x.Path) { Order = x.Order, Name = x.Name },
                #endif

                IResponseAttribute x => new ProducesResponseTypeAttribute(x.Type, (int) x.StatusCode),

                IAnonymousAttribute => new AllowAnonymousAttribute(),
                IAuthorizedAttribute x => new AuthorizeAttribute(x.Policy!) { Roles = x.Roles, AuthenticationSchemes = x.AuthenticationSchemes },

                IRouteParamAttribute x => new FromRouteAttribute { Name = x.Name },
                IQueryParamAttribute x => new FromQueryAttribute { Name = x.Name },
                IBodyParamAttribute => new FromBodyAttribute(),
                IFormParamAttribute => new FromFormAttribute(),
                IHeaderParamAttribute x => new FromHeaderAttribute { Name = x.Name },

                { } => attribute,
                _ => throw new ArgumentNullException(nameof(attribute))
            };
        }
    }
}
