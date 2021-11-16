using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Mapping.Results
{
    public class ResponseToResultMapper : IResponseMapper<IRequest, IResponse>
    {
        public bool CanMap(Type resultType, IResponseContext<IRequest, IResponse> responseContext)
        {
            if (!resultType.IsGenericType)
                return false;
            
            return resultType.GetGenericTypeDefinition() == typeof(IResult<,>)
                || resultType.GetGenericTypeDefinition() == typeof(Result<,>);
        }
        
        public Task<object?> MapAsync(Type resultType, IResponseContext<IRequest, IResponse> responseContext, 
            ISerializer serializer, CancellationToken cancellationToken)
        {
            var genericResultType = typeof(Result<,>).MakeGenericType(resultType.GetGenericArguments()[0], resultType.GetGenericArguments()[1]);

            if (responseContext.Response.IsSuccessful)
            {
                var value = serializer.Deserialize(responseContext.Response.Content.ToString(), resultType.GetGenericArguments()[0]);
                return Task.FromResult<object?>(Activator.CreateInstance(genericResultType, value, default));
            }
            
            var error = serializer.Deserialize(responseContext.Response.Content.ToString(), resultType.GetGenericArguments()[1]);
            return Task.FromResult<object?>(Activator.CreateInstance(genericResultType, default, error));
        }
    }
}
