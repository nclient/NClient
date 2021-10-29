using System;
using System.Collections.Generic;
using NClient.Invocation;
using NClient.Providers.Api;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validator.Api
{
    public class StubRequestBuilder : IRequestBuilder
    {
        public IRequest Build(Guid requestId, string resourceRoot, Method method, IEnumerable<object> arguments)
        {
            return new Request(requestId, resourceRoot, RequestType.Custom);
        }
    }
}
