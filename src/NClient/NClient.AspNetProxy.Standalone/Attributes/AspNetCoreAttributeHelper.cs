using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using NClient.Core.Attributes;

namespace NClient.AspNetProxy.Attributes
{
    public class AspNetCoreAttributeHelper : AttributeHelperBase
    {
        public override Type ApiAttributeType { get; } = typeof(RouteAttribute);

        public override Type MethodAttributeType { get; } = typeof(HttpMethodAttribute);
        public override Type GetAttributeType { get; } = typeof(HttpGetAttribute);
        public override Type PostAttributeType { get; } = typeof(HttpPostAttribute);
        public override Type PutAttributeType { get; } = typeof(HttpPutAttribute);
        public override Type DeleteAttributeType { get; } = typeof(HttpDeleteAttribute);
        
        public override Type FromUriAttributeType { get; } = typeof(FromQueryAttribute);
        public override Type FromHeaderAttributeType { get; } = typeof(FromHeaderAttribute);
        public override Type FromBodyAttributeType { get; } = typeof(FromBodyAttribute);

        public override IReadOnlyCollection<Type> NotSupportedMethodAttributes { get; } = new HashSet<Type>
        {
            typeof(RouteAttribute)
        };
    }
}
