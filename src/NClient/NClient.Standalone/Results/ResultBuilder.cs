using System;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Standalone.Results
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
        
        public object? Build(Type resultType, IHttpResponse response, ISerializer serializer)
        {
            var genericResultType = typeof(Result<,>).MakeGenericType(resultType.GetGenericArguments()[0], resultType.GetGenericArguments()[1]);

            if (response.IsSuccessful)
            {
                var value = serializer.Deserialize(response.Content.ToString(), resultType.GetGenericArguments()[0]);
                return Activator.CreateInstance(genericResultType, value, default);
            }
            
            var error = serializer.Deserialize(response.Content.ToString(), resultType.GetGenericArguments()[1]);
            return Activator.CreateInstance(genericResultType, default, error);
        }
    }
}
