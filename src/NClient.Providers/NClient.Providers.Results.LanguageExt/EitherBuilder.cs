using System;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.DataTypes.Serialisation;
using NClient.Abstractions.Providers.HttpClient;
using NClient.Abstractions.Providers.Results;
using NClient.Abstractions.Providers.Serialization;

namespace NClient.Providers.Results.LanguageExt
{
    public class EitherBuilder : IResultBuilder<IHttpResponse>
    {
        public bool CanBuild(Type resultType, IHttpResponse response)
        {
            if (!resultType.IsGenericType)
                return false;

            return resultType.GetGenericTypeDefinition() == typeof(Either<,>);
        }
        
        public Task<object?> BuildAsync(Type resultType, IHttpResponse response, ISerializer serializer)
        {
            if (response.IsSuccessful)
            {
                var value = serializer.Deserialize(response.Content.ToString(), resultType.GetGenericArguments()[1]);
                return Task.FromResult(BuildEither(resultType.GetGenericArguments()[0], left: null, resultType.GetGenericArguments()[1], right: value));
            }
            
            var error = serializer.Deserialize(response.Content.ToString(), resultType.GetGenericArguments()[0]);
            return Task.FromResult(BuildEither(resultType.GetGenericArguments()[0], left: error, resultType.GetGenericArguments()[1], right: null));
        }

        private object? BuildEither(Type leftType, object? left, Type rightType, object? right)
        {
            var eitherData = BuildEitherData(leftType, left, rightType, right);
            var eitherType = typeof(Either<,>).MakeGenericType(leftType, rightType);
            var arrayOfEitherData = Array.CreateInstance(eitherData.GetType(), length: 1);
            arrayOfEitherData.SetValue(eitherData, index: 0);
            return Activator.CreateInstance(eitherType, arrayOfEitherData);
        }
        
        private object BuildEitherData(Type leftType, object? left, Type rightType, object? right)
        {
            if (left is not null)
            {
                var rightMethodInfo = typeof(EitherData).GetMethod("Left")!.MakeGenericMethod(leftType, rightType);
                return rightMethodInfo.Invoke(obj: null, new[] { left });
            }
            
            if (right is not null)
            {
                var rightMethodInfo = typeof(EitherData).GetMethod("Right")!.MakeGenericMethod(leftType, rightType);
                return rightMethodInfo.Invoke(obj: null, new[] { right });
            }

            throw new ArgumentException($"Either left or right is expected. But the right is {right?.GetType().Name ?? "null"}, and the left is {left?.GetType().Name ?? "null"}.");
        }
    }
}
