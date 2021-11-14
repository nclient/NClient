﻿using System;
using System.Threading.Tasks;
using NClient.Providers.Transport;

namespace NClient.Providers.Api
{
    public interface IRequestBuilder
    {
        Task<IRequest> BuildAsync(Guid requestId, string resource, IMethodInvocation methodInvocation);
    }
}
