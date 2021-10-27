using System;
using System.Collections.Generic;
using NClient.Invocation;
using NClient.Providers.Transport;

namespace NClient.Providers.Api
{
    public interface IRequestBuilder
    {
        IRequest Build(Guid requestId, string resourceRoot, Method method, IEnumerable<object> arguments);
    }
}
