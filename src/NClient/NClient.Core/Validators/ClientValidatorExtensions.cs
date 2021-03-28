﻿using System;
using System.Linq;
using System.Reflection;

namespace NClient.Core.Validators
{
    internal static class ClientValidationExtensions
    {
        public static void EnsureValidity<T>(this T client) where T : class
        {
            foreach (var methodInfo in typeof(T).GetMethods())
            {
                var parameters = methodInfo.GetParameters().Select(GetDefaultParameter).ToArray();
                methodInfo.Invoke(client, parameters);
            }
        }

        public static object? GetDefaultParameter(ParameterInfo parameter)
        {
            return parameter.ParameterType.IsValueType
                ? Activator.CreateInstance(parameter.ParameterType)
                : null;
        }
    }
}
