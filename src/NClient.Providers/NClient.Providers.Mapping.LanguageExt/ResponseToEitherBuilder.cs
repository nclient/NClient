﻿using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.DataTypes.Serialisation;
using NClient.Common.Helpers;
using NClient.Providers.Transport;

namespace NClient.Providers.Mapping.LanguageExt
{
    internal class ResponseToEitherBuilder : IResponseMapper<IRequest, IResponse>
    {
        private readonly IToolset _toolset;
        
        public ResponseToEitherBuilder(IToolset toolset)
        {
            _toolset = toolset;
        }
        
        public bool CanMap(Type resultType, IResponseContext<IRequest, IResponse> responseContext)
        {
            if (!resultType.IsGenericType)
                return false;

            return resultType.GetGenericTypeDefinition() == typeof(Either<,>);
        }
        
        public async Task<object?> MapAsync(Type resultType, IResponseContext<IRequest, IResponse> responseContext, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var stringContent = await responseContext.Response.Content.Stream
                .ReadToEndAsync(responseContext.Response.Content.Encoding ?? Encoding.UTF8, cancellationToken)
                .ConfigureAwait(false);
            
            if (responseContext.Response.IsSuccessful)
            {
                var value = _toolset.Serializer.Deserialize(stringContent, resultType.GetGenericArguments()[1]);
                return BuildEither(resultType.GetGenericArguments()[0], left: null, resultType.GetGenericArguments()[1], right: value);
            }
            
            var error = _toolset.Serializer.Deserialize(stringContent, resultType.GetGenericArguments()[0]);
            return BuildEither(resultType.GetGenericArguments()[0], left: error, resultType.GetGenericArguments()[1], right: null);
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
    