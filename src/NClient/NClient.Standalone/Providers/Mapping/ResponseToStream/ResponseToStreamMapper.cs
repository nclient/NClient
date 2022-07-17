using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Mapping
{
    public class ResponseToStreamMapper : IResponseMapper<IRequest, IResponse>
    {
        public bool CanMap(Type resultType, IResponseContext<IRequest, IResponse> responseContext)
        {
            return resultType == typeof(Stream);
        }
        
        public Task<object?> MapAsync(Type resultType, IResponseContext<IRequest, IResponse> responseContext, CancellationToken cancellationToken)
        {
            return Task.FromResult<object?>(responseContext.Response.Content.Stream);
        }
    }
}
