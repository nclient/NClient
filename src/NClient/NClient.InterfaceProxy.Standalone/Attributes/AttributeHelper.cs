using System;
using System.Collections.Generic;
using NClient.Core.Attributes;
using NClient.InterfaceProxy.Attributes.Methods;
using NClient.InterfaceProxy.Attributes.Parameters;

namespace NClient.InterfaceProxy.Attributes
{
    public class AttributeHelper : AttributeHelperBase
    {
        public override Type ApiAttributeType { get; } = typeof(ApiAttribute);

        public override Type MethodAttributeType { get; } = typeof(AsHttpMethodAttribute);
        public override Type GetAttributeType { get; } = typeof(AsHttpGetAttribute);
        public override Type PostAttributeType { get; } = typeof(AsHttpPostAttribute);
        public override Type PutAttributeType { get; } = typeof(AsHttpPutAttribute);
        public override Type DeleteAttributeType { get; } = typeof(AsHttpDeleteAttribute);

        public override Type RouteParamAttributeType { get; } = typeof(ToRouteAttribute);
        public override Type UriParamAttributeType { get; } = typeof(ToQueryAttribute);
        public override Type HeaderParamAttributeType { get; } = typeof(ToHeaderAttribute);
        public override Type BodyParamAttributeType { get; } = typeof(ToBodyAttribute);

        public override IReadOnlyCollection<Type> NotSupportedMethodAttributes { get; } = new HashSet<Type>();
    }
}
