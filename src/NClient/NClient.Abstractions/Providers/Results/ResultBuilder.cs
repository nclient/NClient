using System;
using System.Threading.Tasks;
using NClient.Abstractions.Providers.Serialization;
using NClient.Abstractions.Providers.Transport;

namespace NClient.Abstractions.Providers.Results
{
    public class ResultBuilder : IResultBuilder<IHttpResponse>
    {
        public bool CanBuild(Type resultType, IHttpResponse response)
        {
            if (!resultType.IsGenericType)
                return false;
            
            return resultType.GetGenericTypeDefinition() == typeof(IResult<,>)
                || resultType.GetGenericTypeDefinition() == typeof(Result<,>);
        }
        
        public Task<object?> BuildAsync(Type resultType, IHttpResponse response, ISerializer serializer)
        {
            var genericResultType = typeof(Result<,>).MakeGenericType(resultType.GetGenericArguments()[0], resultType.GetGenericArguments()[1]);

            if (response.IsSuccessful)
            {
                var value = serializer.Deserialize(response.Content.ToString(), resultType.GetGenericArguments()[0]);
                return Task.FromResult<object?>(Activator.CreateInstance(genericResultType, value, default));
            }
            
            var error = serializer.Deserialize(response.Content.ToString(), resultType.GetGenericArguments()[1]);
            return Task.FromResult<object?>(Activator.CreateInstance(genericResultType, default, error));
        }
    }
}
