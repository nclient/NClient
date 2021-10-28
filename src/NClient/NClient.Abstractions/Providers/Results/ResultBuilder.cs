using System;
using System.Threading.Tasks;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;

namespace NClient.Providers.Results
{
    public class ResultBuilder : IResultBuilder<IResponse>
    {
        public bool CanBuild(Type resultType, IResponse response)
        {
            if (!resultType.IsGenericType)
                return false;
            
            return resultType.GetGenericTypeDefinition() == typeof(IResult<,>)
                || resultType.GetGenericTypeDefinition() == typeof(Result<,>);
        }
        
        public Task<object?> BuildAsync(Type resultType, IResponse response, ISerializer serializer)
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
