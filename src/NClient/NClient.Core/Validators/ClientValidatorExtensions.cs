using System;
using System.Linq;
using System.Reflection;
using MoreLinq;

namespace NClient.Core.Validators
{
    internal static class ClientValidationExtensions
    {
        public static void EnsureValidity<T>(this T client) where T : class, INClient
        {
            typeof(T).GetMethods().ForEach(x =>
            {
                var parameters = x.GetParameters().Select(GetDefaultParameter).ToArray();
                x.Invoke(client, parameters);
            });
        }

        public static object? GetDefaultParameter(ParameterInfo parameter)
        {
            return parameter.ParameterType.IsValueType
                ? Activator.CreateInstance(parameter.ParameterType)
                : null;
        }
    }
}
