using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Common.Helpers;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Mapping.Results
{
    public class ResponseToResultMapper : IResponseMapper<IRequest, IResponse>
    {
        private readonly IToolset _toolset;
        
        public ResponseToResultMapper(IToolset toolset)
        {
            _toolset = toolset;
        }
        
        public bool CanMap(Type resultType, IResponseContext<IRequest, IResponse> responseContext)
        {
            if (!resultType.IsGenericType)
                return false;
            
            return resultType.GetGenericTypeDefinition() == typeof(IResult<,>)
                || resultType.GetGenericTypeDefinition() == typeof(Result<,>);
        }
        
        public async Task<object?> MapAsync(Type resultType, IResponseContext<IRequest, IResponse> responseContext, CancellationToken cancellationToken)
        {
            var genericResultType = typeof(Result<,>).MakeGenericType(resultType.GetGenericArguments()[0], resultType.GetGenericArguments()[1]);

            var stringContent = await responseContext.Response.Content.Stream
                .ReadToEndAsync(responseContext.Response.Content.Encoding, cancellationToken)
                .ConfigureAwait(false);
            
            if (responseContext.Response.IsSuccessful)
            {
                var value = _toolset.Serializer.Deserialize(stringContent, resultType.GetGenericArguments()[0]);
                return Activator.CreateInstance(genericResultType, value, default);
            }
            
            var error = _toolset.Serializer.Deserialize(stringContent, resultType.GetGenericArguments()[1]);
            return Activator.CreateInstance(genericResultType, default, error);
        }
    }
}
